import React, { Fragment } from 'react';
import Fund from '../Fund/Fund'
import './FundsBar.css'
import FundContainer from '../FundContainer/FundContainer';
import AddFundButton from '../AddFundButton/AddFundButton';
import ConfirmModal from '../../modals/ConfirmModal/ConfirmModal';
import AddFund from '../../forms/FundForm/FundForm';
import { FundEntity } from '../../models/FundEntity';

type State = {
    funds: FundEntity[],
    total: number,
    currentFund: FundEntity | null,
    fundModalVisible: boolean,
    isFundModalNewMode: boolean
}

type Props ={}

class FundsBar extends React.Component<Props,State>{
    state = {
        funds: [],
        total: 0,
        currentFund: null,
        fundModalVisible: false,
        isFundModalNewMode: false
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
        const onClickCallback = () => {this.setState({fundModalVisible: true, isFundModalNewMode: true})}
        const SubContainer = FundContainer(true, onClickCallback)(AddFundButton);
        return <SubContainer></SubContainer>
    }

    render(){
        const {funds, fundModalVisible, total} = this.state;
        const onNewModalCallback = (isConfirmed: boolean, fund: FundEntity) => {
            if (isConfirmed && fund){
                const { id, ...fundToAdd } = fund;
                const API_URL = "https://localhost:44319/Fund";
                fetch(API_URL, { method: 'PUT', body: JSON.stringify(fundToAdd),  headers: {'Content-Type': 'application/json'}})
                    .then((res) => res.json())
                    .then(id => {
                        if (id){
                            this.setState((state: any) => state.funds.push(fund));
                            this.setState({fundModalVisible: false});
                        }
                    }
                );
            } else {
                this.setState({fundModalVisible: false});
            }
        };

        const onEditModalCallback = (isConfirmed: boolean, fund: FundEntity) => {
            if (isConfirmed && fund){
                const API_URL = "https://localhost:44319/Fund";
                fetch(API_URL, { method: 'PATCH', body: JSON.stringify(fund),  headers: {'Content-Type': 'application/json'}})
                    .then(response => {
                        if (response.ok){
                            this.setState((state: State) => {
                                const funds = state.funds.map((item: any) =>
                                    (item.id === fund.id) ? {...fund}: item
                                );
                                return {funds};
                            })
                            this.setState({fundModalVisible: false});
                        }
                    }
                );
            } else {
                this.setState({fundModalVisible: false});
            }
        };

        let addNewModal;
        if (fundModalVisible){
            const {isFundModalNewMode, currentFund} = this.state;
            const defaultFund = (isFundModalNewMode) ? {name: "", balance: 0}: currentFund;
            const onModalCallback = (isFundModalNewMode) ? onNewModalCallback: onEditModalCallback;
            addNewModal = ConfirmModal(AddFund)({onModalCallback, isFundModalNewMode, defaultFund, confirmName:"Save", cancelName:"Close"});
        }
        return (
            <Fragment>
                <h3 className="total">Total: {total}&#8381;</h3>
                <div className="funds-bar">
                    {
                    funds.map((fund: FundEntity, i: number) => {
                        const onClickCallback = () => {
                            this.setState({fundModalVisible: true, isFundModalNewMode: false, currentFund: fund})
                        }
                        const SubContainer = FundContainer(false, onClickCallback)(Fund)
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