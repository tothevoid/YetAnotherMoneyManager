import React, { FormEvent } from 'react'
import './AddTransaction.css'
import { TransactionEntity } from '../../models/TransactionEntity';
import { FundEntity } from '../../models/FundEntity';

type Props = {
    fundSources: FundEntity[]
    callback: (arg: State) => void
}

type State = Omit<TransactionEntity, "id">;

const getInitialState = (): any => {
    return {
        name: "",
        date: new Date().toISOString().substr(0, 10),
        moneyQuantity: 500,
        fundSourceId: null,
    }
}

class AddTransaction extends React.Component<Props, State> {

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
        this.setState({ [name]: value} as any)
    }

    onTypeChanged = ({ target: { name, value } }: React.ChangeEvent<HTMLSelectElement>) => {
        this.setState({[name]: value} as any)
    }

    render = () => {
        const { fundSources = [] } = this.props; 
        const { name, moneyQuantity, date, fundSource } = this.state;
        return <div className="manipulations">
            <form onSubmit={this.submit}>
                <div className="parameters">
                    <div className="parameter">
                        <label className="title">Name</label>
                        <input name="name"  className="value" onChange={this.handleChange} value={name} type="text"></input>
                    </div>
                    <div className="parameter">
                        <label className="title">Money change</label>
                        <input name="moneyQuantity" className="value" onChange={this.handleChange} value={moneyQuantity} type="number"></input>
                    </div>
                    <div className="parameter">
                        <label className="title">Date</label>
                        <input name="date" className="value" onChange={this.handleChange} value={date} type="date"></input>
                    </div>          
                    <div className="parameter">
                        <label className="title">Fund souce</label>
                        <select name="fundSource" className="value type" onChange={this.onTypeChanged} value={fundSource}>
                            {
                               fundSources.map((fund: FundEntity) => <option key={fund.id} value={fund.id}>{fund.name}</option>)
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