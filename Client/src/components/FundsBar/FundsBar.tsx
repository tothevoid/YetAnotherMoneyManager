import React, { Fragment } from 'react';
import Fund from '../Fund/Fund'
import './FundsBar.css'
import FundContainer from '../FundContainer/FundContainer';
import AddFundButton from '../AddFundButton/AddFundButton';
import ConfirmModal from '../../modals/ConfirmModal/ConfirmModal';
import AddFund from '../../forms/FundForm/FundForm';
import { FundEntity } from '../../models/FundEntity';

const calculateTotal = (items: FundEntity[]) => {
    if (items && items.length > 0) return items.reduce((total: number, item: FundEntity)=> 
        total += item.balance, 0) 
    else return 0
}

type State = {
    total: number,
    currentFund: FundEntity | null,
    fundModalVisible: boolean,
    isFundModalNewMode: boolean
}

type Props = {
    funds: FundEntity[],
    onAddFundCallback: (fund: FundEntity, onSuccess: (newFunds: FundEntity[])=> any) => null,
    onDeleteFundCallback: (fund: FundEntity, onSuccess: (newFunds: FundEntity[])=> any) => null,
    onUpdateFundCallback: (fund: FundEntity, onSuccess: (newFunds: FundEntity[])=> any) => null
}

class FundsBar extends React.Component<Props,State>{
    state = {
        total: 0,
        currentFund: null,
        fundModalVisible: false,
        isFundModalNewMode: false,
    }
   
    //set total when the server returns funds 
    componentDidUpdate(oldProps: Props) {
        const {funds} = this.props;
        const {total} = this.state;
        const newTotal = calculateTotal(funds);
        if (newTotal !== total) {
            this.setState({total: newTotal});
        }
      }

    getCreateNewButton(){
        const onClickCallback = () => {this.setState({fundModalVisible: true, isFundModalNewMode: true})}
        const SubContainer = FundContainer(true, onClickCallback)(AddFundButton);
        return <SubContainer></SubContainer>
    }

    render(){
        const {fundModalVisible, total} = this.state;
        const {funds} = this.props;
        const onNewModalCallback = (isConfirmed: boolean, fund: FundEntity) => {
            if (isConfirmed && fund){
                this.props.onAddFundCallback(fund, onSuccessFundChangeCallback)
            } else {
                this.setState({fundModalVisible: false});
            }
        };

        const onEditModalCallback = (isConfirmed: boolean, fund: FundEntity) => {
            if (isConfirmed && fund){
                this.props.onUpdateFundCallback(fund, onSuccessFundChangeCallback);
            } else {
                this.setState({fundModalVisible: false});
            }
        };

        const onDeleteCallback = (fund: FundEntity) => {
            if (fund){
                this.props.onDeleteFundCallback(fund, onSuccessFundChangeCallback);
            } else {
                this.setState({fundModalVisible: false});
            }
        };

        const onSuccessFundChangeCallback = (newFunds: FundEntity[]) => {
            this.setState({fundModalVisible: false, total: calculateTotal(newFunds)});
        }

        let addNewModal;
        if (fundModalVisible){
            const {isFundModalNewMode, currentFund} = this.state;
            const defaultFund = (isFundModalNewMode) ? {name: "", balance: 0}: currentFund;
            const onModalCallback = (isFundModalNewMode) ? onNewModalCallback: onEditModalCallback;
            const additionalParams = (isFundModalNewMode) ? {} : {additionalCallback: onDeleteCallback, additionalName: "Delete"} 
            addNewModal = ConfirmModal(AddFund)({onModalCallback, defaultFund,
                confirmName:"Save", cancelName:"Close", ...additionalParams});
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