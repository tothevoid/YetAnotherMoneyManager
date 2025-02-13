import React, { Fragment } from 'react';
import Fund from '../Fund/Fund'
import './FundsBar.scss'
import { FundEntity } from '../../models/FundEntity';
import { SimpleGrid } from '@chakra-ui/react/grid';
import { Flex, Text } from '@chakra-ui/react';
import AddFundButton from '../AddFundButton/AddFundButton';

const calculateTotal = (items: FundEntity[]) => {
    if (items && items.length > 0) return items.reduce((total: number, item: FundEntity)=> 
        total += item.balance, 0) 
    else return 0
}

type State = {
    total: number,
    currentFund: FundEntity | null,
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

    render(){
        const {total} = this.state;
        const {funds} = this.props;
        // const onNewModalCallback = (isConfirmed: boolean, fund: FundEntity) => {
        //     if (isConfirmed && fund){
        //         this.props.onAddFundCallback(fund, onSuccessFundChangeCallback)
        //     } else {
        //         this.setState({fundModalVisible: false});
        //     }
        // };

        // const onEditModalCallback = (isConfirmed: boolean, fund: FundEntity) => {
        //     if (isConfirmed && fund){
        //         this.props.onUpdateFundCallback(fund, onSuccessFundChangeCallback);
        //     } else {
        //         this.setState({fundModalVisible: false});
        //     }
        // };

        const onSuccessFundChangeCallback = (newFunds: FundEntity[]) => {
            this.setState({total: calculateTotal(newFunds)});
        }

        let addNewModal;
        const addFundCallback = (fund: FundEntity) => this.props.onAddFundCallback(fund, onSuccessFundChangeCallback)

        const deleteFundCallback = (fund: FundEntity) => this.props.onDeleteFundCallback(fund, onSuccessFundChangeCallback);

        return (
            <Fragment>
                {/* <h3 className="funds-total">Total: {total}&#8381;</h3> */}
                <Flex justifyContent="space-between" alignItems="center" pt={5} pb={5}>
                    <Text fontSize='3xl'>Funds</Text>
                    <AddFundButton onAdded={addFundCallback}></AddFundButton>
                </Flex>
                <SimpleGrid pt={5} pb={5} spacing={4} templateColumns='repeat(auto-fill, minmax(300px, 3fr))'>
                    {
                    funds.map((fund: FundEntity) => {
                        return <Fund fund={fund} onDeleteCallback={deleteFundCallback} key={fund.id}></Fund>
                    })
                    }
                    {addNewModal}
                </SimpleGrid>
            </Fragment>
        );
    }
}

export default FundsBar;