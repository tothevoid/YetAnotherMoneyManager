import React from 'react';
import './Transaction.css';
import logo from '../../logo.svg'

type TransactionProps = {
    icon: string,
    date: string,
    name: string,
    moneyQuantity: number;
}

const Transaction: React.FC<TransactionProps> = ({icon, date, name, moneyQuantity}) => {
    return (
        <div className={moneyQuantity > 0 ? "green" + " transaction" : "red" + " transaction"}>
            <img src={logo} alt={name} className="icon"></img>
            <p className="date">{date}</p>
            <p className="name">{name}</p>
            <p className="money-quantity">{moneyQuantity}</p>
            {/* <p className="expand">V</p> */}
        </div>
    );
}

export default Transaction;