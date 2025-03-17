import React, { Fragment, useEffect, useState } from 'react';
import "./Manager.scss"
import Transaction from '../../components/Transaction/Transaction';
import AccountsList from '../../components/AccountsList/AccountsList';
import { TransactionEntity } from '../../models/TransactionEntity';
import { AccountEntity } from '../../models/AccountEntity';
import { insertByPredicate, reorderByPredicate } from '../../utils/ArrayExtensions'
import Pagination from '../../components/Pagination/Pagination';
import TransactionStats from '../../components/TransactionStats/TransactionStats';
import { Box, Flex, SimpleGrid, Text } from '@chakra-ui/react';
import AddTransactionButton from '../../components/AddTransactionButton/AddTransactionButton';
import { getTransactions, updateTransaction } from '../../api/transactionApi';
import { AccountToUpdate } from '../../api/models/accountToUpdate';
import { getAccounts } from '../../api/accountApi';
import { useTranslation } from 'react-i18next';

type State = {
    transactions: TransactionEntity[],
    accounts: AccountEntity[],
    month: number,
    year: number
}

const getDefaultState = () => {
    return {
        month: new Date().getMonth() + 1,
        year: new Date().getFullYear(),
        transactions: [] as TransactionEntity[],
        accounts: [] as AccountEntity[],
    };    
}

//TODO: Simplify component
const Manager: React.FC<any> = () => {
    useEffect(() => {
        const initData = async () => {
            await initAccounts();
        }
        initData();
    }, []);

    const initAccounts = async () => {
        const accounts = await getAccounts();
        setState((currentState) => {
            return {...currentState, accounts}
        })
    };

    const onAccountCreated = async (addedAccount: AccountEntity) => {
        if (!addedAccount) {
            return
        }

        setState((currentState: State) => {
            const accounts = state.accounts.concat(addedAccount);
            return {...currentState, accounts};
        });
    };

    const onAccountUpdated = async (updatedAccount: AccountEntity) => {
        if (!updatedAccount) {
            return
        }

        let accountNameChanged = false;

        setState((currentState: State) => {
            const accounts = currentState.accounts.map((account: AccountEntity) => {
                if (account.id === updatedAccount.id) {
                    accountNameChanged = account.name !== updatedAccount.name;
                    return {...updatedAccount}
                } 
                return account
            });

            if (!accountNameChanged) {
                return {...currentState, accounts};
            }
            
            const newTransactions = currentState.transactions.map(transaction => {
                if (transaction.fundSource.id === updatedAccount.id) {
                    transaction.fundSource = {...updatedAccount}
                }

                return transaction;
            });
            return {...currentState, accounts, transactions: newTransactions}
        });
    };

    const onAccountDeleted = async (deletedAccount: AccountEntity) => {
        if (!deletedAccount) {
            return;
        }

        setState((currentState: State) => {
            const accounts = currentState.accounts.filter((account: AccountEntity) => account.id !== deletedAccount.id)
            return { ...currentState, accounts }
        });
    };

    const loadTransactions = async (month: number, year: number) => {
        const transactions = await getTransactions(month, year);
        setState((currentState) => {
            return {...currentState, month, year, transactions}
        })
    };

    const onTransactionCreated = async (createdTransaction: TransactionEntity) => {
        if (!createdTransaction) {
            return;
        }

        if (isCurrentMonthTransaction(createdTransaction)) {
            const {transactions} = state;
            const newTransactions = insertByPredicate(transactions, createdTransaction, 
                (transactionElm: TransactionEntity) => (transactionElm.date <= createdTransaction.date));
            setState((currentState) => {
                return {...currentState, transactions: newTransactions}
            });
        }
        recalculateAccount(createdTransaction, 1);
    };

    const onTransactionDeleted = async (deletedTransaction: TransactionEntity) => { 
        if (!deletedTransaction) {
            return;
        }

        deleteTransaction(deletedTransaction);
        recalculateAccount(deletedTransaction, -1);
    };

    const onTransactionUpdated = async (updatedTransaction: TransactionEntity) => {
        const affectedAccounts = await updateTransaction(updatedTransaction);

        if (!affectedAccounts.length) {
            return;
        }

        const newAccounts = recalculateAccounts(affectedAccounts);
        if (!isCurrentMonthTransaction(updatedTransaction)) {
            deleteTransaction(updatedTransaction);
            setState((currentState) => {
                return {...currentState, accounts: newAccounts}; 
            });
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
            return {...currentState, transactions: updatedTransactions, accounts: newAccounts}  
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

    const recalculateAccount = (changedTransaction: TransactionEntity, sign: number) => {
        const {accounts} = state;
        if (changedTransaction.fundSource && changedTransaction.fundSource.id && accounts && accounts.length !== 0){
            const newAccounts = accounts.map((account: AccountEntity) => {
                if (account.id === changedTransaction.fundSource.id){
                    account.balance += changedTransaction.moneyQuantity * sign;
                } 
                return account;
            });
            setState((currentState) => {
                return {...currentState, accounts: newAccounts};
            });
        }
    };

    const recalculateAccounts = (accountsToUpdated: AccountToUpdate[]) => {
        const {accounts} = state;
        const newAccounts = accounts.map((account: AccountEntity) => {
            const findResult = accountsToUpdated.find((accountToUpdate) => accountToUpdate.accountId === account.id);
            if (findResult){
                account.balance += findResult.delta;
            } 
            return account;
        });
        return newAccounts;
    };

    const [state, setState] = useState<State>(getDefaultState);

    const {transactions, accounts, 
        year, month} = state;
    
    const { t } = useTranslation();

    return (
        <div>
            <AccountsList 
                onAddAccountCallback = {onAccountCreated}
                onUpdateAccountCallback = {onAccountUpdated}
                onDeleteAccountCallback = {onAccountDeleted} 
                accounts = {accounts}>
            </AccountsList>
            <SimpleGrid columns={2} gap={16}>
                <Box>
                    <Flex justifyContent={"space-between"}>
                        <Text fontSize="2xl" fontWeight={600}>{t("manager_transactions_title")}</Text>
                        <AddTransactionButton 
                            fundSources={accounts} 
                            onTransactionCreated={onTransactionCreated}/>
                    </Flex>
                    <Pagination year={year} month={month} onPageSwitched={loadTransactions}></Pagination>
                    <div className="transactions">
                        {
                        (transactions.length !== 0) ?
                        transactions.map((transaction: TransactionEntity) => {       
                            return <Transaction key={transaction.id} transaction={transaction} 
                                onDelete={onTransactionDeleted} onUpdate={onTransactionUpdated}
                                fundSources={accounts}>
                            </Transaction>
                        }):
                        <div className="empty-transactions">{t("manager_transactions_add_transaction")}</div>
                        }
                    </div>
                </Box>
                <Box>
                    {
                        transactions.length > 0 ?
                            <TransactionStats accounts={accounts} transactions={transactions}/>:
                            <Fragment/>
                    }
                </Box>
            </SimpleGrid>
        </div>
    );
}


export default Manager;