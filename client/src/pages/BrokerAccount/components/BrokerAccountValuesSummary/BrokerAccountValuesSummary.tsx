import { useTranslation } from "react-i18next";
import { Stack, Text } from "@chakra-ui/react";
import { formatMoneyByCurrencyCulture } from "../../../../shared/utilities/formatters/moneyFormatter";
import { BrokerAccountPortfolioEntity } from "../../../../models/brokers/BrokerAccountPortfolioEntity";

interface Props {
    portfolio: BrokerAccountPortfolioEntity
    currencyName: string
}

const BrokerAccountValuesSummary: React.FC<Props> = ({ portfolio, currencyName }) => {
    const { t } = useTranslation();

    const {dividendsIncome, profitAndLoss, taxDeductions} = portfolio;

    const dividendsLabel = dividendsIncome ?
        formatMoneyByCurrencyCulture(dividendsIncome, currencyName):
        "";

    const taxDeductionsLabel = taxDeductions ?
        formatMoneyByCurrencyCulture(taxDeductions, currencyName):
        "";

    return <Stack direction={"row"} color="text_primary">
        <Text color={profitAndLoss >= 0 ? "gain": "loss"} backgroundColor="background_primary" borderColor="border_primary" textAlign={'center'} minW={150} rounded={10} padding={2}>{t("broker_account_page_total_profit_and_loss")}: {formatMoneyByCurrencyCulture(profitAndLoss, currencyName)}</Text>
        {taxDeductionsLabel && <Text color={"green.600"} backgroundColor="background_primary" borderColor="border_primary" textAlign={'center'} minW={150} rounded={10} padding={2}>{t("broker_account_page_deduction_taxes")}: {taxDeductionsLabel}</Text>}
        {dividendsLabel && <Text color={"green.600"} backgroundColor="background_primary" borderColor="border_primary" textAlign={'center'} minW={150} rounded={10} padding={2}>{t("broker_account_page_dividends_earnings")}: {dividendsLabel}</Text>}
    </Stack>
}

export default BrokerAccountValuesSummary;