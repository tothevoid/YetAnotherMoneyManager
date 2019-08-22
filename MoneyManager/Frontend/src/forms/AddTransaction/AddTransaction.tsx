import React, { FormEvent } from 'react'
import './AddTransaction.css'
import { TransactionEntity } from '../../models/TransactionEntity';
import { FundEntity } from '../../models/FundEntity';

type Props = {
    fundSources: FundEntity[]
    callback: (arg: State) => void
}

type State = Omit<TransactionEntity, "id">;

const getInitialState = (props: Props): State => {
    return {
        name: "",
        date: new Date().toISOString().substr(0, 10),
        moneyQuantity: 500,
        //TODO: index out of range fix
        fundSource: props.fundSources[0] || null
    };
}

class AddTransaction extends React.Component<Props, State> {

    state = getInitialState(this.props);

    add = () =>{
        const {callback} = this.props;
        if (callback){
            callback(this.state);
            this.setState(getInitialState(this.props));
        }
    };
    
    reset = () =>{
        this.setState(getInitialState(this.props));
    };
    
    submit = (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();
    };

    handleChange = ({ target: { name, value } }: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({ [name]: value} as any)
    }

    onSourceChanged = ({ target: { name, value } }: React.ChangeEvent<HTMLSelectElement>) => {
        const source = this.props.fundSources
            .find((entity: FundEntity) => entity.id === value);
        console.log(source);
        if (source){
            this.setState({[name]: source} as any)   
        }
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
                        <label className="title">Fund source</label>
                        <select name="fundSource" className="value type" onChange={this.onSourceChanged} value={fundSource && fundSource.id || undefined}>
                            {
                                fundSources.map((fund: FundEntity) => 
                                    <option key={fund.id} value={fund.id}>{fund.name}</option>)
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