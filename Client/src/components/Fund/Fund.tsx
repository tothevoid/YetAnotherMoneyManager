import './Fund.scss'
import { FundEntity } from '../../models/FundEntity';
import { Card, CardBody, CardHeader, Heading, Text } from '@chakra-ui/react';
import { currency } from '../../constants/currency';

const Fund = (props: FundEntity, onClickCallback: () => void) => {
    const {name, balance} = props;
    // return <div className="fund"> 
    //     <p className="fund-name">{name}</p>
    //     <p className="fund-balance">{balance}<i className="currency">&#8381;</i></p>
    // </div>
    // key={variant} variant={variant}
    return <Card onClick={() => onClickCallback()}>
        <CardHeader>
            <Heading size='md'> {name}</Heading>
        </CardHeader>
        <CardBody>
            <Text>{balance}{currency.rub}</Text>
        </CardBody>
    </Card>
};

export default Fund;