import React  from 'react';
import Transaction from '../../components/Transaction/Transaction';
import AddTransaction from '../../forms/AddTransaction/AddTransaction';
import FundsBar from '../../components/FundsBar/FundsBar';
import ConfirmModal from '../../modals/ConfirmModal/ConfirmModal';
import { TransactionEntity } from '../../models/TransactionEntity';
import { FundEntity } from '../../models/FundEntity';
import { insertByPredicate, reorderByPredicate } from '../../utils/ArrayExtensions'
import config from '../../config' 

type FundToUpdate = {
    fundId: string,
    delta: number       
}

type State = {
    transactions: TransactionEntity[],
    funds: FundEntity[],
    deleteModalVisible: boolean,
    onModalCallback: (isConfirmed: boolean) => void;
}

class Manager extends React.Component<any, State> {

    state = {transactions: [], funds: [], deleteModalVisible: false, onModalCallback: () => null}
    
    componentDidMount() {
        this.getFunds();
        this.getTransactions();
    }

    getFunds(){
        const url = `${config.api.URL}/Fund`;
        fetch(url, {method: 'GET'})
            .then(res => res.json())
            .then(funds=> this.setState({funds}))
    }

    getTransactions(){
        const url = `${config.api.URL}/Transaction`;
        fetch(url, {method: 'GET'})
            .then(res => res.json())
            .then(res => {if (res){
                return res.map((element: TransactionEntity)=>{
                    const dateObj = new Date(element.date);
                    const date = dateObj.getFullYear().toString() + '-' +
                        (dateObj.getMonth() + 1).toString().padStart(2, "0") +
                        '-' + dateObj.getDate().toString().padStart(2, "0");
                    return {...element, date};
                });
            }})
            .then(transactions => this.setState({transactions}));
    }

    onFundAdded = (fund: FundEntity, onSuccess: (funds:FundEntity[]) => null): any => {
        const { id, ...fundToAdd } = fund;
            const url = `${config.api.URL}/Fund`;
            fetch(url, { method: 'PUT', body: JSON.stringify(fundToAdd), 
                headers: {'Content-Type': 'application/json'}})
                .then((res) => res.json())
                .then(createdId => {
                    if (createdId){
                        const newFund = {id:createdId, ...fundToAdd};
                        this.setState((state: any) => state.funds.push(newFund));
                        onSuccess(this.state.funds);
                    }
                }
        );
    }

    onFundUpdated = (fund: FundEntity, onSuccess: any): any =>{
        const url = `${config.api.URL}/Fund`;
        fetch(url, { method: 'PATCH', body: JSON.stringify(fund),  
            headers: {'Content-Type': 'application/json'}})
            .then(response => {
                if (response.ok){
                    this.setState((state: State) => {
                        const funds = state.funds.map((item: any) =>
                            (item.id === fund.id) ? {...fund}: item
                        );
                        return {funds};
                    })
                    onSuccess(this.state.funds);
                }
            }
        );
    }

    onFundDeleted = (fund: FundEntity, onSuccess: any): any => {
        const {id} = fund;
        const url = `${config.api.URL}/Fund?id=${id}`;
                fetch(url, { method: 'DELETE'})
                    .then(response => {
                        if (response.ok){
                            this.setState((state: any) => {
                                const funds = state.funds.filter((x: FundEntity) => x.id !== id)
                                return { funds }
                            });
                        }
                        onSuccess(this.state.funds);
                    }
                );
    }

    onTransactionCreated = (transaction: any) => {
        const url = `${config.api.URL}/Transaction`;
        fetch(url, { method: 'PUT', body: JSON.stringify(transaction),
            headers: {'Content-Type': 'application/json'}})
            .then((res) => res.json())
            .then(id => {
                if (id){
                    const newTransaction: TransactionEntity = {...transaction, id};
                    const {transactions} = this.state;
                    const newTransactions = insertByPredicate(transactions, newTransaction, 
                        (currentElm: TransactionEntity) => (currentElm.date <= newTransaction.date));
                    this.recalculateFund(transaction, 1);
                    this.setState({transactions: newTransactions});
                }
            }
        );
    };

