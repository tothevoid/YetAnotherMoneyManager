import React from 'react';
import { TransactionEntity } from '../../models/TransactionEntity';
import { FundEntity } from '../../models/FundEntity';
import config from '../../config';
import { TransactionType } from '../../models/TransactionType';
import { ArrowUpIcon, DeleteIcon, EditIcon } from '@chakra-ui/icons';
import { Flex, Stack, Card, CardBody, Text, Button, Container } from '@chakra-ui/react';
import { currency } from '../../constants/currency';


type Props = { 
    onDelete: (id: TransactionEntity) => void,
    onUpdate: (updatedTransaction: TransactionEntity,
        lastTransaction: TransactionEntity,
        onSuccess: () => void) => void,
    transaction: TransactionEntity,
    fundSources: FundEntity[]
} 

class Transaction extends React.Component<Props, State>{

    onDeleteClick = () => {
        this.props.onDelete(this.props.transaction);
    }

    //migrate to config
    getURL = (transactionType: TransactionType) => 
        `${config.api.URL}/images/${transactionType.id}.${transactionType.extension}`
    

    render(){
        const {moneyQuantity, transactionType, name, date, fundSource} = this.props.transaction;
    
        return <Card mt={5} mb={5} boxShadow={"sm"} _hover={{ boxShadow: "md" }}>
            <CardBody>
                <Flex justifyContent="space-between" alignItems="center">
                    {/* {
                        transactionType ? 
                            <img src={this.getURL(transactionType)} alt={name} className="transaction-icon"></img>:
                            <div className="transaction-icon-placeholder"></div>
                    } */}
                    <Stack direction={'row'} alignItems="center">
                        <ArrowUpIcon rounded={16} fontSize="24"  background={'green.100'} color={'green.600'}/>
                        <Stack ml={5}>
                            <Text>{name}</Text>
                            <Text>{date} • {fundSource.name}</Text>
                        </Stack>
                    </Stack>
                    <Flex justifyContent="space-between" alignItems="center">
                        <Text>{transactionType?.name}</Text>
                        <Text>{moneyQuantity}{currency.rub}</Text>
                        <Button background={'white'} size={'sm'}><EditIcon/></Button>
                        <Button background={'white'} size={'sm'} onClick={() => this.onDeleteClick()}>
                        <DeleteIcon color={"red.600"}/>
                        </Button>
                    </Flex>
                </Flex>
            </CardBody>
        </Card>
    }
}

export default Transaction;