import { Fragment, useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { useParams } from "react-router-dom";
import { getSecurityById } from "../../api/securities/securityApi";
import { SecurityEntity } from "../../models/securities/SecurityEntity";
import { HStack, RadioCard, Stack, Text} from "@chakra-ui/react";
import { formatMoneyByCurrencyCulture } from "../../shared/utilities/formatters/moneyFormatter";
import DividendList from "./components/DividendsList/DividendList";
import SecurityHistory from "./components/SecurityHistory/SecurityHistory";
import SecurityTransactionsChart from "./components/SecurityTransactionsChart/SecurityTransactionsChart";

interface State {
	security: SecurityEntity
}

enum DataSource {
	History="History",
	Transactions="Transactions",
	Dividends="Dividends"
}

const SecurityPage: React.FC = () => {
	const { t } = useTranslation();

	const { securityId } = useParams();

	const [state, setState] = useState<State>({ security: null! })
	const [tab, setTab] = useState(DataSource.History);

	useEffect(() => {
		initData();
	}, []);

	if (!securityId) {
		return <Fragment/>
	}

	const initData = async () => {
		const security = await getSecurityById(securityId);
		if (!security) {
			return;
		}

		setState((currentState) => {
			return {...currentState, security}
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

	return <Stack color="text_primary">
		<Stack alignItems="center" gap={2} direction="row">
			<Text fontWeight={900} fontSize="3xl">{ticker}</Text>
			<Text>({name})</Text>
			<Text paddingX={4} paddingY={1} borderRadius={4} backgroundColor="purple.600">{type.name}</Text>
		</Stack>
		<Text>{formatMoneyByCurrencyCulture(actualPrice, currency.name)}</Text>
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
