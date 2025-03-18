import React, { Fragment, useEffect, useState } from 'react';
import "./Manager.scss"
import Transaction from '../../components/Transaction/Transaction';
import { TransactionEntity } from '../../models/TransactionEntity';
import { AccountEntity } from '../../models/AccountEntity';
import { insertByPredicate, reorderByPredicate } from '../../utils/ArrayExtensions'
import Pagination from '../../components/Pagination/Pagination';
import TransactionStats from '../../components/TransactionStats/TransactionStats';
import { Box, Container, Flex, SimpleGrid, Text } from '@chakra-ui/react';
import AddTransactionButton from '../../components/AddTransactionButton/AddTransactionButton';
import { getTransactions, updateTransaction } from '../../api/transactionApi';
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
    
    const { t } = useTranslation();

    return (
        <Container paddingTop={4} paddingBottom={4}>
            <SimpleGrid columns={2} gap={16}>
                <Box>
                    <Flex justifyContent={"space-between"}>
                        <Text fontSize="2xl" fontWeight={600}>{t("manager_transactions_title")}</Text>
                        <AddTransactionButton 
                            accounts={accounts} 
                            onTransactionCreated={onTransactionCreated}/>
                    </Flex>
                    <Pagination year={year} month={month} onPageSwitched={loadTransactions}></Pagination>
                    <div className="transactions">
                        {
                        (transactions.length !== 0) ?
                        transactions.map((transaction: TransactionEntity) => {       
                            return <Transaction key={transaction.id} transaction={transaction} 
                                onDelete={onTransactionDeleted} onUpdate={onTransactionUpdated}
                                accounts={accounts}>
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
        </Container>
    );
}


export default Manager;