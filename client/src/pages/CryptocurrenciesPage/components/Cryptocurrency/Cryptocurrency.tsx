import { Button, Card, Flex, Icon, Stack, Text, Image } from '@chakra-ui/react';
import { MdDelete, MdEdit } from "react-icons/md";
import { Fragment, useEffect, useState } from 'react';
import { formatMoneyByCurrencyCulture } from '../../../../shared/utilities/formatters/moneyFormatter';
import { CryptocurrencyEntity } from '../../../../models/crypto/CryptocurrencyEntity';
import { FaBitcoin } from "react-icons/fa";
import { getIconUrl } from '../../../../api/crypto/cryptocurrencyApi';

interface Props {
    cryptocurrency: CryptocurrencyEntity,
    onEditClicked: (cryptocurrency: CryptocurrencyEntity) => void,
    onDeletedClicked: (cryptocurrency: CryptocurrencyEntity) => void
}

const Cryptocurrency = (props: Props) => {
    const { name, symbol, price, iconKey } = props.cryptocurrency;
    const [iconDate, setIconDate] = useState(new Date());

    useEffect(() => {
        setIconDate(new Date());
    }, [props.cryptocurrency])

    const icon = iconKey ?
        <Image h={8} w={8} rounded={16} src={getIconUrl(iconKey, iconDate)}/>:
        <FaBitcoin size={32} color="#aaa" />

    return <Fragment>
        <Card.Root backgroundColor="background_primary" borderColor="border_primary" >
            <Card.Body color="text_primary" boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
                <Flex justifyContent="space-between" alignItems="center">
                    <Stack>
                        <Stack justifyContent={"start"} direction="row">
                            {icon}
                            <Text fontSize="2xl" fontWeight={600} color="text_primary">{symbol}</Text>
                        </Stack>
                        <Text fontWeight={600}>{name}</Text>
                        <Text fontWeight={600}>{formatMoneyByCurrencyCulture(price, "USD")}</Text>
                    </Stack>
                    <Flex gap={1}>
                        <Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={() => props.onEditClicked(props.cryptocurrency)}>
                            <Icon color="card_action_icon_primary">
                                <MdEdit/>
                            </Icon>
                        </Button>
                        <Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={() => props.onDeletedClicked(props.cryptocurrency)}>
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

export default Cryptocurrency;