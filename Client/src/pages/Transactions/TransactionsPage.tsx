import "./TransactionsPage.scss"

import React, { Fragment, useEffect, useRef, useState } from 'react';
import Transaction from './components/Transaction/Transaction';
import { AccountEntity } from '../../models/accounts/AccountEntity';
import { insertByPredicate, reorderByPredicate } from '../../shared/utilities/arrayUtilities'
import Pagination from './components/Pagination/Pagination';
import TransactionStats from './components/TransactionStats/TransactionStats';
import { Box, Checkbox, Flex, SimpleGrid, Text } from '@chakra-ui/react';
import { createTransaction, getTransactions, updateTransaction } from '../../api/transactions/transactionApi';
import { getAccountsByTypes } from '../../api/accounts/accountApi';
import { useTranslation } from 'react-i18next';
import { TransactionEntity } from "../../models/transactions/TransactionEntity";
import { formatDate } from "../../shared/utilities/formatters/dateFormatter";
import { formatMoneyByCurrencyCulture } from "../../shared/utilities/formatters/moneyFormatter";
import { useUserProfile } from "../../../features/UserProfileSettingsModal/hooks/UserProfileContext";
import ShowModalButton from "../../shared/components/ShowModalButton/ShowModalButton";
import { BaseModalRef } from "../../shared/utilities/modalUtilities";
import TransactionModal from "./modals/TransactionModal/TransactionModal";

type State = {
    transactions: TransactionEntity[],
    accounts: AccountEntity[],
    month: number,
    year: number
    showSystem: boolean
}

const getDefaultState = (): State => {
    return {
        month: new Date().getMonth() + 1,
        year: new Date().getFullYear(),
        transactions: [] as TransactionEntity[],
        accounts: [] as AccountEntity[],
        showSystem: false
    };
}

