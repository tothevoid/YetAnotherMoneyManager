import React, { Fragment, useEffect, useState } from 'react';
import "./Manager.scss"
import Transaction from '../../components/Transaction/Transaction';
import FundsBar from '../../components/FundsBar/FundsBar';
import { TransactionEntity } from '../../models/TransactionEntity';
import { FundEntity } from '../../models/FundEntity';
import { insertByPredicate, reorderByPredicate } from '../../utils/ArrayExtensions'
import Pagination from '../../components/Pagination/Pagination';
import TransactionStats from '../../components/TransactionStats/TransactionStats';
import { Box, Flex, SimpleGrid, Text } from '@chakra-ui/react';
import AddTransactionButton from '../../components/AddTransactionButton/AddTransactionButton';
import { getTransactions, updateTransaction } from '../../api/transactionApi';
import { FundToUpdate } from '../../api/models/fundToUpdate';
import { getFunds } from '../../api/fundApi';
import { useTranslation } from 'react-i18next';

type State = {
    transactions: TransactionEntity[],
    funds: FundEntity[],
    month: number,
    year: number
}

const getDefaultState = () => {
    return {
        month: new Date().getMonth() + 1,
        year: new Date().getFullYear(),
        transactions: [] as TransactionEntity[],
        funds: [] as FundEntity[],
    };    
}

//TODO: Simplify component
const Manager: React.FC<any> = () => {
    useEffect(() => {
        const initData = async () => {
            await initFunds();
        }
        initData();
    }, []);

    const initFunds = async () => {
        const funds = await getFunds();
        setState((currentState) => {
            return {...currentState, funds}
        })
    };

    const onFundCreated = async (addedFund: FundEntity) => {
        if (!addedFund) {
            return
        }

        setState((currentState: State) => {
            const funds = state.funds.concat(addedFund);
            return {...currentState, funds};
        });
    };

    const onFundUpdated = async (updatedFund: FundEntity) => {
        if (!updatedFund) {
            return
        }

        let fundNameChanged = false;

        setState((currentState: State) => {
            const funds = currentState.funds.map((fund: FundEntity) => {
                if (fund.id === updatedFund.id) {
                    fundNameChanged = fund.name !== updatedFund.name;
                    return {...updatedFund}
                } 
                return fund
            });

            if (!fundNameChanged) {
                return {...currentState, funds};
            }
            
            const newTransactions = currentState.transactions.map(transaction => {
                if (transaction.fundSource.id === updatedFund.id) {
                    transaction.fundSource = {...updatedFund}
                }

                return transaction;
            });
            return {...currentState, funds, transactions: newTransactions}
        });
    };

    const onFundDeleted = async (deletedFund: FundEntity) => {
        if (!deletedFund) {
            return;
        }

        setState((currentState: State) => {
            const funds = currentState.funds.filter((fund: FundEntity) => fund.id !== deletedFund.id)
            return { ...currentState, funds }
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
        recalculateFund(createdTransaction, 1);
    };

    const onTransactionDeleted = async (deletedTransaction: TransactionEntity) => { 
        if (!deletedTransaction) {
            return;
        }

        deleteTransaction(deletedTransaction);
        recalculateFund(deletedTransaction, -1);
    };

    const onTransactionUpdated = async (updatedTransaction: TransactionEntity) => {
        const affectedFunds = await updateTransaction(updatedTransaction);

        if (!affectedFunds.length) {
            return;
        }

        const newFunds = recalculateFunds(affectedFunds);
        if (!isCurrentMonthTransaction(updatedTransaction)) {
            deleteTransaction(updatedTransaction);
            setState((currentState) => {
                return {...currentState, funds: newFunds}; 
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
            return {...currentState, transactions: updatedTransactions, funds: newFunds}  
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

    const recalculateFund = (changedTransaction: TransactionEntity, sign: number) => {
        const {funds} = state;
        if (changedTransaction.fundSource && changedTransaction.fundSource.id && funds && funds.length !== 0){
            const newFunds = funds.map((fund: FundEntity) => {
                if (fund.id === changedTransaction.fundSource.id){
                    fund.balance += changedTransaction.moneyQuantity * sign;
                } 
                return fund;
            });
            setState((currentState) => {
                return {...currentState, funds: newFunds};
            });
        }
    };

    const recalculateFunds = (fundsToUpdate: FundToUpdate[]) => {
        const {funds} = state;
        const newFunds = funds.map((fund: FundEntity) => {
            const findResult = fundsToUpdate.find((fundToUpdate) => fundToUpdate.fundId === fund.id);
            if (findResult){
                fund.balance += findResult.delta;
            } 
            return fund;
        });
        return newFunds;
    };

    const [state, setState] = useState<State>(getDefaultState);

    const {transactions, funds, 
        year, month} = state;
    
    const { t } = useTranslation();

    return (
        <div>
            <FundsBar 
                onAddFundCallback = {onFundCreated}
                onUpdateFundCallback = {onFundUpdated}
                onDeleteFundCallback = {onFundDeleted} 
                funds = {funds}>
            </FundsBar>
            <SimpleGrid columns={2} gap={16}>
                <Box>
                    <Flex justifyContent={"space-between"}>
                        <Text fontSize="2xl" fontWeight={600}>{t("manager_transactions_title")}</Text>
                        <AddTransactionButton 
                            fundSources={funds} 
                            onTransactionCreated={onTransactionCreated}/>
                    </Flex>
                    <Pagination year={year} month={month} onPageSwitched={loadTransactions}></Pagination>
                    <div className="transactions">
                        {
                        (transactions.length !== 0) ?
                        transactions.map((transaction: TransactionEntity) => {       
                            return <Transaction key={transaction.id} transaction={transaction} 
                                onDelete={onTransactionDeleted} onUpdate={onTransactionUpdated}
                                fundSources={funds}>
                            </Transaction>
                        }):
                        <div className="empty-transactions">{t("manager_transactions_add_transaction")}</div>
                        }
                    </div>
                </Box>
                <Box>
                    {
                        transactions.length > 0 ?
                            <TransactionStats funds={funds} transactions={transactions}/>:
                            <Fragment/>
                    }
                </Box>
            </SimpleGrid>
        </div>
    );
}


export default Manager;