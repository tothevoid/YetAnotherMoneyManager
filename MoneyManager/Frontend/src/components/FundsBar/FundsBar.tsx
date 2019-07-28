import React from 'react';
import Fund from '../Fund/Fund'
import './FundsBar.css'
import FundContainer from '../FundContainer/FundContainer';
import AddFundButton from '../AddFundButton/AddFundButton';
import ConfirmModal from '../../modals/ConfirmModal/ConfirmModal';
import AddFund from '../../forms/AddFund/AddFund';

class FundsBar extends React.Component{
    state = {
        funds: [],
        addFundModalVisible: false
    }
    
    componentDidMount() {
        const API_URL = "https://localhost:44319/Fund";
        fetch(API_URL, {method: 'GET'})
            .then(res => res.json())
            .then(res => this.setState({funds: res}));
    }

    getCreateNewButton(){
        const onClickCallback = () => {this.setState({addFundModalVisible: true})}
        const SubContainer = FundContainer(true, onClickCallback)(AddFundButton);
        return <SubContainer></SubContainer>
    }

    render(){
        const {funds, addFundModalVisible} = this.state;
        const onModalCallback = (isConfirmed: any, fund: any) => {
            if (isConfirmed && fund){
                console.log(fund);
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
            <div className="funds-bar">
                {
                funds.map((fund: any, i: number) => {
                    const SubContainer = FundContainer()(Fund)
                    return <SubContainer {...fund} key={i}></SubContainer>
                })
                }
                {this.getCreateNewButton()}
                {addNewModal}
            </div>
        );
    }
}

export default FundsBar;