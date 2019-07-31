import React  from 'react';
import Transaction from '../../components/Transaction/Transaction';
import AddTransaction from '../../forms/AddTransaction/AddTransaction';
import FundsBar from '../../components/FundsBar/FundsBar';
import ConfirmModal from '../../modals/ConfirmModal/ConfirmModal';
import { TransactionEntity } from '../../models/TransactionEntity';

type State = {
    transactions: TransactionEntity[],
    deleteModalVisible: boolean,
    onModalCallback: (isConfirmed: boolean) => void;
}

class Transactions extends React.Component<any, State> {

    state = {transactions: [], deleteModalVisible: false, onModalCallback: () => null}

    componentDidMount() {
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
            .then(res => this.setState({transactions: res}));
    }

    onTransactionCreated = (transaction: any) => {
        const API_URL = "https://localhost:44319/Transaction";
        fetch(API_URL, { method: 'PUT', body: JSON.stringify(transaction),  headers: {'Content-Type': 'application/json'}})
            .then((res) => {
                return res.json();
            })
            .then(id => {
                if (id){
                    transaction = {...transaction, id};
                    this.setState((state: any) => state.transactions.push(transaction))
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
        const {transactions, deleteModalVisible, onModalCallback} = this.state;
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
                <FundsBar></FundsBar>
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

export default Transactions;