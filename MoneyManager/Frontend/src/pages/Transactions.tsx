import React, { FormEvent } from 'react';
import Transaction from '../components/Transaction/Transaction';

const onAdd = () =>{
    console.log("Hey");
};

const submit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
};

const mockTransaction = (id: number) =>{
    return <Transaction key={id} icon="" name="test" date={Date.now().toString()} moneyQuantity={0}></Transaction>
}

const Transactions: React.FC = () => {
    return (
        <div>
            <div className="transactions">
                {
                Array(10).fill(1).map((_, i) => {        
                    return (mockTransaction(i)) 
                })
                }
            </div>
            <div className="manipulations">
                <form onSubmit={submit}>
                    <div className="parameters">
                        <input type="text"></input>
                        <input type="text"></input>
                        <input type="text"></input>
                        <input className="" type="text"></input>
                    </div>
                    <div className="buttons">
                        <button className="cancel-btn">Отмена</button>
                        <button onClick={onAdd} className="add-btn">Добавить</button>
                    </div>
                </form>
            </div>
        </div>
    );
}

export default Transactions;