    recalculateFund = (changedTrasnaction: TransactionEntity, sign: number) => {
        const {funds} = this.state;
        if (changedTrasnaction.fundSource && changedTrasnaction.fundSource.id && funds && funds.length !== 0){
            const newFunds = funds.map((fund: FundEntity) => {
                if (fund.id === changedTrasnaction.fundSource.id){
                    fund.balance += changedTrasnaction.moneyQuantity * sign;
                } 
                return fund;
            }) as FundEntity[]
            this.setState({funds: newFunds});
        }
    }

    recalculateFunds = (fundsToUpdate: FundToUpdate[]) => {
        const {funds} = this.state;
        const newFunds = funds.map((fund: FundEntity) => {
            const findResult = fundsToUpdate.find((fundToUpdate) => fundToUpdate.fundId === fund.id)
            if (findResult){
                fund.balance += findResult.delta;
            } 
            return fund;
        }) as FundEntity[]
        return newFunds;
    }

   
    onTransactionDeleted = (transaction: any) => { 
        const onModalCallback = (isConfirmed: boolean) => {
            if (isConfirmed){
                const url = `${config.api.URL}/Transaction`;
                fetch(url, { method: 'DELETE', body: JSON.stringify(transaction), 
                    headers: {'Content-Type': 'application/json'}})
                    .then(res => {
                        if (res.status === 200) {
                            this.setState((state: State) => {
                                const transactions = state.transactions.filter((x: TransactionEntity) => x.id !== transaction.id)
                                return { transactions }
                            });
                            this.recalculateFund(transaction, -1);
                        }
                        this.setState({deleteModalVisible: false});
                    }
                );
            } else {
                this.setState({deleteModalVisible: false});
            }
        }
        this.setState({deleteModalVisible: true, onModalCallback});
    }

    onTransactionUpdated = (updatedTransaction: TransactionEntity, 
        lastTransaction: TransactionEntity, onSuccess: () => void) => {
        const url = `${config.api.URL}/Transaction`;
        fetch(url, { method: 'PATCH', body: JSON.stringify({updatedTransaction, lastTransaction}),  
            headers: {'Content-Type': 'application/json'}})
            .then(res => res.json())
            .then((res: FundToUpdate[]) => {
                if (res){
                    onSuccess();
                    const {transactions} = this.state;
                    const newFunds = this.recalculateFunds(res);
                    const updatedTransactions = reorderByPredicate(transactions, updatedTransaction, 
                        (currentElement: TransactionEntity) => (currentElement.date <= updatedTransaction.date),
                        (currentElement: TransactionEntity) => (currentElement.id !== updatedTransaction.id));
                    this.setState({transactions: updatedTransactions, funds: newFunds});
                }
            })
    }

    render(){
        console.log(process.env.NODE_ENV);
        const {transactions, funds, deleteModalVisible, onModalCallback} = this.state;
        let deleteModal;
        const content = () => <p>{"Are you sure want to delete this record?"}</p>;
        if (deleteModalVisible){
            deleteModal = ConfirmModal(content)({onModalCallback});
        }
        return (
            <div>
                {deleteModal}
                <h1 className="page-title">Money management</h1>
                <h2 className="sub-title">My funds</h2>
                <FundsBar onAddFundCallback = {this.onFundAdded}
                    onDeleteFundCallback = {this.onFundDeleted} 
                    onUpdateFundCallback = {this.onFundUpdated} funds={funds}></FundsBar>
                <h2 className="sub-title">Transactions</h2>
                <AddTransaction fundSources={funds} callback={this.onTransactionCreated}/>
                <div className="transactions">
                    {
                    transactions.map((transaction: TransactionEntity) => {       
                        return <Transaction key={transaction.id} transaction = {transaction} 
                            onDelete={this.onTransactionDeleted} onUpdate={this.onTransactionUpdated}
                            fundSources={funds}>
                        </Transaction>
                    })
                    }
                </div>
            </div>
        );
    }
}

export default Manager;