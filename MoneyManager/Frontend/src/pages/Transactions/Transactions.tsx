import React  from 'react';
import Transaction from '../../components/Transaction/Transaction';
import AddTransaction from '../../forms/AddTransaction/AddTransaction';
import FundsBar from '../../components/FundsBar/FundsBar';
import ConfirmModal from '../../modals/ConfirmModal/ConfirmModal';

class Transactions extends React.Component<any, any> {

    state = {transactions: [], deleteModalVisible: false, onModalCallback: null}

    componentDidMount() {
        const API_URL = "https://localhost:44319/Transaction";
        fetch(API_URL, {method: 'GET'})
            .then(res => res.json())
            .then(res => {if (res){
                return res.map((element: any)=>{
                    const date = new Date(element.date).toLocaleDateString(undefined, {day: "numeric", month: "numeric", year: "2-digit"});
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
                    const date = new Date(transaction.date).toLocaleDateString(undefined, {day: "numeric", month: "numeric", year: "2-digit"});
                    transaction = {...transaction, id, date};
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
                            this.setState((state: any) => {
                                const transactions = state.transactions.filter((x: any) => x.id !== id)
                                return { transactions }
                            });
                        }
                        this.setState({deleteModalVisible: false, onModalCallback: null});
                    }
                );
            } else {
                this.setState({deleteModalVisible: false, onModalCallback: null});
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
                <h2 className="sub-title">Spents</h2>
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