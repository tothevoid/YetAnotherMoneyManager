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

type FundToUpdate = {
    fundId: string,
    delta: number
}

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
    
    componentDidMount() {
        this.getFunds();
        this.getTypes();
    };

    getFunds = () => {
        const url = `${config.api.URL}/Fund`;
        fetch(url, {method: "GET"})
            .then(checkPromiseStatus)
            .then((response: Response) => response.json())
            .then((funds: FundEntity[]) => this.setState({funds}))
            .catch(logPromiseError);
    };

    getTransactions = (month: number, year: number) => {
        this.setState({month, year});
        const url = `${config.api.URL}/Transaction?month=${month}&year=${year}`;
        fetch(url, {method: "GET"})
            .then(checkPromiseStatus)
            .then((response: Response) => response.json())
            .then((transactions: TransactionEntity[]) => 
                transactions.map((transaction: TransactionEntity) => {
                    const date = new Date(transaction.date);
                    // convertToInputDate
                    return {...transaction, date: date};
                })
            )
            .then(transactions => this.setState({transactions}))
            .catch(logPromiseError);
    };

    onFundAdded = (fund: FundEntity, onSuccess: (funds:FundEntity[]) => void) => {
        const { id, ...fundToAdd } = fund;
        const url = `${config.api.URL}/Fund`;
        fetch(url, { method: "PUT", body: JSON.stringify(fundToAdd), 
            headers: {"Content-Type": "application/json"}})
            .then(checkPromiseStatus)
            .then((response: Response) => response.json())
            .then(createdId => {
                if (createdId){
                    const newFund = {id:createdId, ...fundToAdd} as FundEntity;
                    this.setState((state: State) => {
                        const funds = state.funds.concat(newFund);
                        return {funds};
                    });
                    onSuccess(this.state.funds);
                }
            })
            .catch(logPromiseError)
    };

    onFundUpdated = (updatedFund: FundEntity, onSuccess: (funds: FundEntity[]) => void) => {
        const url = `${config.api.URL}/Fund`;
        fetch(url, { method: "PATCH", body: JSON.stringify(updatedFund),  
            headers: {"Content-Type": "application/json"}})
            .then(checkPromiseStatus)
            .then(() => {
                this.setState((state: State) => {
                    const funds = state.funds.map((fund: FundEntity) =>
                        (fund.id === updatedFund.id) ? {...updatedFund}: fund
                    );
                    return {funds};
                })
                onSuccess(this.state.funds);
            })
            .catch(logPromiseError)
    };

    onFundDeleted = (deletedFund: FundEntity, onSuccess: (funds: FundEntity[]) => void) => {
        const {id} = deletedFund;
        const url = `${config.api.URL}/Fund?id=${id}`;
        fetch(url, { method: "DELETE"})
            .then(checkPromiseStatus)
            .then(() => {
                this.setState((state: State) => {
                    const funds = state.funds.filter((fund: FundEntity) => fund.id !== id)
                    return { funds }
                });
                onSuccess(this.state.funds);
            })
            .catch(logPromiseError)
    };

    onTransactionCreated = (transaction: TransactionEntity) => {
        const url = `${config.api.URL}/Transaction`;
        fetch(url, { method: "PUT", body: JSON.stringify(transaction),
            headers: {"Content-Type": "application/json"}})
            .then(checkPromiseStatus)
            .then((response: Response) => response.json())
            .then(id => {
                const newTransaction: TransactionEntity = {...transaction, id};
                if (this.isCurrentMonthTransaction(newTransaction)){
                    const {transactions} = this.state;
                    const newTransactions = insertByPredicate(transactions, newTransaction, 
                        (transactionElm: TransactionEntity) => (transactionElm.date <= newTransaction.date));
                    this.setState({transactions: newTransactions});
                }
                this.recalculateFund(newTransaction, 1);
            })
            .catch(logPromiseError);
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
   
    onTransactionDeleted = (deletedTransaction: TransactionEntity) => { 
        const url = `${config.api.URL}/Transaction`;
        fetch(url, { method: "DELETE", body: JSON.stringify(deletedTransaction), 
            headers: {"Content-Type": "application/json"}})
            .then(checkPromiseStatus)
            .then(() => {
                this.removeTransaction(deletedTransaction);
                this.recalculateFund(deletedTransaction, -1);
            })
            .catch(logPromiseError);
    };

    onTransactionUpdated = (updatedTransaction: TransactionEntity) => {
        const url = `${config.api.URL}/Transaction`;
        fetch(url, { method: "PATCH", body: JSON.stringify(updatedTransaction),  
            headers: {"Content-Type": "application/json"}})
            .then(checkPromiseStatus)
            .then(response => response.json())
            .then((funds: FundToUpdate[]) => {
                const {transactions} = this.state;
                const newFunds = this.recalculateFunds(funds);

                const isCurrentMonthTransaction = this.isCurrentMonthTransaction(updatedTransaction);
                if (!isCurrentMonthTransaction) {
                    this.removeTransaction(updatedTransaction);
                    return;
                }

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
            })
            .catch(logPromiseError)
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
                <FundsBar onAddFundCallback = {this.onFundAdded}
                    onDeleteFundCallback = {this.onFundDeleted} 
                    onUpdateFundCallback = {this.onFundUpdated} 
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
                                onTransactionCreated={this.onTransactionCreated}/>
                        </Flex>
                        <Pagination year={year} month={month} onPageSwitched={this.getTransactions}></Pagination>
                        <div className="transactions">
                            {
                            (transactions.length !== 0) ?
                            transactions.map((transaction: TransactionEntity) => {       
                                return <Transaction key={transaction.id} transaction={transaction} 
                                    onDelete={this.onTransactionDeleted} onUpdate={this.onTransactionUpdated}
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