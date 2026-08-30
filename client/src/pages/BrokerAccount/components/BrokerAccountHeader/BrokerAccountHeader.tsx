import { useCallback } from "react";
import { useTranslation } from "react-i18next";
import { Card, Flex, HStack, Text, Icon } from "@chakra-ui/react";
import { formatShortDateTime } from "../../../../shared/utilities/formatters/dateFormatter";
import { formatMoneyByCurrencyCulture } from "../../../../shared/utilities/formatters/moneyFormatter";
import { BrokerAccountPortfolioEntity } from "../../../../models/brokers/BrokerAccountPortfolioEntity";
import { BsWallet2, BsPiggyBank, BsArrowUpRight, BsArrowDownRight, BsClockHistory, BsBank } from "react-icons/bs";
import { MdRefresh } from "react-icons/md";
import { TbReceiptTax } from "react-icons/tb";
import { MetricItem } from "../../../../shared/components/MetricItem/MetricItem";
import "../../../../shared/components/RefreshButton/RefreshButton.scss";

interface Props {
    name: string;
    currencyName: string;
    portfolio: BrokerAccountPortfolioEntity;
    onPullQuotations: () => void;
    lastPullDate: Date | null;
    isReloading: boolean;
}

const BrokerAccountHeader: React.FC<Props> = ({
    name,
    currencyName,
    portfolio,
    onPullQuotations,
    lastPullDate,
    isReloading
}) => {
    const { t, i18n } = useTranslation();

    const formatPullDate = useCallback(() => {
        if (!lastPullDate) {
            return "";
        }
        const formattedDate = formatShortDateTime(lastPullDate, i18n, false);
        return t("broker_account_page_last_pull_date", { date: formattedDate });
    }, [i18n, lastPullDate, t]);

    const currentValueLabel = formatMoneyByCurrencyCulture(portfolio.currentAmount, currencyName);
    const pnlLabel = formatMoneyByCurrencyCulture(portfolio.profitAndLoss, currencyName);
    const isProfit = portfolio.profitAndLoss >= 0;

    const mainCurrencyLabel = formatMoneyByCurrencyCulture(portfolio.mainCurrencyAmount, currencyName);
    const dividendsLabel = portfolio.dividendsIncome
        ? formatMoneyByCurrencyCulture(portfolio.dividendsIncome, currencyName)
        : null;
    const taxDeductionsLabel = portfolio.taxDeductions
        ? formatMoneyByCurrencyCulture(portfolio.taxDeductions, currencyName)
        : null;

    return (
        <Card.Root
            backgroundColor="background_primary"
            borderColor="border_primary"
            borderRadius="xl"
            mb={4}
            boxShadow="sm"
        >
            <Card.Body padding={4}>
                {/* Header Top Bar: Account Title & Sync Action */}
                <Flex justifyContent="space-between" alignItems="center" flexWrap="wrap" gap={3} mb={3}>
                    <Text fontSize="xl" fontWeight={800} color="text_primary">
                        {name}
                    </Text>

                    {/* Integrated Sync Date & Refresh Pill Button */}
                    <HStack
                        as="button"
                        onClick={isReloading ? undefined : onPullQuotations}
                        px={3}
                        py={1.5}
                        borderRadius="md"
                        backgroundColor="background_secondary"
                        borderColor="border_primary"
                        borderWidth="1px"
                        color="text_secondary"
                        cursor={isReloading ? "not-allowed" : "pointer"}
                        opacity={isReloading ? 0.6 : 1}
                        transition="all 0.2s"
                        _hover={isReloading ? {} : { backgroundColor: "background_primary", borderColor: "action_primary" }}
                        alignItems="center"
                        gap={2}
                    >
                        <Icon color="text_secondary">
                            <BsClockHistory size={13} />
                        </Icon>
                        {lastPullDate && (
                            <Text fontSize="xs" fontWeight={500} color="text_secondary">
                                {formatPullDate()}
                            </Text>
                        )}
                        <Icon
                            transition="transform 0.3s ease"
                            animation={isReloading ? "loading-spin 1.5s linear infinite" : "none"}
                            color="action_primary"
                        >
                            <MdRefresh size={16} />
                        </Icon>
                    </HStack>
                </Flex>

                {/* Metrics list */}
                <Flex
                    pt={3}
                    mt={3}
                    borderTopWidth="1px"
                    borderColor="border_primary"
                    justifyContent="flex-start"
                    alignItems="center"
                    flexWrap="wrap"
                    gap={{ base: 4, md: 8 }}
                >
                    <MetricItem
                        icon={<BsBank size={15} />}
                        iconBg="rgba(234, 179, 8, 0.15)"
                        iconColor="yellow.400"
                        label={t("broker_account_portfolio_history_portfolio_value")}
                        value={currentValueLabel}
                        size="sm"
                    />

                    <MetricItem
                        icon={isProfit ? <BsArrowUpRight size={15} /> : <BsArrowDownRight size={15} />}
                        iconBg={isProfit ? "pnl_positive_bg" : "pnl_negative_bg"}
                        iconColor={isProfit ? "pnl_positive" : "pnl_negative"}
                        label={t("broker_account_page_total_profit_and_loss")}
                        value={isProfit ? `+${pnlLabel}` : pnlLabel}
                        valueColor={isProfit ? "pnl_positive" : "pnl_negative"}
                        size="sm"
                    />

                    <MetricItem
                        icon={<BsWallet2 size={15} />}
                        iconBg="rgba(59, 130, 246, 0.15)"
                        iconColor="blue.400"
                        label={t("broker_account_portfolio_history_main_currency_amount")}
                        value={mainCurrencyLabel}
                        size="sm"
                    />

                    <MetricItem
                        icon={<BsPiggyBank size={15} />}
                        iconBg="rgba(34, 197, 94, 0.15)"
                        iconColor="green.400"
                        label={t("broker_account_page_dividends_earnings")}
                        value={dividendsLabel ?? formatMoneyByCurrencyCulture(0, currencyName)}
                        size="sm"
                    />

                    <MetricItem
                        icon={<TbReceiptTax size={15} />}
                        iconBg="rgba(168, 85, 247, 0.15)"
                        iconColor="purple.400"
                        label={t("broker_account_page_deduction_taxes")}
                        value={taxDeductionsLabel ?? formatMoneyByCurrencyCulture(0, currencyName)}
                        size="sm"
                    />
                </Flex>
            </Card.Body>
        </Card.Root>
    );
};

export default BrokerAccountHeader;