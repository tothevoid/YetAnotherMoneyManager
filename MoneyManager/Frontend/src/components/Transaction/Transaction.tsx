import React from 'react';
import './Transaction.css';
import logo from '../../logo.svg'

type TransactionProps = {
    name: string,
    date: string,
    moneyQuantity: number;
    description: string,
    type: number
}

class Transaction extends React.Component<any, any> {
    constructor(props: any){
        super(props);
        this.state = {...this.props};
    }

    deleteTransaction = () => {
        if (this.state.deleteCallback){
            this.state.deleteCallback();
        }
    };

    updateTransaction = () => {
        //update this.state
    };

    render = () => {
        return <div className={this.props.moneyQuantity > 0 ? "green" + " transaction" : "red" + " transaction"}>
            <img src={logo} alt={this.props.name} className="icon"></img>
            <p className="date">{this.props.date}</p>
            <p className="name">{this.props.name}</p>
            <p className="money-quantity">{this.state.moneyQuantity}</p>
            <div className="buttons">
                <span className="edit"></span>
                <span className="delete"></span>
            </div>
         </div>
    }
}

export default Transaction;