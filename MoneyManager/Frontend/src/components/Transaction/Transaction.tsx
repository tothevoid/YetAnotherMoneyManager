import React from 'react';
import './Transaction.css';
import logo from '../../logo.svg'
import { TransactionEntity } from '../../models/TransactionEntity';

type Props = { onDelete: (id: string) => void } & TransactionEntity

const Transaction = (props: Props) => {
    const {moneyQuantity, name, date} = props;

    const onDeleteClick = () => {
        if (props.onDelete){
            props.onDelete(props.id);
        }
    };

    return <div className={moneyQuantity > 0 ? "green transaction" : "red transaction"}>
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