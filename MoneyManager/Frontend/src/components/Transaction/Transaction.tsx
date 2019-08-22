import React from 'react';
import './Transaction.css';
import logo from '../../logo.svg'
import { TransactionEntity } from '../../models/TransactionEntity';

type Props = { onDelete: (id: string) => void } & TransactionEntity
type State = {isEditMode: boolean, lastTransaction: TransactionEntity, currentTransaction: TransactionEntity}
class Transaction extends React.Component<Props, State>{

    state = {
        isEditMode: false,
        lastTransaction: {...this.props},
        currentTransaction: {...this.props}
    }

    onDeleteClick = () => {
        const {id} = this.props;
        if (this.props.onDelete){
            this.props.onDelete(id);
        }
    }

    handleChange = ({ target: { name, value } }: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({currentTransaction: { ...this.state.currentTransaction, [name]: value}} as any)
    }

    onTypeChanged = ({ target: { value } }: React.ChangeEvent<HTMLSelectElement>) => {
        this.setState({type: value} as any)
    }

    onEditClick = () => 
        this.setState({isEditMode: true});

    onConfirmClick = () =>{
        console.log(this.state);
        const {currentTransaction} = this.state;
        const API_URL = "https://localhost:44319/Transaction";
        fetch(API_URL, { method: 'PATCH', body: JSON.stringify(currentTransaction),  headers: {'Content-Type': 'application/json'}})
            .then(res => {
                if (res.ok){
                    this.setState({isEditMode: false, lastTransaction: {...this.state.currentTransaction}});
                }
            })
    }

    onDiscardClick = () =>
        this.setState({isEditMode: false, currentTransaction: {...this.state.lastTransaction}})

    getButtons(){
        const {isEditMode} = this.state;
        if (isEditMode){
            return <div className="manipulation-buttons">
                <button onClick={this.onConfirmClick} className="manipulation-btn">Confirm</button>
                <button onClick={this.onDiscardClick} className="manipulation-btn">Discard</button>
            </div>
        } else {
            return <div className="manipulation-buttons">
                <button onClick={this.onEditClick} className="manipulation-btn">Edit</button>
                <button onClick={this.onDeleteClick} className="manipulation-btn">X</button>
            </div>
        }     
    }

    render(){
        const {moneyQuantity, name, date, fundSource} = this.state.currentTransaction;
        const {isEditMode} = this.state;
        return <div className={moneyQuantity > 0 ? "green transaction" : "red transaction"}>
            <img src={logo} alt={name} className="icon"></img>
            <input type="date" name="date" className="date field" onChange={this.handleChange} disabled={!isEditMode} value={date}></input>
            <input type="text" name="name" className="name field" onChange={this.handleChange} disabled={!isEditMode} value={name}></input>
            <input type="number" name="moneyQuantity" className="money-quantity field" onChange={this.handleChange} disabled={!isEditMode} value={moneyQuantity}></input>
            <input type="text" name="moneyQuantity" className="money-quantity field" disabled={!isEditMode} value={fundSource && fundSource.name}></input>
            {this.getButtons()}
        </div>
    }
}

export default Transaction;