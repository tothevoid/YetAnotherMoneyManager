import React  from 'react';
import Transaction from '../../components/Transaction/Transaction';
import AddTransaction from '../../forms/AddTransaction';
import { any } from 'prop-types';

class Transactions extends React.Component<any, any> {

    constructor(props: any){
        super(props);
        this.state = this.getDefaultState();
        console.log(this.state);
    }

    loadTransactions = () => {
        console.log("loaded");
    }

    mockTransaction = (id: number) =>{
        const money = (Math.random() > 0.5) ? 100 : -100;
        return {
            name: "test",
            date: new Date().toISOString().substr(0, 10),
            moneyQuantity: money,
            description: "",
            type: 0
        }
    }

    onAddTransaction = () => {

    };

    showResult = (addedTransaction: object) =>{
        this.setState((state:any) => {
            const transactions = state.transactions.push(addedTransaction);
            return transactions;
        })
    };

    getDefaultState(){
        return {
            transactions: Array(10).fill(1).map((elm: any, id: number) => {
                return this.mockTransaction(id);
            })
        }
    }

    render(){
        const {transactions} = this.state;
        return (
            <div>
                <h1 className="pageTitle">Расходы</h1>
                <div className="transactions">
                    {
                    transactions.map((transaction: any, i: number) => {        
                        return <Transaction key={i}
                            description={transaction.description} 
                            name={transaction.name}
                            date={transaction.date}
                            type={transaction.type}
                            moneyQuantity={transaction.moneyQuantity}>
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