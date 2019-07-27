import React, { FormEvent } from 'react'
import './AddTransaction.css'

const mockTypes = (id: number) =>{
    return <option key={id}>{id}</option>;
}

const getInitialState = (): TransactionState => {
    return {
        name: "",
        date: new Date().toISOString().substr(0, 10),
        moneyQuantity: 500,
        type: 0
    }
}

export type Props = {
    callback: (arg: TransactionState) => void
}

export type TransactionState = {
    name: string,
    date: string,
    moneyQuantity: number,
    type: number,
}

class AddTransaction extends React.Component<any, any> {

    state = getInitialState();

    add = () =>{
        const {callback} = this.props;
        if (callback){
            callback(this.state);
            this.setState(getInitialState());
        }
    };
    
    reset = () =>{
        this.setState(getInitialState());
    };
    
    submit = (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();
    };

    handleChange = ({ target: { name, value } }: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
          [name]: value,
        })
    }

    onTypeChanged = ({ target: { value } }: React.ChangeEvent<HTMLSelectElement>) => {
        this.setState({
          type: value,
        })
    }

    render = () => {
        const { name, moneyQuantity, date, type } = this.state;
        return <div className="manipulations">
            <form onSubmit={this.submit}>
                <div className="parameters">
                    <div className="parameter">
                        <label className="title">Name</label>
                        <input name="name"  className="value" onChange={this.handleChange} value={name} type="text"></input>
                    </div>
                    <div className="parameter">
                        <label className="title">Money spent</label>
                        <input name="moneyQuantity" className="value" onChange={this.handleChange} value={moneyQuantity} type="number"></input>
                    </div>
                    <div className="parameter">
                        <label className="title">Date</label>
                        <input name="date" className="value" onChange={this.handleChange} value={date} type="date"></input>
                    </div>          
                    <div className="parameter">
                        <label className="title">Type</label>
                        <select name="type" className="value type" onChange={this.onTypeChanged} value={type}>
                            {
                                Array(10).fill(1).map((_, i) => {        
                                    return (mockTypes(i)) 
                                })
                            }
                        </select>
                    </div>         
                </div>
                <div className="buttons">
                    <button onClick={this.add} className="add-btn">Add</button>
                    <button onClick={this.reset} className="cancel-btn">Reset</button>
                </div>
            </form>
        </div>
    }   
}

export default AddTransaction;