const TransactionsPage: React.FC<any> = () => {
    useEffect(() => {
        const initData = async () => {
            await initAccounts();
        }
        initData();
    }, []);

    const initAccounts = async () => {
        const accounts = await getAccountsByTypes([
            "6ea1867f-c067-412c-b443-8b9bc2467202",	// Credit card
            "a08f5553-379e-4294-a2e5-75e88219433c",	// Cash
            "cda2ce07-551e-48cf-988d-270c0d022866"	// Debit card
        ], true);
        setState((currentState) => {
            return {...currentState, accounts}
        })
    };

    const loadTransactions = async (month: number, year: number) => {
        const transactions = await getTransactions(month, year, state.showSystem);
        setState((currentState) => {
            return {...currentState, month, year, transactions}
        })
    };

    const onTransactionCreated = async (createdTransaction: TransactionEntity) => {
        if (!createdTransaction) {
            return;
        }

        if (!isCurrentMonthTransaction(createdTransaction)) {
            return;
        }

        const {transactions} = state;
        const newTransactions = insertByPredicate(transactions, createdTransaction, 
            (transactionElm: TransactionEntity) => (transactionElm.date <= createdTransaction.date));
        setState((currentState) => {
            return {...currentState, transactions: newTransactions}
        });
    };

    const onTransactionDeleted = async (deletedTransaction: TransactionEntity) => { 
        if (!deletedTransaction) {
            return;
        }

        deleteTransaction(deletedTransaction);
    };

    const onTransactionUpdated = async (updatedTransaction: TransactionEntity) => {
        const affectedAccounts = await updateTransaction(updatedTransaction);

        if (!affectedAccounts.length) {
            return;
        }

        if (!isCurrentMonthTransaction(updatedTransaction)) {
            deleteTransaction(updatedTransaction);
            return;
        }

        const {transactions} = state;
        const newTransactions = transactions.map((transaction: TransactionEntity) => {
            return transaction.id === updatedTransaction.id ?
                {...updatedTransaction}:
                transaction;
        });

        // TODO: date is not changed => reorder is not required 
        const updatedTransactions = reorderByPredicate(newTransactions, updatedTransaction, 
            (currentElement: TransactionEntity) => (currentElement.date <= updatedTransaction.date),
            (currentElement: TransactionEntity) => (currentElement.id !== updatedTransaction.id));

        setState((currentState) => {
            return {...currentState, transactions: updatedTransactions}  
        });
    };

    const deleteTransaction = (deletingTransaction: TransactionEntity) => {
        setState((currentState: State) => {
            const transactions = currentState.transactions
                .filter((transaction: TransactionEntity) => transaction.id !== deletingTransaction.id)
            return { ...currentState, transactions }
        });
    }

    const isCurrentMonthTransaction = (transaction: TransactionEntity): boolean => {
        const {month, year} = state;

        return transaction.date.getMonth() === month - 1 &&
               transaction.date.getFullYear() === year
    }

    const [state, setState] = useState<State>(getDefaultState);

    const {transactions, accounts, year, month} = state;
    
    const { t, i18n } = useTranslation();

    const { user } = useUserProfile();

    const onCheckboxChanged = async (checkboxChange: any) => {
		setState((currentState) => {
			return {...currentState, showSystem: !!checkboxChange.checked};
		});
	}

    useEffect(() => {
        const loadTransactions = async () => {
            const transactions = await getTransactions(state.month, state.year, state.showSystem);
            setState((currentState) => {
                return {...currentState, month, year, transactions}
            })
        };
        loadTransactions();
    }, [state.showSystem]);

    const groupedTransactions = transactions.reduce((accumulator: Map<number, TransactionEntity[]>, currentValue: TransactionEntity) => {
        const day = currentValue.date.getDate();
        if (accumulator.has(day)) {
            accumulator.get(day).push(currentValue);
        } else {
            accumulator.set(day, [currentValue]);
        }

        return accumulator;
    }, new Map<number, TransactionEntity[]>);

    const calculateSummary = (transactions: TransactionEntity[]) => {
        const transaction = transactions.reduce((accumulator, currentValue) => {
            accumulator += currentValue.amount * currentValue.account.currency.rate;
            return accumulator;
        }, 0)

        return formatMoneyByCurrencyCulture(transaction, user?.currency.name);
    }

    const addTransactionModalRef = useRef<BaseModalRef>(null);
            
    const onAddTransactionClick = () => {
        addTransactionModalRef.current?.openModal()
    }

    const onTransactionAdded = async (transaction: TransactionEntity) => {
        const createdTransaction = await createTransaction(transaction);
        if (!createdTransaction) {
            return;
        }

        onTransactionCreated(createdTransaction);
    }

    return (
        <Box color="text_primary" paddingTop={4} paddingBottom={4}>
            <SimpleGrid columns={2} gap={16}>
                <Box>
                    <Flex justifyContent={"space-between"}>
                        <Text fontSize="2xl" fontWeight={600}>{t("manager_transactions_title")}</Text>
                        <ShowModalButton buttonTitle={t("manager_transactions_add_transaction")} onClick={onAddTransactionClick}>
                            <TransactionModal modalRef={addTransactionModalRef} accounts={state.accounts} onSaved={onTransactionAdded}/>
                        </ShowModalButton>
                    </Flex>
                    <Box marginBlock={"10px"}>
                        <Checkbox.Root checked={state.showSystem} onCheckedChange={onCheckboxChanged} variant="solid">
                            <Checkbox.HiddenInput />
                            <Checkbox.Control />
                            <Checkbox.Label color="text_primary">{t("manager_transactions_show_system")}</Checkbox.Label>
                        </Checkbox.Root>
                    </Box>
                    <Pagination year={year} month={month} onPageSwitched={loadTransactions}></Pagination>
                    <div className="transactions">
                        {
                            transactions.length ?
                                [...groupedTransactions.entries()].map(([transactionDay, transactions]) => 
                                    <Fragment>
                                        <Flex justifyContent="space-between">
                                            <Text key={transactionDay}>{formatDate(new Date(state.year, state.month - 1, transactionDay), i18n, false)}</Text>
                                            <Text key={transactionDay}>{calculateSummary(transactions)}</Text>
                                        </Flex>
                                        
                                        {
                                            transactions.map((transaction: TransactionEntity) => {       
                                                return <Transaction key={transaction.id} transaction={transaction} 
                                                    onDelete={onTransactionDeleted} onUpdate={onTransactionUpdated}
                                                    accounts={accounts}>
                                                </Transaction>
                                            })
                                        }
                                    </Fragment>
                                ):
                                <div className="empty-transactions">{t("manager_transactions_add_transaction")}</div>
                        }
                    </div>
                </Box>
                <Box>
                    {
                        transactions.length > 0 && <TransactionStats accounts={accounts} transactions={transactions}/>
                    }
                </Box>
            </SimpleGrid>
        </Box>
    );
}

export default TransactionsPage;