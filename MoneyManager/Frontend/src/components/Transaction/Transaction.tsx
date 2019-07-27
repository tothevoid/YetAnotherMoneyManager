import React from 'react';
import './Transaction.css';
import logo from '../../logo.svg'

export type TransactionProps = {
    name: string,
    date: string,
    moneyQuantity: number;
    type: number,
    id: string,
    onDelete: (id: string) => void
}

const Transaction = (props: TransactionProps) => {
    const {moneyQuantity, name, date} = props;

    const onDeleteClick = () => {
        if (props.onDelete){
            props.onDelete(props.id);
        }
    };

    return <div className={moneyQuantity > 0 ? "green" + " transaction" : "red" + " transaction"}>
        <img src={logo} alt={name} className="icon"></img>
        <p className="date">{date}</p>
        <p className="name">{name}</p>
        <p className="money-quantity">{moneyQuantity}</p>
        <div className="buttons">
            <span className="edit"></span>
            <span className="delete"></span>
        </div>
        <button onClick={onDeleteClick} className="delete-btn">X</button>
    </div>
}

export default Transaction;