import React, { Fragment } from 'react';
import "./Manager.scss"
import Transaction from '../../components/Transaction/Transaction';
import FundsBar from '../../components/FundsBar/FundsBar';
import { TransactionEntity } from '../../models/TransactionEntity';
import { FundEntity } from '../../models/FundEntity';
import { insertByPredicate, reorderByPredicate } from '../../utils/ArrayExtensions'
import config from '../../config' 
import Pagination from '../../components/Pagination/Pagination';
import TransactionStats from '../../components/TransactionStats/TransactionStats';
import { logPromiseError, checkPromiseStatus } from '../../utils/PromiseUtils';
import { TransactionType } from '../../models/TransactionType';
import { Box, Flex, SimpleGrid, Text } from '@chakra-ui/react';
import AddTransactionButton from '../../components/AddTransactionButton/AddTransactionButton';
import { createTransaction, deleteTransaction, getTransactions, updateTransaction } from '../../api/transactionApi';
import { FundToUpdate } from '../../api/models/fundToUpdate';
import { createFund, deleteFund, getFunds, updateFund } from '../../api/fundApi';

type State = {
    transactions: TransactionEntity[],
    funds: FundEntity[],
    transactionTypes: TransactionType[],
    month: number,
    year: number
}

class Manager extends React.Component<any, State> {

    state = {
        month: new Date().getMonth() + 1,
        year: new Date().getFullYear(),
        transactions: [] as TransactionEntity[],
        funds: [] as FundEntity[],
        transactionTypes: [] as TransactionType[]
    };
    
    async componentDidMount() {
        await this.getFunds();
        this.getTypes();
    };

    getFunds = async () => {
        const funds = await getFunds();
        this.setState({funds})
    };

    addFund = async (fund: FundEntity) => {
        const createdFundId = await createFund(fund);
        if (!createdFundId) {
            return;
        }
        const newFund = {...fund, id: createdFundId} as FundEntity;
        this.setState((state: State) => {
            const funds = state.funds.concat(newFund);
            return {funds};
        });
    };

    updateFund = async (updatedFund: FundEntity) => {
        const isFundUpdated = await updateFund(updatedFund);
        if (!isFundUpdated) {
            return;
        }

        let fundNameChanged = false;

        this.setState((state: State) => {
            const funds = state.funds.map((fund: FundEntity) => {
                if (fund.id === updatedFund.id) {
                    fundNameChanged = fund.name !== updatedFund.name;
                    return {...updatedFund}
                } 
                return fund
            });

            if (!fundNameChanged) {
                return {funds};
            }
            
            const newTransactions = state.transactions.map(transaction => {
                if (transaction.fundSource.id === updatedFund.id) {
                    transaction.fundSource = {...updatedFund}
                }

                return transaction;
            });
            return {funds, transactions: newTransactions}
        });
    };

    deleteFund = async (deletedFund: FundEntity) => {
        const isFundDeleted = await deleteFund(deletedFund);

        if (!isFundDeleted) {
            return;
        }

        this.setState((state: State) => {
            const funds = state.funds.filter((fund: FundEntity) => fund.id !== deletedFund.id)
            return { funds }
        });
    };

    getTransactions = async (month: number, year: number) => {
        const transactions = await getTransactions(month, year);
        this.setState({month, year, transactions})
    };

    createTransaction = async (transaction: TransactionEntity) => {
        const createdTransaction = await createTransaction(transaction);
        if (!createdTransaction) {
            return;
        }

        if (this.isCurrentMonthTransaction(createdTransaction)) {
            const {transactions} = this.state;
            const newTransactions = insertByPredicate(transactions, createdTransaction, 
                (transactionElm: TransactionEntity) => (transactionElm.date <= createdTransaction.date));
            this.setState({transactions: newTransactions});
        }
        this.recalculateFund(createdTransaction, 1);
    };

    deleteTransaction = async (deletedTransaction: TransactionEntity) => { 
        const isDeleted = await deleteTransaction(deletedTransaction);
        if (isDeleted) {
            this.removeTransaction(deletedTransaction);
            this.recalculateFund(deletedTransaction, -1);
        }
    };

