import React from 'react';
import './Transaction.scss';
import { TransactionEntity } from '../../models/TransactionEntity';
import { FundEntity } from '../../models/FundEntity';
import config from '../../config';
import { TransactionType } from '../../models/TransactionType';

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
        if (name === "moneyQuantity"){
            value = parseInt(value) as any;
        }
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

    //migrate to config
    getURL = (transactionType: TransactionType) => 
        `${config.api.URL}/images/${transactionType.id}.${transactionType.extension}`
    

    render(){
        const {fundSources} = this.props;
        const {moneyQuantity, transactionType, name, date, fundSource = {id: "", name: ""}} = this.state.currentTransaction;
        const {isEditMode} = this.state;
        return <div className={moneyQuantity > 0 ? "green transaction" : "red transaction"}>
            {
                transactionType ? 
                    <img src={this.getURL(transactionType)} alt={name} className="transaction-icon"></img>:
                    <div className="transaction-icon-placeholder"></div>
            }
            <input type="date" name="date" className="date field" onChange={this.handleChange} disabled={!isEditMode} value={date}></input>
            <input type="text" name="name" className="name field" onChange={this.handleChange} disabled={!isEditMode} value={name}></input>
            <input type="number" name="moneyQuantity" className="money-quantity field" onChange={this.handleChange} disabled={!isEditMode} value={moneyQuantity}></input>
            {
                !isEditMode ? 
                <input type="text" className="money-quantity field" disabled={true} value={(fundSource && fundSource.name) ? fundSource.name: "" }></input>:
                <select name="fundSource" className="field" onChange={this.onSourceChanged} disabled={!isEditMode} value={(fundSource && fundSource.id) ? fundSource.id: ""}>
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