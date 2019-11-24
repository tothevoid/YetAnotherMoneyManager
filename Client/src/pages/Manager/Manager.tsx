import React from 'react';
import "./Manager.scss"
import Transaction from '../../components/Transaction/Transaction';
import AddTransaction from '../../forms/AddTransaction/AddTransaction';
import FundsBar from '../../components/FundsBar/FundsBar';
import ConfirmModal from '../../modals/ConfirmModal/ConfirmModal';
import { TransactionEntity } from '../../models/TransactionEntity';
import { FundEntity } from '../../models/FundEntity';
import { insertByPredicate, reorderByPredicate } from '../../utils/ArrayExtensions'
import config from '../../config' 
import Pagination from '../../components/Pagination/Pagination';
import TransactionMoneyGraphs from '../../components/TransactionMoneyGraphs/TransactionMoneyGraphs';
import { logPromiseError, checkPromiseStatus } from '../../utils/PromiseUtils';
import { convertToInputDate } from '../../utils/DateUtils';
import Hideable, { hideableHOC } from '../../HOC/Hideable/Hideable';
import TransactionTypeForm from '../../forms/TransactionTypeForm/TransactionTypeForm';

type FundToUpdate = {
    fundId: string,
    delta: number       
}

type State = {
    transactions: TransactionEntity[],
    funds: FundEntity[],
    deleteModalVisible: boolean,
    setTypeModalVisible: boolean,
    month: number,
    year: number,
    onModalCallback: (isConfirmed: boolean) => void;
}

class Manager extends React.Component<any, State> {

    state = {
        month: new Date().getMonth() + 1,
        year: new Date().getFullYear(),
        transactions: [],
        funds: [], 
        deleteModalVisible: false,
        setTypeModalVisible: false,
        onModalCallback: () => null,
    };
    
    componentDidMount() {
        this.getFunds();
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
                    return {...transaction, date: convertToInputDate(date)};
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

    onTransactionCreated = (transaction: Omit<TransactionEntity, "id">) => {
        const url = `${config.api.URL}/Transaction`;
        fetch(url, { method: "PUT", body: JSON.stringify(transaction),
            headers: {"Content-Type": "application/json"}})
            .then(checkPromiseStatus)
            .then((response: Response) => response.json())
            .then(id => {
                const {month, year, transactions} = this.state;
                const newTransaction: TransactionEntity = {...transaction, id};
                const date = new Date(newTransaction.date);
                if (date.getMonth() === month - 1 && date.getFullYear() === year){
                    const newTransactions = insertByPredicate(transactions, newTransaction, 
                        (transactionElm: TransactionEntity) => (transactionElm.date <= newTransaction.date));
                    this.setState({transactions: newTransactions});
                }
                this.recalculateFund(newTransaction, 1);
            })
            .catch(logPromiseError);
    };

    recalculateFund = (changedTrasnaction: TransactionEntity, sign: number) => {
        const {funds} = this.state;
        if (changedTrasnaction.fundSource && changedTrasnaction.fundSource.id && funds && funds.length !== 0){
            const newFunds = funds.map((fund: FundEntity) => {
                if (fund.id === changedTrasnaction.fundSource.id){
                    fund.balance += changedTrasnaction.moneyQuantity * sign;
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
        const onModalCallback = (isConfirmed: boolean) => {
            this.setState({deleteModalVisible: false});
            if (isConfirmed){
                const url = `${config.api.URL}/Transaction`;
                fetch(url, { method: "DELETE", body: JSON.stringify(deletedTransaction), 
                    headers: {"Content-Type": "application/json"}})
                    .then(checkPromiseStatus)
                    .then(() => {
                        this.setState((state: State) => {
                            const transactions = state.transactions
                                .filter((transaction: TransactionEntity) => transaction.id !== deletedTransaction.id)
                            return { transactions }
                        });
                        this.recalculateFund(deletedTransaction, -1);
                    })
                    .catch(logPromiseError);
            }
        }
        this.setState({deleteModalVisible: true, onModalCallback});
    };

    onTransactionUpdated = (updatedTransaction: TransactionEntity, 
        lastTransaction: TransactionEntity, onSuccess: () => void) => {
        const url = `${config.api.URL}/Transaction`;
        fetch(url, { method: "PATCH", body: JSON.stringify({updatedTransaction, lastTransaction}),  
            headers: {"Content-Type": "application/json"}})
            .then(checkPromiseStatus)
            .then(response => response.json())
            .then((funds: FundToUpdate[]) => {
                if (funds && funds.length !== 0){
                    onSuccess();
                    const {transactions} = this.state;
                    const newFunds = this.recalculateFunds(funds);
                    const updatedTransactions = reorderByPredicate(transactions, updatedTransaction, 
                        (currentElement: TransactionEntity) => (currentElement.date <= updatedTransaction.date),
                        (currentElement: TransactionEntity) => (currentElement.id !== updatedTransaction.id));
                    this.setState({transactions: updatedTransactions, funds: newFunds});
                }
            })
            .catch(logPromiseError)
    };

    onStartedEditType = () => {
        const onModalCallback = (isConfirmed: boolean) => {
            this.setState({setTypeModalVisible: false});
            if (isConfirmed){

            }
        }
        this.setState({setTypeModalVisible: true, onModalCallback});
    }

    onTypeClick = () => 
        this.onStartedEditType();
    

    render(){
        const {transactions, funds, deleteModalVisible, onModalCallback, setTypeModalVisible, year, month} = this.state;
        let modal;
        if (deleteModalVisible){
            const content = () => <p>{"Are you sure want to delete this record?"}</p>;
            modal = ConfirmModal(content)({title: "Confirm transaction delete", onModalCallback});
        } else if (setTypeModalVisible){
            modal = ConfirmModal(TransactionTypeForm)({title: "Types manager", onModalCallback});
        }
      
        const Graphs = hideableHOC(TransactionMoneyGraphs);
        const hidableGraphsComponent = 
            <Graphs funds={funds} transactions={transactions} title={"Month money distribution"}></Graphs>;

        const NewTransaction = hideableHOC(AddTransaction);
        const hidableAddTransactionComponent = 
            <NewTransaction fundSources={funds} callback={this.onTransactionCreated} title={"New transaction"}></NewTransaction>;
        return (
            <div>
                <button onClick={() => this.onTypeClick()}></button>
                {modal}
                <h1 className="page-title">Money management</h1>
                <h2 className="sub-title">My funds</h2>
                <FundsBar onAddFundCallback = {this.onFundAdded}
                    onDeleteFundCallback = {this.onFundDeleted} 
                    onUpdateFundCallback = {this.onFundUpdated} 
                    funds = {funds}>
                </FundsBar>
                {hidableGraphsComponent}
                {hidableAddTransactionComponent}
                <h2 className="sub-title">Transactions</h2>
                <Pagination year={year} month={month} onPageSwitched={this.getTransactions}></Pagination>
                <div className="transactions">
                    {
                    (transactions.length !== 0) ?
                    transactions.map((transaction: TransactionEntity) => {       
                        return <Transaction key={transaction.id} transaction={transaction} 
                            onDelete={this.onTransactionDeleted} onUpdate={this.onTransactionUpdated}
                            fundSources={funds}>
                        </Transaction>
                    }):
                    <div className="empty-transactions">There is no transactions yet</div>
                    }
                </div>
            </div>
        );
    }
}

export default Manager;