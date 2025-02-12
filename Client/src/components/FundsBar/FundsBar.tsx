import React, { Fragment } from 'react';
import Fund from '../Fund/Fund'
import './FundsBar.scss'
import ConfirmModal from '../../modals/ConfirmModal/ConfirmModal';
import AddFund from '../../forms/FundForm/FundForm';
import { FundEntity } from '../../models/FundEntity';
import { Button } from '@chakra-ui/react/button';
import { SimpleGrid } from '@chakra-ui/react/grid';
import { AddIcon } from '@chakra-ui/icons'
import { Flex, Text } from '@chakra-ui/react';

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
    onAddFundCallback: (fund: FundEntity, onSuccess: (newFunds: FundEntity[])=> void) => void,
    onDeleteFundCallback: (fund: FundEntity, onSuccess: (newFunds: FundEntity[])=> void) => void,
    onUpdateFundCallback: (fund: FundEntity, onSuccess: (newFunds: FundEntity[])=> void) => void
}

class FundsBar extends React.Component<Props,State>{
    state = {
        total: 0,
        currentFund: null,
        fundModalVisible: false,
        isFundModalNewMode: false,
    }
   
    //set total when the server returns funds 
    componentDidUpdate() {
        const {funds} = this.props;
        const {total} = this.state;
        const newTotal = calculateTotal(funds);
        if (newTotal !== total) {
            this.setState({total: newTotal});
        }
      }

    getCreateNewButton(){
        const onClickCallback = () => {this.setState({fundModalVisible: true, isFundModalNewMode: true})}
        return <Button onClick={() => onClickCallback()} leftIcon={<AddIcon />} colorScheme='purple' size='md'>
            Add fund
        </Button>
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
                title: "Add fund", confirmName:"Save", cancelName:"Close", ...additionalParams});
        }
        return (
            <Fragment>
                {/* <h3 className="funds-total">Total: {total}&#8381;</h3> */}
                <Flex justifyContent="space-between" alignItems="center" pt={5} pb={5}>
                    <Text fontSize='3xl'>Funds</Text>
                    {this.getCreateNewButton()}
                </Flex>
                <SimpleGrid pt={5} pb={5} spacing={8} templateColumns='repeat(auto-fill, minmax(300px, 3fr))'>
                    {
                    funds.map((fund: FundEntity, i: number) => {
                        const onClickCallback = () => {
                            this.setState({fundModalVisible: true, isFundModalNewMode: false, currentFund: fund})
                        }
                        return <Fund {...fund} key={fund.id} click  ></Fund>
                    })
                    }
                    {addNewModal}
                </SimpleGrid>
            </Fragment>
        );
    }
}

export default FundsBar;