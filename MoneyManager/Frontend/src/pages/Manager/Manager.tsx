import React  from 'react';
import Transaction from '../../components/Transaction/Transaction';
import AddTransaction from '../../forms/AddTransaction/AddTransaction';
import FundsBar from '../../components/FundsBar/FundsBar';
import ConfirmModal from '../../modals/ConfirmModal/ConfirmModal';
import { TransactionEntity } from '../../models/TransactionEntity';
import { FundEntity } from '../../models/FundEntity';

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
        const API_URL = "https://localhost:44319/Fund";
        fetch(API_URL, {method: 'GET'})
            .then(res => res.json())
            .then(funds=> this.setState({funds}))
    }

    getTransactions(){
        const API_URL = "https://localhost:44319/Transaction";
        fetch(API_URL, {method: 'GET'})
            .then(res => res.json())
            .then(res => {if (res){
                return res.map((element: TransactionEntity)=>{
                    const dateObj = new Date(element.date);
                    const date = dateObj.getFullYear().toString() + '-' + (dateObj.getMonth() + 1).toString().padStart(2, "0") +
                        '-' + dateObj.getDate().toString().padStart(2, "0");
                    return {...element, date};
                });
            }})
            .then(transactions => this.setState({transactions}));
    }

    onFundAdded = (fund: FundEntity, onSuccess: (funds:FundEntity[]) => null): any => {
        const { id, ...fundToAdd } = fund;
            const API_URL = "https://localhost:44319/Fund";
            fetch(API_URL, { method: 'PUT', body: JSON.stringify(fundToAdd),  headers: {'Content-Type': 'application/json'}})
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
        const API_URL = "https://localhost:44319/Fund";
        fetch(API_URL, { method: 'PATCH', body: JSON.stringify(fund),  headers: {'Content-Type': 'application/json'}})
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
        const API_URL = `https://localhost:44319/Fund?id=${id}`;
                fetch(API_URL, { method: 'DELETE'})
                    .then(response => {
                        if (response.ok){
                            this.setState((state: any) => {
                                const funds = state.funds.filter((x: FundEntity) => x.id !== id)
                                return { funds}
                            });
                        }
                        onSuccess(this.state.funds);
                    }
                );
    }

    onTransactionCreated = (transaction: any) => {
        const API_URL = "https://localhost:44319/Transaction";
        fetch(API_URL, { method: 'PUT', body: JSON.stringify(transaction),  headers: {'Content-Type': 'application/json'}})
            .then((res) => res.json())
            .then(id => {
                if (id){
                    const newTransaction = {...transaction, id};
                    this.setState((state: any) => state.transactions.push(newTransaction))
                }
               
            }
        );
    };

    onTransactionDeleted = (id: string) => { 
        const onModalCallback = (isConfirmed: boolean) => {
            if (isConfirmed){
                const API_URL = `https://localhost:44319/Transaction?id=${id}`;
                fetch(API_URL, { method: 'DELETE'})
                    .then(res => {
                        if (res.status === 200) {
                            this.setState((state: State) => {
                                const transactions = state.transactions.filter((x: TransactionEntity) => x.id !== id)
                                return { transactions }
                            });
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

    render(){
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
                <AddTransaction callback={this.onTransactionCreated}/>
                <div className="transactions">
                    {
                    transactions.map((transaction: any, i: number) => {       
                        return <Transaction {...transaction} onDelete={this.onTransactionDeleted} key={i}>
                        </Transaction>
                    })
                    }
                </div>
            </div>
        );
    }
}

export default Manager;