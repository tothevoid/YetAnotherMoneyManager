import React  from 'react';
import Transaction from '../../components/Transaction/Transaction';
import AddTransaction from '../../forms/AddTransaction';

const getInitialState = () =>{
    return {
        transactions: Array(10).fill(1).map((elm: any, id: number) => {
            return mockTransaction(id);
        })
    }
}

const mockTransaction = (id: number) =>{
    const money = (Math.random() > 0.5) ? 100 : -100;
    const uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g,(c,r)=>('x'==c?(r=Math.random()*16|0):(r&0x3|0x8)).toString(16));
    return {
        name: "test",
        date: new Date().toISOString().substr(0, 10),
        moneyQuantity: money,
        description: "",
        type: 0,
        id: uuid
    }
}

class Transactions extends React.Component<any, any> {

    state = getInitialState();

    loadTransactions = () => {
        console.log("loaded");
    }

    onAddTransaction = () => {

    };

    showResult = (addedTransaction: object) =>{
        this.setState((state:any) => {
            const transactions = state.transactions.push(addedTransaction);
            return transactions;
        })
    };

    onTransactionDelete = (id: string) => {
        this.setState((state: any) => {
            const transactions = state.transactions.filter((x: any)=>x.id !== id)
            return {transactions}
        });
    }

    render(){
        const {transactions} = this.state;
        return (
            <div>
                <h1 className="pageTitle">Расходы</h1>
                <div className="transactions">
                    {
                    transactions.map((transaction: any, i: number) => {        
                        return <Transaction {...transaction} onDelete={this.onTransactionDelete} key={i}>
                        </Transaction>
                    })
                    }
                </div>
                <AddTransaction callback={this.showResult}/>
            </div>
        );
    }
}

export default Transactions;