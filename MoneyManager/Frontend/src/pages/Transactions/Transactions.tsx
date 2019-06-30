import React, { FormEvent } from 'react';
import Transaction from '../../components/Transaction/Transaction';

const onAdd = () =>{
    console.log("added");
};

const onCancel = () =>{
    console.log("canceled");
};

const submit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
};

const mockTransaction = (id: number) =>{
    const money = (Math.random() > 0.5) ? 100 : -100;
    return <Transaction key={id} icon="" name="test" date={Date.now().toString()} moneyQuantity={money}></Transaction>
}

const mockTypes = (id: number) =>{
    return <option key={id}>{id}</option>;
}

const Transactions: React.FC = () => {
    return (
        <div>
            <h1 className="pageTitle">Расходы</h1>
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
                        <div className="parameter">
                            <label className="title">Название</label>
                            <input className="value" type="text"></input>
                        </div>
                        <div className="parameter">
                        <label className="title">Кол-во денег</label>
                            <input className="value" type="number"></input>
                        </div>
                        <div className="parameter">
                            <label className="title">Описание</label>
                            <input className="value" type="text"></input>
                        </div>          
                        <div className="parameter">
                            <label className="title">Тип</label>
                            <select className="value">
                                {
                                    Array(10).fill(1).map((_, i) => {        
                                        return (mockTypes(i)) 
                                    })
                                }
                            </select>
                        </div>         
                    </div>
                    <div className="buttons">
                        <button onClick={onCancel} className="cancel-btn">Отмена</button>
                        <button onClick={onAdd} className="add-btn">Добавить</button>
                    </div>
                </form>
            </div>
        </div>
    );
}

export default Transactions;