import { Fragment, useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { useParams } from "react-router-dom";
import { getSecurityById, getSecurityStats } from "../../api/securities/securityApi";
import { SecurityEntity } from "../../models/securities/SecurityEntity";
import { Card, SimpleGrid, Stack, Tabs, Text} from "@chakra-ui/react";
import { formatMoneyByCurrencyCulture } from "../../shared/utilities/formatters/moneyFormatter";
import DividendList from "./components/DividendsList/DividendList";
import SecurityHistory from "./components/SecurityHistory/SecurityHistory";
import SecurityTransactionsChart from "./components/SecurityTransactionsChart/SecurityTransactionsChart";
import { SecurityStats } from "../../models/securities/SecurityStats";
import { GrTransaction } from "react-icons/gr";
import { MdHistory } from "react-icons/md";
import { PiCoinsLight } from "react-icons/pi";

interface State {
	security: SecurityEntity,
	securityStats: SecurityStats
}

const SecurityPage: React.FC = () => {
	const { t } = useTranslation();

	const { securityId } = useParams();

	const [state, setState] = useState<State>({ security: null!, securityStats: null! })

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

	const { ticker, name, type, actualPrice, currency} = state.security;
 	
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

  		<Tabs.Root variant="enclosed" defaultValue="transactions">
			<Tabs.List background={"background_primary"}>
				<Tabs.Trigger _selected={{bg: "purple.600"}} color="text_primary" value="history">
					<MdHistory/>
					{t("security_page_tabs_history")}
				</Tabs.Trigger>
				<Tabs.Trigger _selected={{bg: "purple.600"}} color="text_primary" value="transactions">
					<GrTransaction />
					 {t("security_page_tabs_transactions")}
				</Tabs.Trigger>
				<Tabs.Trigger _selected={{bg: "purple.600"}} color="text_primary" value="dividends">
					<PiCoinsLight />
					 {t("security_page_tabs_dividends")}
				</Tabs.Trigger>
			</Tabs.List>
			<Tabs.Content value="history">
				<SecurityHistory ticker={ticker}/>
			</Tabs.Content>
			<Tabs.Content value="transactions">
				<SecurityTransactionsChart securityId={securityId}/>
			</Tabs.Content>
			<Tabs.Content value="dividends">
				<DividendList securityId={securityId}/>
			</Tabs.Content>
		</Tabs.Root>
	</Stack>
}

export default SecurityPage;
