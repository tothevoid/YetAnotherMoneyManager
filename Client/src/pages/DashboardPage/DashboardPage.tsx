import { useTranslation } from "react-i18next";
import { Box, Card, Grid, Span, Stack} from "@chakra-ui/react";
import { Text} from "@chakra-ui/react";
import { MdTrendingUp } from "react-icons/md";
import { useUserProfile } from "../../contexts/UserProfileContext";
import { Fragment, useEffect, useState } from "react";
import { getDashboard } from "../../api/dashboard/dashobardApi";
import { Dashboard } from "../../models/dashboard/DashboardEntity";
import { formatMoneyByCurrencyCulture } from "../../formatters/moneyFormatter";


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
    return (
        <Stack color="text_primary">
            <Text fontWeight={900} fontSize={"3xl"}>{t("dashboard_title")}</Text>
            <Card.Root backgroundColor="background_primary" borderColor="border_primary" marginY={5}>
                <Card.Body color="text_primary">
                    <Grid templateColumns="repeat(3, 1fr)">
                        <Box>
                            <Text>{t("dashboard_total")} {user?.currency.name}</Text>
                            <Text fontWeight={900}>{formatMoneyByCurrencyCulture(dashboard?.total ?? 0, currency)}</Text>
                        </Box>
                        <Box>
                            <Text>{t("dashboard_total_in_securities")}</Text>
                            <Text fontWeight={900}>{formatMoneyByCurrencyCulture(dashboard?.brokerAccountStats.total ?? 0, currency)}</Text>
                        </Box>
                        <Box>
                            <Text>{t("dashboard_total_in_accounts")}</Text>
                            <Text fontWeight={900}>{formatMoneyByCurrencyCulture(dashboard?.accountStats.total ?? 0, currency)}</Text>
                        </Box>
                    </Grid>
                </Card.Body>
            </Card.Root>
            <Grid gap={10} templateColumns="repeat(2, 1fr)" marginY={5}>
                <Card.Root backgroundColor="background_primary" borderColor="border_primary">
                    <Card.Body color="text_primary">
                        <Stack gapY={2}>
                            <Text fontWeight={700} fontSize={"xl"}>{t("dashboard_cash")}</Text>
                            <Text fontWeight={600} fontSize={"md"}>Total: {formatMoneyByCurrencyCulture(dashboard.accountStats.totalCash, currency)}</Text>
                            {
                                dashboard.accountStats.cashDistribution.map((cash) => 
                                <Stack gapX={2} direction={"row"}>
                                        <Text>
                                            {cash.name} - {formatMoneyByCurrencyCulture(cash.amount, cash.currency)}
                                        </Text>
                                        <Text>
                                            ({formatMoneyByCurrencyCulture(cash.convertedAmount, currency)})
                                        </Text>
                                </Stack>
                                )
                            }
                        </Stack>
                    </Card.Body>
                </Card.Root>
                <Card.Root backgroundColor="background_primary" borderColor="border_primary" >
                    <Card.Body color="text_primary">
                        <Stack gapY={2}>
                            <Text fontWeight={700} fontSize={"xl"}>{t("dashboard_securities")}</Text>
                            <Text fontWeight={600} fontSize={"md"}>Total: {formatMoneyByCurrencyCulture(dashboard.brokerAccountStats.total, currency)}</Text>
                            {
                                dashboard.brokerAccountStats.distribution.map((brokerAccount) => 
                                <Stack gapX={2} direction={"row"}>
                                        <Text>
                                            {brokerAccount.name} - {formatMoneyByCurrencyCulture(brokerAccount.amount, brokerAccount.currency)}
                                        </Text>
                                        <Text>
                                            ({formatMoneyByCurrencyCulture(brokerAccount.convertedAmount, currency)})
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
                            <Text fontWeight={700} fontSize={"xl"}>{t("dashboard_deposits")}</Text>
                            <Text fontWeight={600} fontSize={"md"}>Total: {formatMoneyByCurrencyCulture(dashboard.accountStats.totalDeposit, currency)}</Text>
                            {
                                dashboard.accountStats.depositsDistribution.map((deposit) => 
                                <Stack gapX={2} direction={"row"}>
                                        <Text>
                                            {deposit.name} - {formatMoneyByCurrencyCulture(deposit.amount, deposit.currency)}
                                        </Text>
                                        <Text>
                                            ({formatMoneyByCurrencyCulture(deposit.convertedAmount, currency)})
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
                            <Text fontWeight={700} fontSize={"xl"}>{t("dashboard_bank_accounts")}</Text>
                            <Text fontWeight={600} fontSize={"md"}>Total: {formatMoneyByCurrencyCulture(dashboard.accountStats.totalBankAccount, currency)}</Text>
                            {
                                dashboard.accountStats.bankAccountsDistribution.map((bankAccount) => 
                                <Stack gapX={2} direction={"row"}>
                                        <Text>
                                            {bankAccount.name} - {formatMoneyByCurrencyCulture(bankAccount.amount, bankAccount.currency)}
                                        </Text>
                                        <Text>
                                            ({formatMoneyByCurrencyCulture(bankAccount.convertedAmount, currency)})
                                        </Text>
                                </Stack>
                                )
                            }
                        </Stack>
                    </Card.Body>
                </Card.Root>
                {/* <Card.Root backgroundColor="background_primary" borderColor="border_primary">
                    <Card.Body color="text_primary">
                        <Text fontWeight={700} fontSize={"xl"}>Crypto</Text>
                        <Text fontWeight={600} fontSize={"md"}>In progress...</Text>
                    </Card.Body>
                </Card.Root> */}
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
