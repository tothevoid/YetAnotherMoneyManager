import { useTranslation } from "react-i18next";
import { Box, Card, Grid, Stack} from "@chakra-ui/react";
import { Text} from "@chakra-ui/react";
import { useUserProfile } from "../../../features/UserProfileSettingsModal/hooks/UserProfileContext";
import { Fragment, useEffect, useState } from "react";
import { getDashboard } from "../../api/dashboard/dashobardApi";
import { Dashboard, DistributionModel } from "../../models/dashboard/DashboardEntity";
import { formatMoneyByCurrencyCulture } from "../../shared/utilities/formatters/moneyFormatter";
import DistributionChart from "./components/DistributionChart";

interface Props {}

interface State {
	dashboard: Dashboard | null
}
   

const DashboardPage: React.FC<Props> = () => {
	const { t } = useTranslation();

	const { user } = useUserProfile();

	const [state, setState] = useState<State>({dashboard: null});

	useEffect(() => {
		const initData = async () => {
			const dashboard = await getDashboard();

			if (!dashboard) {
				return;
			}

			setState((currentState) => {
				return {...currentState, dashboard};
			});
		}
		initData();
	}, []);

	const currency = user?.currency.name ?? "";

	if (!state.dashboard || !user) {
		return <Fragment/>
	}

	const {dashboard} = state;

	const totalsData: DistributionModel[] = [
		{ name: t("dashboard_cash"), convertedAmount: dashboard.accountStats.totalCash, currency, amount: dashboard.accountStats.totalCash },
		{ name: t("dashboard_securities"), convertedAmount: dashboard.brokerAccountStats.total, currency, amount: dashboard.brokerAccountStats.total},
		{ name: t("dashboard_deposits"), convertedAmount: dashboard.depositStats.totalStartedAmount, currency, amount: dashboard.depositStats.totalStartedAmount},
		{ name: t("dashboard_deposit_incomes"), convertedAmount: dashboard.depositStats.totalEarned, currency, amount: dashboard.depositStats.totalEarned },
		{ name: t("dashboard_bank_accounts"), convertedAmount: dashboard.accountStats.totalBankAccount, currency, amount: dashboard.accountStats.totalBankAccount },
		{ name: t("dashboard_debts"), convertedAmount: dashboard.debtStats.total, currency, amount: dashboard.debtStats.total }
	]

	const formatDistributionCard = (title: string, total: number, distribution: DistributionModel[]) => {
		if (!distribution.length) {
			return <Fragment/>
		}

		return <Card.Root backgroundColor="background_primary" borderColor="border_primary">
			<Card.Body color="text_primary">
				<Stack gapY={2}>
					<Text fontWeight={700} fontSize={"xl"}>{title}: {formatMoneyByCurrencyCulture(total, currency)}</Text>
					<DistributionChart data={distribution} mainCurrency={user.currency.name}/>
				</Stack>
			</Card.Body>
		</Card.Root>
	}

	const assetsCharts = [
		formatDistributionCard(t("dashboard_cash"), dashboard.accountStats.totalCash, dashboard.accountStats.cashDistribution),
		formatDistributionCard(t("dashboard_securities"), dashboard.brokerAccountStats.total, dashboard.brokerAccountStats.distribution),
		formatDistributionCard(t("dashboard_deposits"), dashboard.depositStats.totalStartedAmount, dashboard.depositStats.startedAmountDistribution),
		formatDistributionCard(t("dashboard_deposit_incomes"), dashboard.depositStats.totalEarned, dashboard.depositStats.earningsDistribution),
		formatDistributionCard(t("dashboard_bank_accounts"), dashboard.accountStats.totalBankAccount, dashboard.accountStats.bankAccountsDistribution),
		formatDistributionCard(t("dashboard_debts"), dashboard.debtStats.total, dashboard.debtStats.distribution)
	];

	const transactionsStats = [
		formatDistributionCard("Spents", dashboard.transactionStats.spentsTotal, dashboard.transactionStats.spentsDistribution),
		formatDistributionCard("Incomes", dashboard.transactionStats.incomesTotal, dashboard.transactionStats.incomesDistribution)
	]

	return (
		<Stack color="text_primary">
			<Text fontWeight={900} fontSize={"3xl"}>{t("dashboard_title")}</Text>
			<Card.Root backgroundColor="background_primary" borderColor="border_primary" marginY={5}>
				<Card.Body color="text_primary">
					<Grid templateColumns="repeat(3, 1fr)">
						<Box>
							<Text fontWeight={700} fontSize={"xl"}>{t("dashboard_total")}: {formatMoneyByCurrencyCulture(dashboard?.total ?? 0, currency)}</Text>
						</Box>
						<DistributionChart data={totalsData} mainCurrency={user.currency.name}/>
					</Grid>
				</Card.Body>
			</Card.Root>
			<Grid gap={10} templateColumns="repeat(2, 1fr)" marginY={5}>
				{assetsCharts}
			</Grid>
			<Grid gap={10} templateColumns="repeat(2, 1fr)" marginY={5}>
				{transactionsStats}
			</Grid>
		</Stack>
	)
}

export default DashboardPage;
