import { useTranslation } from "react-i18next";
import { Box, Card, Grid, Span, Stack} from "@chakra-ui/react";
import { Text} from "@chakra-ui/react";
import { useUserProfile } from "../../contexts/UserProfileContext";
import { Fragment, useEffect, useState } from "react";
import { getDashboard } from "../../api/dashboard/dashobardApi";
import { Dashboard, DistributionModel } from "../../models/dashboard/DashboardEntity";
import { formatMoneyByCurrencyCulture } from "../../formatters/moneyFormatter";
import DistributionChart from "../../components/dashboard/DistributionChart";


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
        { name: t("dashboard_deposits"), convertedAmount: dashboard.accountStats.totalDeposit, currency, amount: dashboard.accountStats.totalDeposit },
        { name: t("dashboard_bank_accounts"), convertedAmount: dashboard.accountStats.totalBankAccount, currency, amount: dashboard.accountStats.totalBankAccount }
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
                <Card.Root backgroundColor="background_primary" borderColor="border_primary">
                    <Card.Body color="text_primary">
                        <Stack gapY={2}>
                            <Text fontWeight={700} fontSize={"xl"}>{t("dashboard_cash")}: {formatMoneyByCurrencyCulture(dashboard.accountStats.totalCash, currency)}</Text>
                            <DistributionChart data={dashboard.accountStats.cashDistribution} mainCurrency={user.currency.name}/>
                        </Stack>
                    </Card.Body>
                </Card.Root>
                <Card.Root backgroundColor="background_primary" borderColor="border_primary" >
                    <Card.Body color="text_primary">
                        <Stack gapY={2}>
                            <Text fontWeight={700} fontSize={"xl"}>{t("dashboard_securities")}: {formatMoneyByCurrencyCulture(dashboard.brokerAccountStats.total, currency)}</Text>
                            <DistributionChart data={dashboard.brokerAccountStats.distribution} mainCurrency={user.currency.name}/>
                        </Stack>
                    </Card.Body>
                </Card.Root>
                <Card.Root backgroundColor="background_primary" borderColor="border_primary">
                    <Card.Body color="text_primary">
                        <Stack gapY={2}>
                            <Text fontWeight={700} fontSize={"xl"}>{t("dashboard_deposits")}: {formatMoneyByCurrencyCulture(dashboard.accountStats.totalDeposit, currency)}</Text>
                            <DistributionChart data={dashboard.accountStats.depositsDistribution} mainCurrency={user.currency.name}/>
                        </Stack>
                    </Card.Body>
                </Card.Root>
                <Card.Root backgroundColor="background_primary" borderColor="border_primary">
                    <Card.Body color="text_primary">
                        <Stack gapY={2}>
                            <Text fontWeight={700} fontSize={"xl"}>{t("dashboard_bank_accounts")}: {formatMoneyByCurrencyCulture(dashboard.accountStats.totalBankAccount, currency)}</Text>
                            <DistributionChart data={dashboard.accountStats.bankAccountsDistribution} mainCurrency={user.currency.name}/>
                        </Stack>
                    </Card.Body>
                </Card.Root>
            </Grid>
            <Grid gap={10} templateColumns="repeat(2, 1fr)" marginY={5}>
                <Card.Root backgroundColor="background_primary" borderColor="border_primary">
                    <Card.Body color="text_primary">
                        <Stack gapY={2}>
                            <Text fontWeight={700} fontSize={"xl"}>Spents</Text>
                            <Text fontWeight={600} fontSize={"md"}>Total: {formatMoneyByCurrencyCulture(dashboard.transactionStats.spentsTotal, currency)}</Text>
                            {
                               dashboard.transactionStats.spentsDistribution.map((spent) => 
                                    <Stack gapX={2} direction={"row"}>
                                        <Text>
                                            {spent.name} - {formatMoneyByCurrencyCulture(spent.amount, spent.currency)}
                                        </Text>
                                        <Text>
                                            ({formatMoneyByCurrencyCulture(spent.convertedAmount, currency)})
                                        </Text>
                                    </Stack>
                                )
                            }
                        </Stack>
                    </Card.Body>
                </Card.Root>
                <Card.Root backgroundColor="background_primary" borderColor="border_primary">
                    <Card.Body color="text_primary">
                         <Stack gapY={2}>
                            <Text fontWeight={700} fontSize={"xl"}>Incomes</Text>
                            <Text fontWeight={600} fontSize={"md"}>Total: {formatMoneyByCurrencyCulture(dashboard.transactionStats.incomesTotal, currency)}</Text>
                            {
                               dashboard.transactionStats.incomesDistribution.map((spent) => 
                                    <Stack gapX={2} direction={"row"}>
                                        <Text>
                                            {spent.name} - {formatMoneyByCurrencyCulture(spent.amount, spent.currency)}
                                        </Text>
                                        <Text>
                                            ({formatMoneyByCurrencyCulture(spent.convertedAmount, currency)})
                                        </Text>
                                    </Stack>
                                )
                            }
                        </Stack>
                    </Card.Body>
                </Card.Root>
            </Grid>
        </Stack>
    )
}

export default DashboardPage;
