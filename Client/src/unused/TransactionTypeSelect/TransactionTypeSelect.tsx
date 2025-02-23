import './TransactionTypeSelect.scss'
import React, { Fragment } from 'react';
import TransactionTypeForm from '../TransactionTypeForm/TransactionTypeForm';
import { TransactionType } from '../../models/TransactionType';
import ConfirmModal from '../../modals/ConfirmModal/ConfirmModal';
import Button from '../../components/Basic/Button/Button';

type Props = {
    transactionTypes: TransactionType[],
    onTypeAdded: (type: TransactionType) => void;
    onTypeSelected: (type: TransactionType) => void;
}

type State = {
    isTypeModalVisible: boolean,
    selectedType: TransactionType,
}

class TransactionTypeSelect extends React.Component<Props, State> {
 
    state = {
        isTypeModalVisible: false,
        selectedType: {name: ""} as TransactionType
    }

    onStartedEditType = () => 
        this.setState({isTypeModalVisible: true});

    closeModal = () => 
        this.setState({isTypeModalVisible: false});
        
    render = () => {
        let modal;
        if (this.state.isTypeModalVisible){
            modal = ConfirmModal(TransactionTypeForm)({title: "Types manager", callback: (selectedType: TransactionType) => {
                this.props.onTypeSelected(selectedType);
                this.setState({selectedType});
                this.closeModal();
            }, 
            onModalCallback: this.closeModal,
            transactionTypes: this.props.transactionTypes, onTypeAdded: this.props.onTypeAdded});
        }

        return <Fragment>
            {modal}
            <span>{this.state.selectedType.name}</span>
            <Button text="Set" onClick={this.onStartedEditType} classes="add-type-button"/>
        </Fragment>
    }   
}

export default TransactionTypeSelect;