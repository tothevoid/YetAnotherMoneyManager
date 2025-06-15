import { Fragment, useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { useParams } from "react-router-dom";
import { getSecurityById, getSecurityStats } from "../../api/securities/securityApi";
import { SecurityEntity } from "../../models/securities/SecurityEntity";
import { Box, Card, HStack, RadioCard, SimpleGrid, Stack, Text} from "@chakra-ui/react";
import { formatMoneyByCurrencyCulture } from "../../shared/utilities/formatters/moneyFormatter";
import DividendList from "./components/DividendsList/DividendList";
import SecurityHistory from "./components/SecurityHistory/SecurityHistory";
import SecurityTransactionsChart from "./components/SecurityTransactionsChart/SecurityTransactionsChart";
import { SecurityStats } from "../../models/securities/SecurityStats";

interface State {
	security: SecurityEntity,
	securityStats: SecurityStats
}

enum DataSource {
	History="History",
	Transactions="Transactions",
	Dividends="Dividends"
}

const SecurityPage: React.FC = () => {
	const { t } = useTranslation();

	const { securityId } = useParams();

	const [state, setState] = useState<State>({ security: null!, securityStats: null! })
	const [tab, setTab] = useState(DataSource.History);

	useEffect(() => {
		initData();
	}, []);

	if (!securityId) {
		return <Fragment/>
	}

	const initData = async () => {
		const security = await getSecurityById(securityId);
		const securityStats = await getSecurityStats(securityId);
		if (!security || !securityStats) {
			return;
		}

		setState((currentState) => {
			return {...currentState, security, securityStats}
		})
	}

	if (!state.security) {
		return <Fragment/>
	}

	const tabs = [
		{ value: DataSource.History, title: t("security_page_tabs_history")},
		{ value: DataSource.Transactions, title: t("security_page_tabs_transactions") },
		{ value: DataSource.Dividends, title: t("security_page_tabs_dividends") },
	]	

	const { ticker, name, type, actualPrice, currency} = state.security;
 	
	const renderActiveTab = () => {
		switch (tab) {
			case DataSource.History:
				return <SecurityHistory ticker={ticker}/>
			case DataSource.Transactions:
				return <SecurityTransactionsChart securityId={securityId}/>
			case DataSource.Dividends:
				return <DividendList securityId={securityId}/>
			default:
				<Fragment/>
		}
	}

	const formatStatsCard = (value: string) => {
		return <Card.Root backgroundColor="background_primary" borderColor="border_primary" color="text_primary">
			<Card.Body>
				{value}
			</Card.Body>
		</Card.Root>
	}

	return <Stack color="text_primary">
		<Stack alignItems="center" gap={2} direction="row">
			<Text fontWeight={900} fontSize="3xl">{ticker}</Text>
			<Text>({name})</Text>
			<Text paddingX={4} paddingY={1} borderRadius={4} backgroundColor="purple.600">{type.name}</Text>
		</Stack>
		<Text>{formatMoneyByCurrencyCulture(actualPrice, currency.name)}</Text>
		<SimpleGrid pt={5} pb={5} gap={4} templateColumns='repeat(auto-fill, minmax(350px, 3fr))'>
			{formatStatsCard(t("security_page_stats_securities", {securities: state.securityStats.hasOnBrokerAccounts}))}
			{formatStatsCard(t("security_page_stats_transactions_min", {min: formatMoneyByCurrencyCulture(state.securityStats.transactionsMin, currency.name)}))}
			{formatStatsCard(t("security_page_stats_transactions_max", {max: formatMoneyByCurrencyCulture(state.securityStats.transactionsMax, currency.name)}))}
			{formatStatsCard(t("security_page_stats_transactions_avg", {avg: formatMoneyByCurrencyCulture(state.securityStats.transactionsAvg, currency.name)}))}
			{formatStatsCard(t("security_page_stats_dividends_income", {income: formatMoneyByCurrencyCulture(state.securityStats.dividendsIncome, currency.name)}))}
		</SimpleGrid>
		<RadioCard.Root
			orientation="horizontal"
			align="center"
			justify="center"
			maxW="xl"
			defaultValue={DataSource.History}
			value={tab} onValueChange={(e) => setTab(e.value as DataSource)}>
			<HStack align="stretch">
				{tabs.map((item) => (
					<RadioCard.Item borderWidth={"2px"} borderColor={item.value === tab ? "border_primary": "transparent"} 
						background={"background_primary"} key={item.value} value={item.value}>
						<RadioCard.ItemHiddenInput />
						<RadioCard.ItemControl>
							<RadioCard.ItemText>{item.title}</RadioCard.ItemText>
						</RadioCard.ItemControl>
					</RadioCard.Item>
				))}
			</HStack>
		</RadioCard.Root>
		{renderActiveTab()}
	</Stack>
}

export default SecurityPage;
