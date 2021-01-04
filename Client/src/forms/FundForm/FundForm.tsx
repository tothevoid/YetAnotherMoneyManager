import React, { FormEvent } from 'react'
import './FundForm.scss'
import { FundEntity } from '../../models/FundEntity';

type Props = {
    isNewMode: boolean,
    defaultFund: FundEntity,
    callback: (arg: FundEntity) => void
}

// type State = Omit<FundEntity, "id">;

const getInitialState = (props: Props): FundEntity => {
    return {...props.defaultFund}
}

class AddFund extends React.Component<Props, FundEntity> {

    state = getInitialState(this.props);

    add = () =>{
        const {callback} = this.props;
        if (callback){
            callback(this.state);
        }
    };
    
    submit = (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();
    };

    handleChange = ({ target: { name, value, type } }: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({[name]: type === 'number' ? parseFloat(value) : value} as any)
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
                        <input step="10" name="balance" className="value number" onChange={this.handleChange} value={balance} type="number"></input>
                    </div>
                </div>
            </form>
        </div>
    }   
}

export default AddFund;