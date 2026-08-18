import { Button, Card, Flex, Icon, Stack, Text, Link } from '@chakra-ui/react';
import { MdDelete, MdEdit, MdCompareArrows } from "react-icons/md";
import { Fragment, useCallback } from 'react';
import { AccountEntity } from '../../../../models/accounts/AccountEntity';
import { formatMoneyByCurrencyCulture } from '../../../../shared/utilities/formatters/moneyFormatter';
import { getBankIconUrl } from '../../../../api/banks/bankApi';
import { ACCOUNT_TYPE } from '../../../../shared/constants/accountType';
import { BsCurrencyExchange, BsBank } from "react-icons/bs";
import StoredIcon from '../../../../shared/components/StoredIcon';

interface Props {
    account: AccountEntity,
    onTransferClicked: (account: AccountEntity) => void,
    onEditClicked: (account: AccountEntity) => void,
    onDeleteClicked: (account: AccountEntity) => void,
}

const Account = (props: Props) => {
    const {name, balance, currency, bank, accountType} = props.account;

    const renderIcon = useCallback(() => {
        if (accountType.id === ACCOUNT_TYPE.CASH) {
            return (
                <StoredIcon
                    fallbackIcon={<BsCurrencyExchange size={14} color="#aaa" />}
                    size="xs"
                />
            );
        }

        const iconUrl = bank?.iconKey ? getBankIconUrl(bank.iconKey) : undefined;
        return (
            <StoredIcon
                src={iconUrl}
                fallbackIcon={<BsBank size={14} color="#aaa" />}
                size="xs"
            />
        );
    }, [accountType, bank]);

    const getTitle = useCallback(() => {
        if (accountType.id === ACCOUNT_TYPE.CASH ) {
            return <Link color="text_primary" href={`../cash_account/${props.account.id}`} fontWeight={600}>{name}</Link>
        }
        return <Text fontWeight={600}>{name}</Text>;
    }, []);

    return <Fragment>
        <Card.Root backgroundColor="background_primary" borderColor="border_primary" >
            <Card.Body color="text_primary" boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
                <Flex justifyContent="space-between" alignItems="center">
                    <Stack>
                        <Flex gapX={2} alignItems={"center"}>
                            {renderIcon()}
                            {getTitle()}
                        </Flex>
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