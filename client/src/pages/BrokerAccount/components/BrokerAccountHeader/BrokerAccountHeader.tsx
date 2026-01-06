import { Fragment, useCallback } from "react";
import { useTranslation } from "react-i18next";
import { useParams } from "react-router-dom";
import { Stack, Text } from "@chakra-ui/react";
import RefreshButton from "../../../../shared/components/RefreshButton/RefreshButton";
import { formatShortDateTime } from "../../../../shared/utilities/formatters/dateFormatter";
import { formatMoneyByCurrencyCulture } from "../../../../shared/utilities/formatters/moneyFormatter";

interface Props {
    name: string,
    currencyName: string,
    currentValue: number,

    onPullQuotations: () => void,
    lastPullDate: Date | null
    isReloading: boolean
}

const BrokerAccountHeader: React.FC<Props> = ({ name, currentValue, currencyName, onPullQuotations, lastPullDate, isReloading }) => {
    const { t, i18n } = useTranslation();

    const { brokerAccountId } = useParams();

    const formatPullDate = useCallback(() => {
        if (!lastPullDate) {
            return "";
        }
        const formattedDate = formatShortDateTime(lastPullDate, i18n, false);
        return t("broker_account_page_last_pull_date", { date: formattedDate });
    }, [i18n, lastPullDate]);

    if (!brokerAccountId) {
        return <Fragment/>
    }

    const currentValueLabel = formatMoneyByCurrencyCulture(currentValue, currencyName);
 
    return <Stack mb={4} alignItems={"end"} gapX={2} direction={"row"} color="text_primary">
        <Text fontSize="3xl" fontWeight={900}>
            {`${name}: ${currentValueLabel}`}
        </Text>
        { lastPullDate && <Text backgroundColor="background_primary" borderColor="border_primary" textAlign={'center'} minW={150} rounded={10} padding={2} background={'black.600'}>{formatPullDate(lastPullDate)}</Text>}
        <RefreshButton transparent isRefreshing={isReloading} onClick={onPullQuotations}/>
    </Stack>;
}

export default BrokerAccountHeader;