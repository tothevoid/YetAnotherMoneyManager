import { Card, Flex, Stack, Text, Image } from '@chakra-ui/react';
import { Fragment } from 'react';
import { useTranslation } from 'react-i18next';
import { HiOutlineBuildingOffice2 } from 'react-icons/hi2';
import { ClientCryptoAccountCryptocurrencyEntity } from '../../../../models/crypto/CryptoAccountCryptocurrencyEntity';
import { getIconUrl } from '../../../../api/crypto/cryptocurrencyApi';
import { formatMoneyByCurrencyCulture } from '../../../../shared/utilities/formatters/moneyFormatter';

type Props = {
    cryptoAccountCryptocurrency: ClientCryptoAccountCryptocurrencyEntity,
    onReloadCryptoAccountCryptocurrencies: () => void
}

const CryptoAccountCryptocurrency = (props: Props) => {
    const { quantity, cryptoAccount, cryptocurrency } = props.cryptoAccountCryptocurrency;

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
                </Flex>
            </Card.Body>
        </Card.Root>
    </Fragment>
};

export default CryptoAccountCryptocurrency;