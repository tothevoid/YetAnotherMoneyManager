import React, { Fragment } from 'react';
import Fund from '../Fund/Fund'
import './FundsBar.css'
import FundContainer from '../FundContainer/FundContainer';
import AddFundButton from '../AddFundButton/AddFundButton';
import ConfirmModal from '../../modals/ConfirmModal/ConfirmModal';
import AddFund from '../../forms/AddFund/AddFund';
import { FundEntity } from '../../models/FundEntity';

type State = {
    funds: FundEntity[],
    total: number,
    addFundModalVisible: boolean
}

type Props ={}

class FundsBar extends React.Component<Props,State>{
    state = {
        funds: [],
        total: 0,
        addFundModalVisible: false
    }
    
    componentDidMount() {
        const API_URL = "https://localhost:44319/Fund";
        fetch(API_URL, {method: 'GET'})
            .then(res => res.json())
            .then(res => this.setState({funds: res, total: this.calculateTotal(res)}));
    }

    calculateTotal = (items: FundEntity[]) =>
        items.reduce((total: number, item: any)=> 
            total+=item.balance,0)

    getCreateNewButton(){
        const onClickCallback = () => {this.setState({addFundModalVisible: true})}
        const SubContainer = FundContainer(true, onClickCallback)(AddFundButton);
        return <SubContainer></SubContainer>
    }

    render(){
        const {funds, addFundModalVisible, total} = this.state;
        const onModalCallback = (isConfirmed: boolean, fund: FundEntity) => {
            if (isConfirmed && fund){
                const API_URL = "https://localhost:44319/Fund";
                fetch(API_URL, { method: 'PUT', body: JSON.stringify(fund),  headers: {'Content-Type': 'application/json'}})
                    .then((res) => res.json())
                    .then(id => {
                        if (id){
                            this.setState((state: any) => state.funds.push(fund));
                            this.setState({addFundModalVisible: false});
                        }
                    }
                );
            } else {
                this.setState({addFundModalVisible: false});
            }
        };
        let addNewModal;
        if (addFundModalVisible){
            addNewModal = ConfirmModal(AddFund)({onModalCallback});
        }
        return (
            <Fragment>
                <h3 className="total">Total: {total}&#8381;</h3>
                <div className="funds-bar">
                    {
                    funds.map((fund: FundEntity, i: number) => {
                        const SubContainer = FundContainer()(Fund)
                        return <SubContainer {...fund} key={i}></SubContainer>
                    })
                    }
                    {this.getCreateNewButton()}
                    {addNewModal}
                </div>
            </Fragment>
        );
    }
}

export default FundsBar;