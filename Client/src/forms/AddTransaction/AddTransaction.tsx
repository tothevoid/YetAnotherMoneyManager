import React, { FormEvent } from 'react'
import './AddTransaction.scss'
import { TransactionEntity } from '../../models/TransactionEntity';
import { FundEntity } from '../../models/FundEntity';
import { getCurrentDate } from '../../utils/DateUtils';

type Props = {
    fundSources: FundEntity[]
    callback: (arg: State) => void
}

type State = Omit<TransactionEntity, "id">;

const getInitialState = (props: Props): State => {
    const source = (props.fundSources && props.fundSources.length !== 0) ? props.fundSources[0] :
        {id: ""} as FundEntity
    return {
        name: "",
        date: getCurrentDate(),
        moneyQuantity: 500,
        fundSource: source
    };
}

class AddTransaction extends React.Component<Props, State> {

    state = getInitialState(this.props);

    componentDidUpdate() {
        const {fundSources} = this.props;
        const {fundSource} = this.state;
        if (fundSources && fundSources.length !== 0 && (!fundSource || !fundSource.id)) {
            this.setState({fundSource:  fundSources[0]});
        }
      }

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

    handleChange = ({ target: { name, value, type } }: React.ChangeEvent<HTMLInputElement>) => {
        const normalizedValue = (type === "number" || type === "price") ? parseFloat(value) : value;
        this.setState({ [name]: normalizedValue} as any)
    }

    onSourceChanged = ({ target: { name, value } }: React.ChangeEvent<HTMLSelectElement>) => {
        const source = this.props.fundSources
            .find((entity: FundEntity) => entity.id === value);
        if (source){
            this.setState({[name]: source} as any)   
        }
    }

    render = () => {
        const { fundSources = [] } = this.props; 
        const { name, moneyQuantity, date, fundSource = {id: ""} } = this.state;
        return <form onSubmit={this.submit}>
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
                    <select name="fundSource" className="value type" onChange={this.onSourceChanged} value={fundSource.id}>
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
    }   
}

export default AddTransaction;