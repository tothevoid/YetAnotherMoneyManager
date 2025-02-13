import './Fund.scss'
import { FundEntity } from '../../models/FundEntity';
import { Button, Card, CardBody, Flex, Stack, Text } from '@chakra-ui/react';
import { currency } from '../../constants/currency';
import { DeleteIcon, EditIcon } from '@chakra-ui/icons';

const Fund = (props: FundEntity, onClickCallback: () => void) => {
    const {name, balance} = props;
    return <Card onClick={() => onClickCallback()}>
        <CardBody boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
            <Flex justifyContent="space-between" alignItems="center">
                <Stack>
                    <Text fontWeight={600}>{name}</Text>
                    <Text fontWeight={700}>{balance}{currency.rub}</Text>
                </Stack>
                <div>
                    <Button background={'white'} size={'sm'}><EditIcon/></Button>
                    <Button background={'white'} size={'sm'}><DeleteIcon color={"red.600"}/></Button>
                </div>
            </Flex>
        </CardBody>

    </Card>
};

export default Fund;