import { Card, Flex, Link, Span, Stack, Text, Image } from '@chakra-ui/react';
import { Fragment } from 'react';
import { useTranslation } from 'react-i18next';
import { HiOutlineBuildingOffice2 } from 'react-icons/hi2';
import { getIconUrl } from '../../../../api/securities/securityApi';
import { BrokerAccountSecurityEntity } from '../../../../models/brokers/BrokerAccountSecurityEntity';
import { formatMoneyByCurrencyCulture } from '../../../../shared/utilities/formatters/moneyFormatter';

type Props = {
    brokerAccountSecurity: BrokerAccountSecurityEntity,
    onDeleteCallback: (account: BrokerAccountSecurityEntity) => void,
    onEditCallback: (account: BrokerAccountSecurityEntity) => void,
    onReloadBrokerAccounts: () => void
}

const BrokerAccountSecurity = (props: Props) => {
    const {price, quantity, security, brokerAccount} = props.brokerAccountSecurity;

    const { t } = useTranslation();

    const actualPrice = security.actualPrice * quantity;
    const profitAndLoss = actualPrice - price;

    const color = profitAndLoss > 0 ?
        "green.600":
        "red.600";

    const percentage = profitAndLoss / actualPrice * 100;

    const securityLink = `../security/${security.id}`;

    //TODO: Fix duplication with SecurityPage
    const icon = security.iconKey ?
        <Image h={8} w={8} rounded={16} src={getIconUrl(security.iconKey)}/>:
        <HiOutlineBuildingOffice2 size={32} color="#aaa" />

    return <Fragment>
        <Card.Root backgroundColor="background_primary" borderColor="border_primary" >
            <Card.Body color="text_primary" boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
                <Flex justifyContent="space-between" alignItems="center">
                    <Stack>
                        <Stack justifyContent={"center"} direction={"row"}>
                            {icon}
                            <Link color="text_primary" href={securityLink} fontSize="xl" fontWeight={900}>{security?.name} ({security?.ticker})</Link>
                        </Stack>
                        <Text fontWeight={600}>{t("broker_account_security_card_security_quantity")}: {quantity}</Text>
                        <Text fontWeight={600}>{t("broker_account_security_card_security_initial_price")}: {formatMoneyByCurrencyCulture(price, security?.currency?.name)}</Text>
                        <Text fontWeight={600}>{t("broker_account_security_card_security_current_price")}: {formatMoneyByCurrencyCulture(actualPrice, security?.currency?.name)}</Text>
                        <Text fontWeight={600}>
                            {t("broker_account_security_card_security_p&l")}:
                            <Span paddingLeft={1.5} color={color}>{formatMoneyByCurrencyCulture(profitAndLoss, security?.currency?.name)} ({percentage.toFixed(2)}%)</Span>
                        </Text>
                    </Stack>
                </Flex>
            </Card.Body>
        </Card.Root>
    </Fragment>
};

export default BrokerAccountSecurity;