import { useTranslation } from "react-i18next";
import { Stack, Text } from "@chakra-ui/react";
import { calculateDiff } from "../../../../shared/utilities/numericDiffsUtilities";
import { formatMoneyByCurrencyCulture } from "../../../../shared/utilities/formatters/moneyFormatter";

interface Props {
    initialValue: number
    currentValue: number
    dividendIncomes: number
    taxDeductionIncomes: number
    currencyName: string
}

const BrokerAccountValuesSummary: React.FC<Props> = ({ initialValue, currentValue, dividendIncomes, taxDeductionIncomes, currencyName }) => {
    const { t } = useTranslation();

    const dividendsLabel = dividendIncomes ?
        formatMoneyByCurrencyCulture(dividendIncomes, currencyName):
        "";

    const taxDeductionsLabel = taxDeductionIncomes ?
        formatMoneyByCurrencyCulture(taxDeductionIncomes, currencyName):
        "";

    const profitAndLossValue = currentValue + dividendIncomes + taxDeductionIncomes

    const {profitAndLoss, profitAndLossPercentage, color} = calculateDiff(currentValue, initialValue, currencyName);
    const profitAndLossWithDividends = calculateDiff(profitAndLossValue, initialValue, currencyName);

    return <Stack direction={"row"} color="text_primary">
        <Text backgroundColor="background_primary" borderColor="border_primary" color={color} textAlign={'center'} minW={150} rounded={10} padding={2} background={'black.600'}>{t("broker_account_page_securities_profit_and_loss")}: {profitAndLoss} | {profitAndLossPercentage}%</Text>
        {taxDeductionsLabel && <Text color={"green.600"} backgroundColor="background_primary" borderColor="border_primary" textAlign={'center'} minW={150} rounded={10} padding={2}>{t("broker_account_page_deduction_taxes")}: {taxDeductionsLabel}</Text>}
        {dividendsLabel && <Text color={"green.600"} backgroundColor="background_primary" borderColor="border_primary" textAlign={'center'} minW={150} rounded={10} padding={2}>{t("broker_account_page_dividends_earnings")}: {dividendsLabel}</Text>}
        <Text backgroundColor="background_primary" borderColor="border_primary" color={profitAndLossWithDividends.color} textAlign={'center'} minW={150} rounded={10} padding={2}>{t("broker_account_page_total_profit_and_loss")}: {profitAndLossWithDividends.profitAndLoss}  | {profitAndLossWithDividends.profitAndLossPercentage}%</Text>
    </Stack>
}

export default BrokerAccountValuesSummary;