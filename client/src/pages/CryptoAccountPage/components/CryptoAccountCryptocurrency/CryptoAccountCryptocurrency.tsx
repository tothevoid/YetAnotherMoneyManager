import { Card, Flex, Stack, Text, Image, Button, Icon } from '@chakra-ui/react';
import { Fragment } from 'react';
import { useTranslation } from 'react-i18next';
import { HiOutlineBuildingOffice2 } from 'react-icons/hi2';
import { CryptoAccountCryptocurrencyEntity } from '../../../../models/crypto/CryptoAccountCryptocurrencyEntity';
import { getIconUrl } from '../../../../api/crypto/cryptocurrencyApi';
import { formatMoneyByCurrencyCulture } from '../../../../shared/utilities/formatters/moneyFormatter';
import { MdDelete, MdEdit } from 'react-icons/md';

type Props = {
    cryptoAccountCryptocurrency: CryptoAccountCryptocurrencyEntity,
    onReloadCryptoAccountCryptocurrencies: () => void,
    onEditClicked: (cryptoAccountCryptocurrency: CryptoAccountCryptocurrencyEntity) => void,
    onDeleteClicked: (cryptoAccountCryptocurrency: CryptoAccountCryptocurrencyEntity) => void
}

const CryptoAccountCryptocurrency = (props: Props) => {
    const { quantity, cryptocurrency } = props.cryptoAccountCryptocurrency;

    const { t } = useTranslation();

    //TODO: Fix duplication with SecurityPage
    const icon = cryptocurrency.iconKey ?
        <Image h={8} w={8} rounded={16} src={getIconUrl(cryptocurrency.iconKey)}/>:
        <HiOutlineBuildingOffice2 size={32} color="#aaa" />

    return <Fragment>
        <Card.Root backgroundColor="background_primary" borderColor="border_primary" >
            <Card.Body color="text_primary" boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
                <Flex justifyContent="space-between" alignItems="center">
                    <Stack>
                        <Stack justifyContent={"start"} direction={"row"}>
                            {icon}
                            <Text color="text_primary" fontSize="xl" fontWeight={900}>{cryptocurrency.name}</Text>
                        </Stack>
                        <Text fontWeight={600}>{t("broker_account_security_card_security_quantity")}: {formatMoneyByCurrencyCulture(quantity, "USD")}</Text>
                    </Stack>
                    <Flex gap={2} justifyContent="space-between" alignItems="center">
                        <Button background={'background_secondary'} size={'sm'} onClick={() => props.onEditClicked(props.cryptoAccountCryptocurrency)}>
                            <Icon color="card_action_icon_primary">
                                <MdEdit/>
                            </Icon>
                        </Button>
                        <Button background={'background_secondary'} size={'sm'} onClick={() => props.onDeleteClicked(props.cryptoAccountCryptocurrency)}>
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

export default CryptoAccountCryptocurrency;