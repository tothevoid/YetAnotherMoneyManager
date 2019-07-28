import React, { FormEvent } from 'react'
import './AddFund.css'

const getInitialState = (): TransactionState => {
    return {
        name: "",
        balance: 0,
    }
}

export type Props = {
    callback: (arg: TransactionState) => void
}

export type TransactionState = {
    name: string,
    balance: number,
}

class AddFund extends React.Component<any, any> {

    state = getInitialState();

    add = () =>{
        const {callback} = this.props;
        if (callback){
            callback(this.state);
            this.setState(getInitialState());
        }
    };
    
    submit = (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();
    };

    handleChange = ({ target: { name, value } }: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
          [name]: value,
        })
    }

    render = () => {
        const { name, balance } = this.state;
        return <div className="manipulations">
            <form onSubmit={this.submit}>
                <div className="fund-parameters">
                    <div className="fund-parameter">
                        <label className="title">Name</label>
                        <input autoFocus={true} name="name" className="value" onChange={this.handleChange} value={name} type="text"></input>
                    </div>
                    <div className="fund-parameter">
                        <label className="title">Balance</label>
                        <input name="balance" className="value" onChange={this.handleChange} value={balance} type="number"></input>
                    </div>
                </div>
            </form>
        </div>
    }   
}

export default AddFund;