
import React, { Fragment, useCallback, useEffect, useState } from "react";

import { getDailyStats } from "../../../../api/brokers/brokerAccountSummaryApi";
import { Card, SimpleGrid, Stack, Table, Text, Link, Image} from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { BrokerAccountEntity } from "../../../../models/brokers/BrokerAccountEntity";
import { BrokerAccountDailySecurityStatsEntity, BrokerAccountDailyStatsEntity } from "../../../../models/brokers/BrokerAccountDailyStatsEntity";
import { Nullable } from "../../../../shared/utilities/nullable";
import MoneyCard from "../../../../shared/components/MoneyCard/MoneyCard";
import { calculateDiff } from "../../../../shared/utilities/numericDiffsUtilities";
import { formatMoneyByCurrencyCulture } from "../../../../shared/utilities/formatters/moneyFormatter";
import {  formatTime } from "../../../../shared/utilities/formatters/dateFormatter";
import RefreshButton from "../../../../shared/components/RefreshButton/RefreshButton";
import StatsCard from "../../../../shared/components/StatsCard/StatsCard";
import { getIconUrl } from "../../../../api/securities/securityApi";

interface Props {
	brokerAccount: BrokerAccountEntity;
}

const BrokerAccountDailyStats: React.FC<Props> = ({ brokerAccount }) => {
	const { t, i18n } = useTranslation();

	const [dailyStats, setDailyStats] = useState<Nullable<BrokerAccountDailyStatsEntity>>(null);

	const [isRefreshing, setIsRefreshing] = useState(false);

	const fetchDailyStats = useCallback(async (showRefresh: boolean = false) => {
		if (showRefresh) {
			setIsRefreshing(true);
		}
		
		const fetchedDailyStats = await getDailyStats(brokerAccount.id);
		setDailyStats(fetchedDailyStats);

		if (showRefresh) {
			setIsRefreshing(false);
		}
	}, [brokerAccount]);

	useEffect(() => {
		const fetchData = async () => {
			if (dailyStats) {
				return;
			}
			await fetchDailyStats();
		};
		fetchData();
	}, []);

	const formatStat = useCallback((stat: BrokerAccountDailySecurityStatsEntity) => {
		const currencyName = stat.security.currency.name;
		const { profitAndLoss, profitAndLossPercentage, color } = calculateDiff(stat.startPrice, stat.currentPrice, currencyName);

		const minDiff = formatMoneyByCurrencyCulture(stat.minPrice - stat.currentPrice, currencyName);
		const maxDiff = formatMoneyByCurrencyCulture(stat.maxPrice - stat.currentPrice, currencyName);

		return <Table.Row color="text_primary" backgroundColor="background_primary">
			<Table.Cell>
				<Link color="text_primary" href={`/security/${stat.security.id}`}>
					<Image h={8} w={8} rounded={16} src={getIconUrl(stat.security.iconKey)}/>
					{stat.security.ticker}
				</Link>
			</Table.Cell>
			<Table.Cell>{formatMoneyByCurrencyCulture(stat.startPrice, currencyName)}</Table.Cell>
			<Table.Cell>{formatMoneyByCurrencyCulture(stat.currentPrice, currencyName)}</Table.Cell>
			<Table.Cell>{formatMoneyByCurrencyCulture(stat.minPrice, currencyName)} ({minDiff})</Table.Cell>
			<Table.Cell>{formatMoneyByCurrencyCulture(stat.maxPrice, currencyName)} ({maxDiff})</Table.Cell>
			<Table.Cell color={color}>{profitAndLoss}</Table.Cell>
			<Table.Cell color={color}>{profitAndLossPercentage}%</Table.Cell>
		</Table.Row>
	}, []);

	if (!dailyStats) {
		return <Fragment/>
	}

	const { profitAndLoss, profitAndLossPercentage, color } = calculateDiff(dailyStats.currentPortfolioValue, 
		dailyStats.startPortfolioValue, brokerAccount.currency.name);

	return (
		<Stack gapY={4}>
			<Stack direction="row" justifyContent="start" alignItems="center">
				<Text fontSize="3xl" fontWeight={900} color={"text_primary"}>{t("broker_account_daily_stats_title", { date: formatTime(new Date(dailyStats.fetchDate), i18n)})}</Text>
				<RefreshButton isRefreshing={isRefreshing} transparent onClick={() => fetchDailyStats(true)}/>
			</Stack>
			<SimpleGrid columns={2} gap={4}>
				<MoneyCard title={t("broker_account_start_portfolio_title")} value={dailyStats.startPortfolioValue} currency={brokerAccount.currency.name}/>
				<StatsCard color={color} title={t("broker_account_current_portfolio_title")} 
					value={`${formatMoneyByCurrencyCulture(dailyStats.currentPortfolioValue, brokerAccount.currency.name)} (${profitAndLoss} - ${profitAndLossPercentage}%)`} />
			</SimpleGrid>
			<Card.Root backgroundColor="background_primary" borderColor="border_primary" color="text_primary">
				<Card.Header>
					{t("broker_account_daily_stats_dynamics")}
				</Card.Header>
				<Card.Body fontSize="xl" fontWeight={700}>
					<Table.Root>
						<Table.Header>
							<Table.Row backgroundColor="background_primary">
								<Table.ColumnHeader color="text_primary">{t("broker_account_daily_stats_security_column")}</Table.ColumnHeader>
								<Table.ColumnHeader color="text_primary">{t("broker_account_daily_stats_open_value_column")}</Table.ColumnHeader>
								<Table.ColumnHeader color="text_primary">{t("broker_account_daily_stats_current_value_column")}</Table.ColumnHeader>
								<Table.ColumnHeader color="text_primary">{t("broker_account_daily_stats_min_value_column")}</Table.ColumnHeader>
								<Table.ColumnHeader color="text_primary">{t("broker_account_daily_stats_max_value_column")}</Table.ColumnHeader>
								<Table.ColumnHeader color="text_primary">{t("broker_account_daily_stats_diff_column")}</Table.ColumnHeader>
								<Table.ColumnHeader color="text_primary">{t("broker_account_daily_stats_percentage_diff_column")}</Table.ColumnHeader>
							</Table.Row>
						</Table.Header>
						<Table.Body>
						{
							dailyStats.brokerAccountDailySecurityStats.map((stat) => formatStat(stat))
						}
						</Table.Body>
					</Table.Root>
				</Card.Body>
			</Card.Root>
		</Stack>
	);
};

export default BrokerAccountDailyStats;