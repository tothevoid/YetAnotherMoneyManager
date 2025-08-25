import { Button, Card, Flex, Icon, Stack, Text } from '@chakra-ui/react';
import { MdDelete, MdEdit, MdCompareArrows } from "react-icons/md";
import { Fragment } from 'react';
import { AccountEntity } from '../../../../models/accounts/AccountEntity';
import { formatMoneyByCurrencyCulture } from '../../../../shared/utilities/formatters/moneyFormatter';

interface Props {
    account: AccountEntity,
    onTransferClicked: (account: AccountEntity) => void,
    onEditClicked: (account: AccountEntity) => void,
    onDeleteClicked: (account: AccountEntity) => void,
}

const Account = (props: Props) => {
    const {name, balance, currency} = props.account;

    return <Fragment>
        <Card.Root backgroundColor="background_primary" borderColor="border_primary" >
            <Card.Body color="text_primary" boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
                <Flex justifyContent="space-between" alignItems="center">
                    <Stack>
                        <Text fontWeight={600}>{name}</Text>
                        <Text fontWeight={700}>{formatMoneyByCurrencyCulture(balance, currency.name)}</Text>
                    </Stack>
                    <Flex gap={1}>
                        <Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={() => props.onTransferClicked(props.account)}>
                            <Icon color="card_action_icon_primary">
                                <MdCompareArrows/>
                            </Icon>
                        </Button>
                        <Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={() => props.onEditClicked(props.account)}>
                            <Icon color="card_action_icon_primary">
                                <MdEdit/>
                            </Icon>
                        </Button>
                        <Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={() => props.onDeleteClicked(props.account)}>
                            <Icon color="card_action_icon_danger">
                                <MdDelete/>
                            </Icon>
                        </Button>
                    </Flex>
                </Flex>
            </Card.Body>
        </Card.Root>
    </Fragment>
};

export default Account;