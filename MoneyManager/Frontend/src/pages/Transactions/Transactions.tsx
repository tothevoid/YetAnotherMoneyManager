import React  from 'react';
import Transaction from '../../components/Transaction/Transaction';
import AddTransaction from '../../forms/AddTransaction';
import FundsBar from '../../components/FundsBar/FundsBar';

class Transactions extends React.Component<any, any> {

    state = {transactions: []}

    componentDidMount() {
        const API_URL = "https://localhost:44319/Transaction";
        fetch(API_URL, {method: 'GET'})
            .then(res => res.json())
            .then(res => this.setState({transactions: res}));
    }

    loadTransactions = () => {
        console.log("loaded");
    }

    onTransactionCreated = (transaction: object) => {
        const API_URL = "https://localhost:44319/Transaction";
        console.log(transaction);
        fetch(API_URL, { method: 'PUT', body: JSON.stringify(transaction),  headers: {'Content-Type': 'application/json'}})
            .then(res => {
                console.log(res);
                if (res.status === 200) {
                    this.setState((state: any) => state.transactions.push(transaction))
                }
            }
        );
    };

    onTransactionDeleted = (id: string) => {
        const API_URL = `https://localhost:44319/Transaction?id=${id}`;
        fetch(API_URL, { method: 'DELETE'})
            .then(res => {
                console.log(res);
                if (res.status === 200) {
                    this.setState((state: any) => {
                        const transactions = state.transactions.filter((x: any) => x.id !== id)
                        return { transactions }
                    });
                }
            }
        );
    }

    render(){
        const {transactions} = this.state;
        return (
            <div>
                <h1 className="page-title">Затраты</h1>
                <h2 className="sub-title">Мои деньги</h2>
                <FundsBar></FundsBar>
                <h2 className="sub-title">Расходы</h2>
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