import React from 'react';
import './Transaction.css';
import logo from '../../logo.svg'
import { TransactionEntity } from '../../models/TransactionEntity';
import { FundEntity } from '../../models/FundEntity';

type Props = { 
    onDelete: (id: TransactionEntity) => void, 
    onUpdate: (updatedTransaction: TransactionEntity, 
        lastTransaction: TransactionEntity,
        onSuccess: () => void) => void,  
    transaction: TransactionEntity,
    fundSources: FundEntity[]
} 

type State = {isEditMode: boolean, lastTransaction: TransactionEntity, currentTransaction: TransactionEntity}
class Transaction extends React.Component<Props, State>{

    state = {
        isEditMode: false,
        lastTransaction: {...this.props.transaction},
        currentTransaction: {...this.props.transaction}
    }

    onDeleteClick = () => {
        const {lastTransaction} = this.state;
        if (this.props.onDelete){
            this.props.onDelete(lastTransaction);
        }
    }

    handleChange = ({ target: { name, value } }: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({currentTransaction: { ...this.state.currentTransaction, [name]: value}} as any)
    }

    onSourceChanged = ({ target: { name, value } }: React.ChangeEvent<HTMLSelectElement>) => {
        const source = this.props.fundSources
            .find((entity: FundEntity) => entity.id === value);
        if (source){
            this.setState({[name]: source} as any)   
            this.setState({currentTransaction: { ...this.state.currentTransaction, [name]: source}} as any)
        }
    }

    onEditClick = () => 
        this.setState({isEditMode: true});

    onConfirmClick = () =>{
        const {currentTransaction, lastTransaction} = this.state;
        const {onUpdate} = this.props;
        const onSuccess = () => {
            this.setState({isEditMode: false, lastTransaction: {...this.state.currentTransaction}});
        }
        onUpdate(currentTransaction, lastTransaction, onSuccess);
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
        const {fundSources} = this.props;
        const {moneyQuantity, name, date, fundSource = {id: "", name: ""}} = this.state.currentTransaction;
        const {isEditMode} = this.state;
        return <div className={moneyQuantity > 0 ? "green transaction" : "red transaction"}>
            {/* <img src={logo} alt={name} className="icon"></img> */}
            <input type="date" name="date" className="date field" onChange={this.handleChange} disabled={!isEditMode} value={date}></input>
            <input type="text" name="name" className="name field" onChange={this.handleChange} disabled={!isEditMode} value={name}></input>
            <input type="number" name="moneyQuantity" className="money-quantity field" onChange={this.handleChange} disabled={!isEditMode} value={moneyQuantity}></input>
            {
                !isEditMode ? 
                <input type="text" className="money-quantity field" disabled={true} value={fundSource.name}></input>:
                <select name="fundSource" className="field" onChange={this.onSourceChanged} disabled={!isEditMode} value={fundSource.id}>
                {
                    fundSources.map((fund: FundEntity) => 
                        <option key={fund.id} value={fund.id}>{fund.name}</option>)
                }
                </select>
            }
            {this.getButtons()}
        </div>
    }
}

export default Transaction;