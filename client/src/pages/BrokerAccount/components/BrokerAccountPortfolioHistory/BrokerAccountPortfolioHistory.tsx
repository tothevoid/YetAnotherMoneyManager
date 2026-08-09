import { Fragment, useCallback, useEffect, useState } from "react";
import { SimpleGrid, Stack, Text, Input, Skeleton } from "@chakra-ui/react";
import DatePicker from "react-datepicker";
import { useTranslation } from "react-i18next";
import { useUserProfile } from "../../../../../features/UserProfileSettingsModal/hooks/UserProfileContext";
import MoneyCard from "../../../../shared/components/MoneyCard/MoneyCard";
import { Nullable } from "../../../../shared/utilities/nullable";
import { getBrokerAccountPortfolioHistory } from "../../../../api/brokers/brokerAccountPortfolioHistoryApi";
import { BrokerAccountPortfolioHistoryEntity } from "../../../../models/brokers/BrokerAccountPortfolioHistoryEntity";

interface Props {
    brokerAccountId?: Nullable<string>;
}

const formatDateToIso = (date: Date): string => {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");
    return `${year}-${month}-${day}`;
};

const BrokerAccountPortfolioHistory: React.FC<Props> = ({ brokerAccountId }) => {
    const { user } = useUserProfile();
    const { t } = useTranslation();

    const [selectedDate, setSelectedDate] = useState<Date>(new Date());
    const [history, setHistory] = useState<BrokerAccountPortfolioHistoryEntity | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(false);

    const fetchHistory = useCallback(async (date: Date) => {
        setIsLoading(true);
        try {
            const isoDate = formatDateToIso(date);
            const result = await getBrokerAccountPortfolioHistory(brokerAccountId, isoDate);
            if (result) {
                setHistory(result);
            }
        } finally {
            setIsLoading(false);
        }
    }, [brokerAccountId]);

    useEffect(() => {
        if (selectedDate) {
            fetchHistory(selectedDate);
        }
    }, [selectedDate, fetchHistory]);

    if (!user) return <Fragment />;

    const currencyName = user.currency.name;

    return (
        <SimpleGrid marginBlock={4} gap={4}>
            <Stack direction="row" alignItems="center" gap={4}>
                <Text color="text_primary" fontWeight="bold">
                    {t("broker_account_portfolio_history_date_select")}:
                </Text>
                <DatePicker
                    autoComplete="off"
                    selected={selectedDate}
                    onChange={(date: Date | null) => {
                        if (date) {
                            setSelectedDate(date);
                        }
                    }}
                    dateFormat="dd.MM.yyyy"
                    customInput={
                        <Input
                            width="200px"
                            color="text_primary"
                            backgroundColor="background_primary"
                            borderColor="border_primary"
                        />
                    }
                />
            </Stack>

            {isLoading ? (
                <>
                    <SimpleGrid columns={2} gap={4}>
                        <Skeleton height="100px" borderRadius="md" />
                        <Skeleton height="100px" borderRadius="md" />
                    </SimpleGrid>
                    <SimpleGrid columns={2} gap={4}>
                        <Skeleton height="100px" borderRadius="md" />
                        <Skeleton height="100px" borderRadius="md" />
                    </SimpleGrid>
                    <SimpleGrid columns={2} gap={4}>
                        <Skeleton height="100px" borderRadius="md" />
                        <Skeleton height="100px" borderRadius="md" />
                    </SimpleGrid>
                    <SimpleGrid columns={2} gap={4}>
                        <Skeleton height="100px" borderRadius="md" />
                    </SimpleGrid>
                </>
            ) : (
                history && (
                    <>
                        <SimpleGrid columns={2} gap={4}>
                            <MoneyCard
                                title={t("broker_account_portfolio_history_portfolio_value")}
                                value={history.portfolioValue}
                                currency={currencyName}
                            />
                            <MoneyCard
                                title={t("broker_account_portfolio_history_profit_and_loss")}
                                value={history.profitAndLoss}
                                currency={currencyName}
                            />
                        </SimpleGrid>
                        <SimpleGrid columns={2} gap={4}>
                            <MoneyCard
                                title={t("broker_account_portfolio_history_main_currency_amount")}
                                value={history.mainCurrencyAmount}
                                currency={currencyName}
                            />
                            <MoneyCard
                                title={t("broker_account_portfolio_history_total_dividends")}
                                value={history.totalDividends}
                                currency={currencyName}
                            />
                        </SimpleGrid>
                        <SimpleGrid columns={2} gap={4}>
                            <MoneyCard
                                title={t("broker_account_portfolio_history_total_tax_deduction")}
                                value={history.totalTaxDeduction}
                                currency={currencyName}
                            />
                            <MoneyCard
                                title={t("broker_account_portfolio_history_total_deposited")}
                                value={history.totalDeposited}
                                currency={currencyName}
                            />
                        </SimpleGrid>
                        <SimpleGrid columns={2} gap={4}>
                            <MoneyCard
                                title={t("broker_account_portfolio_history_total_withdrawn")}
                                value={history.totalWithdraw}
                                currency={currencyName}
                            />
                        </SimpleGrid>
                    </>
                )
            )}
        </SimpleGrid>
    );
};

export default BrokerAccountPortfolioHistory;
