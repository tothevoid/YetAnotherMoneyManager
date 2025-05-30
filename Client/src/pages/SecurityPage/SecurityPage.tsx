import { Fragment, useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { useParams } from "react-router-dom";
import { getSecurityById } from "../../api/securities/securityApi";
import { SecurityEntity } from "../../models/securities/SecurityEntity";
import { HStack, RadioCard, Stack, Text} from "@chakra-ui/react";
import DividendList from "../../components/securities/DividendsList/DividendList";
import SecurityHistory from "../../components/securities/SecurityHistory/SecurityHistory";
import { formatMoneyByCurrencyCulture } from "../../formatters/moneyFormatter";
import SecurityTransactionsChart from "../../components/securities/SecurityTransactionsChart/SecurityTransactionsChart";
import Dividend from "../../components/securities/Dividend/Dividend";

interface Props {}

interface State {
	security: SecurityEntity
}

enum DataSource {
	History="History",
	Transactions="Transactions",
	Dividends="Dividends"
}

const SecurityPage: React.FC<Props> = () => {
	const { t } = useTranslation();

	const { securityId } = useParams();

	const [state, setState] = useState<State>({ security: null })
	const [tab, setTab] = useState(DataSource.History);


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

	useEffect(() => {
		initData();
	}, []);

	if (!state.security) {
		return <Fragment/>
	}

	const tabs = [
		{ value: DataSource.History, title: "History"},
		{ value: DataSource.Transactions, title: "Transactions" },
		{ value: DataSource.Dividends, title: "Dividends" },
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
	return (
		<Stack color="text_primary">
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
				maxW="lg"
				defaultValue={DataSource.History}
				value={tab} onValueChange={(e) => setTab(e.value as DataSource)}>
				<HStack align="stretch">
					{tabs.map((item) => (
					<RadioCard.Item key={item.value} value={item.value}>
						<RadioCard.ItemHiddenInput />
						<RadioCard.ItemControl>
							<RadioCard.ItemText ms="-4">{item.title}</RadioCard.ItemText>
						</RadioCard.ItemControl>
					</RadioCard.Item>
					))}
				</HStack>
			</RadioCard.Root>
			{renderActiveTab()}
		</Stack>
	)
}

export default SecurityPage;
