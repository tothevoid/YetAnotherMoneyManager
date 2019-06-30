import React from 'react';
import './Transaction.css';

type TransactionProps = {
    icon: string,
    date: string,
    name: string,
    moneyQuantity: number;
}

const Transaction: React.FC<TransactionProps> = ({icon, date, name, moneyQuantity}) => {
    return (
        <div className="transaction">
            <img className="icon"></img>
            <p className="date">{date}</p>
            <p className="name">{name}</p>
            <p className="money-quantity">{moneyQuantity}</p>
            {/* <p className="expand">V</p> */}
        </div>
    );
}

export default Transaction;