    updateTransaction = async (updatedTransaction: TransactionEntity) => {
        const affectedFunds = await updateTransaction(updatedTransaction);

        if (!affectedFunds.length) {
            return;
        }

        const newFunds = this.recalculateFunds(affectedFunds);
        const isCurrentMonthTransaction = this.isCurrentMonthTransaction(updatedTransaction);
        if (!isCurrentMonthTransaction) {
            this.removeTransaction(updatedTransaction);
            this.setState({funds: newFunds});
            return;
        }

        const {transactions} = this.state;
        const newTransactions = transactions.map((transaction: TransactionEntity) => {
            return transaction.id === updatedTransaction.id ?
                {...updatedTransaction}:
                transaction;
        });

        // TODO: date is not changed => reorder is not required 
        const updatedTransactions = reorderByPredicate(newTransactions, updatedTransaction, 
            (currentElement: TransactionEntity) => (currentElement.date <= updatedTransaction.date),
            (currentElement: TransactionEntity) => (currentElement.id !== updatedTransaction.id));

        this.setState({transactions: updatedTransactions, funds: newFunds});
    };

    removeTransaction = (deletingTransaction: TransactionEntity) => {
        this.setState((state: State) => {
            const transactions = state.transactions
                .filter((transaction: TransactionEntity) => transaction.id !== deletingTransaction.id)
            return { transactions }
        });
    }

    isCurrentMonthTransaction = (transaction: TransactionEntity): boolean => {
        const {month, year} = this.state;

        return transaction.date.getMonth() === month - 1 &&
               transaction.date.getFullYear() === year
    }

    recalculateFund = (changedTransaction: TransactionEntity, sign: number) => {
        const {funds} = this.state;
        if (changedTransaction.fundSource && changedTransaction.fundSource.id && funds && funds.length !== 0){
            const newFunds = funds.map((fund: FundEntity) => {
                if (fund.id === changedTransaction.fundSource.id){
                    fund.balance += changedTransaction.moneyQuantity * sign;
                } 
                return fund;
            });
            this.setState({funds: newFunds});
        }
    };

    recalculateFunds = (fundsToUpdate: FundToUpdate[]) => {
        const {funds} = this.state;
        const newFunds = funds.map((fund: FundEntity) => {
            const findResult = fundsToUpdate.find((fundToUpdate) => fundToUpdate.fundId === fund.id);
            if (findResult){
                fund.balance += findResult.delta;
            } 
            return fund;
        });
        return newFunds;
    };

    getTypes = () => {
        const url = `${config.api.URL}/TransactionType`;
        fetch(url, {method: "GET"})
            .then(checkPromiseStatus)
            .then((response: Response) => response.json())
            .then((transactionTypes: TransactionType[]) => this.setState({transactionTypes}))
            .catch(logPromiseError);
    };

    onTypeAdded = (newType: TransactionType) => {
        this.setState((state: State) => {
            const transactionTypes = state.transactionTypes.concat(newType);
            return {transactionTypes};
        });
    }
    
    render(){
        const {transactions, funds, transactionTypes, 
            year, month} = this.state;
        
        return (
            <div>
                <FundsBar onAddFundCallback = {this.addFund}
                    onDeleteFundCallback = {this.deleteFund} 
                    onUpdateFundCallback = {this.updateFund} 
                    funds = {funds}>
                </FundsBar>
                <SimpleGrid columns={2} spacing={16}>
                    <Box>
                        <Flex justifyContent={"space-between"}>
                            <Text fontSize="2xl" fontWeight={600}>Transactions</Text>
                            <AddTransactionButton 
                                fundSources={funds} 
                                onTypeAdded={this.onTypeAdded} 
                                transactionTypes={transactionTypes}
                                onTransactionCreated={this.createTransaction}/>
                        </Flex>
                        <Pagination year={year} month={month} onPageSwitched={this.getTransactions}></Pagination>
                        <div className="transactions">
                            {
                            (transactions.length !== 0) ?
                            transactions.map((transaction: TransactionEntity) => {       
                                return <Transaction key={transaction.id} transaction={transaction} 
                                    onDelete={this.deleteTransaction} onUpdate={this.updateTransaction}
                                    transactionTypes={transactionTypes}
                                    fundSources={funds}>
                                </Transaction>
                            }):
                            <div className="empty-transactions">There is no transactions yet</div>
                            }
                        </div>
                    </Box>
                    <Box>
                        {
                            transactions.length > 0 ?
                                <TransactionStats transactionTypes={transactionTypes} funds={funds} transactions={transactions}/>:
                                <Fragment/>
                        }
                    </Box>
                </SimpleGrid>
            </div>
        );
    }
}

export default Manager;