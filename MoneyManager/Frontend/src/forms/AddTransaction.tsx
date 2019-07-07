import React, { FormEvent } from 'react'
import './AddTransaction.css'

const mockTypes = (id: number) =>{
    return <option key={id}>{id}</option>;
}

class AddTransaction extends React.Component<any, any> {

    constructor(props: any){
        super(props);
        const {callback} = this.props;
        this.state = this.getDefaultState();
        this.state = {...this.state, callback}
    }

    getDefaultState(){
        return {
            name: "",
            date: new Date().toISOString().substr(0, 10),
            moneyQuantity: "",
            description: "",
            type: 0,
        }
    }

    add = () =>{
        const {callback, ...output} = this.state;
        if (callback){
            callback(output);
            this.setState(this.getDefaultState());
        }
    };
    
    reset = () =>{
        this.setState(this.getDefaultState());
    };
    
    submit = (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();
    };

    onNameChanged = ({ target: { value } }: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
          name: value,
        })
    }

    onMoneyQuantityChanged = ({ target: { value } }: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
          moneyQuantity: value,
        })
    }

    onDateChanged = ({ target: { value } }: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
          date: value,
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
                        <label className="title">Название</label>
                        <input className="value" onChange={this.onNameChanged} value={name} type="text"></input>
                    </div>
                    <div className="parameter">
                    <label className="title">Кол-во денег</label>
                        <input className="value" onChange={this.onMoneyQuantityChanged} value={moneyQuantity} type="number"></input>
                    </div>
                    <div className="parameter">
                        <label className="title">Дата</label>
                        <input className="value" onChange={this.onDateChanged} value={date} type="date"></input>
                    </div>          
                    <div className="parameter">
                        <label className="title">Тип</label>
                        <select className="value type" onChange={this.onTypeChanged} value={type}>
                            {
                                Array(10).fill(1).map((_, i) => {        
                                    return (mockTypes(i)) 
                                })
                            }
                        </select>
                    </div>         
                </div>
                <div className="buttons">
                    <button onClick={this.reset} className="cancel-btn">Сбросить</button>
                    <button onClick={this.add} className="add-btn">Добавить</button>
                </div>
            </form>
        </div>
    }   
}

export default AddTransaction;