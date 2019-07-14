import React from 'react';
import './Transaction.css';
import logo from '../../logo.svg'

export type TransactionProps = {
    name: string,
    date: string,
    moneyQuantity: number;
    description: string,
    type: number
}

class Transaction extends React.Component<any, any> {
   
    state = {...this.props};

    deleteTransaction = () => {
        if (this.state.deleteCallback){
            this.state.deleteCallback();
        }
    };

    updateTransaction = () => {
        //update this.state
    };

    render = () => {
        const {moneyQuantity, name, date} = this.state;
        return <div className={moneyQuantity > 0 ? "green" + " transaction" : "red" + " transaction"}>
            <img src={logo} alt={name} className="icon"></img>
            <p className="date">{date}</p>
            <p className="name">{name}</p>
            <p className="money-quantity">{moneyQuantity}</p>
            <div className="buttons">
                <span className="edit"></span>
                <span className="delete"></span>
            </div>
         </div>
    }
}

export default Transaction;