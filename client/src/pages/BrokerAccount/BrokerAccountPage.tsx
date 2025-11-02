import { Fragment, useCallback, useEffect, useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import { useParams } from "react-router-dom";
import { BrokerAccountEntity } from "../../models/brokers/BrokerAccountEntity";
import { getBrokerAccountById } from "../../api/brokers/brokerAccountApi";
import { Stack, Tabs, Text } from "@chakra-ui/react";
import { formatMoneyByCurrencyCulture } from "../../shared/utilities/formatters/moneyFormatter";
import { calculateDiff } from "../../shared/utilities/numericDiffsUtilities";
import { getLastPullDate, pullBrokerAccountQuotations } from "../../api/brokers/brokerAccountSecurityApi";
import BrokerAccountSecuritiesList, { BrokerAccountSecuritiesListRef } from "./components/BrokerAccountSecuritiesList/BrokerAccountSecuritiesList";
import { useSignalR } from "../../shared/hooks/SignalRHook";
import SecurityTransactionsList from "./components/SecurityTransactionsList/SecurityTransactionsList";
import RefreshButton from "../../shared/components/RefreshButton/RefreshButton";
import { GrTransaction } from "react-icons/gr";
import { PiCoinsLight } from "react-icons/pi";
import DividendPaymentsList from "./components/DividendPaymentsList/DividendPaymentsList";
import { getEarningsByBrokerAccount } from "../../api/brokers/dividendPaymentApi";
import { MdAttachMoney, MdQueryStats } from "react-icons/md";
import BrokerAccountFundTransfersList from "./components/BrokerAccountFundTransfersList/BrokerAccountFundTransfersList";
import BrokerAccountStats from "./components/BrokerAccountStats/BrokerAccountStats";
import { formatShortDateTime } from "../../shared/utilities/formatters/dateFormatter";

interface State {
    brokerAccount: BrokerAccountEntity | null,
    isReloading: boolean
}

const BrokerAccountPage: React.FC = () => {
    const { t, i18n } = useTranslation();
    const securitiesRef = useRef<BrokerAccountSecuritiesListRef>(null);

    const { brokerAccountId } = useParams(); // Получаем текущий таб из URL

    const [state, setState] = useState<State>({ brokerAccount: null, isReloading: false });

    const [incomes, setIncomes] = useState<number>(0);

    const [lastPullDate, setLastPullDate] = useState<Date | null>(null);

    const onQuotesRecalculated = async (message: string) => {
        const data = JSON.parse(message);
        
        if (data && data.date) {
            const newDate = new Date(data.date);
            setLastPullDate(newDate);
        }

        await onDataReloaded();
    }

    const fetchBrokerAccount = async () => {
        if (!brokerAccountId) {
            return;
        }

        const brokerAccount = await getBrokerAccountById(brokerAccountId);
        if (!brokerAccount) {
            return;
        }

        setState((currentState) => {
            return {...currentState, brokerAccount, isReloading: false}
        })
    }

    const fetchLastPullDate = async () => {
        const lastPullDate = await getLastPullDate();
        if (lastPullDate) {
            setLastPullDate(lastPullDate);
        }
    }

    useSignalR(onQuotesRecalculated);

    useEffect(() => {
        const getData = async () => {
            await fetchBrokerAccount();
            await fetchLastPullDate();
        }

        getData();
    }, []);

    useEffect(() => {
        if (!state.brokerAccount) {
            return;
        }

        const account = state.brokerAccount;

        const getData = async () => {
            const brokerAccountIncomes = await getEarningsByBrokerAccount(account.id);
            setIncomes(brokerAccountIncomes);
        }

        getData();
    }, [state.brokerAccount]);

    const formatPullDate = useCallback((date: Date) => {
        const formattedDate = formatShortDateTime(date, i18n, false);
        return t("broker_account_page_last_pull_date", { date: formattedDate });
    }, [i18n]);

    const onDataReloaded = useCallback(async () => {
        await fetchBrokerAccount();
        await securitiesRef.current?.reloadData();
    }, []);

    const onBrokerAccountFundTransfersChanged = useCallback(async () => {
        await fetchBrokerAccount();
    }, []);

    if (!brokerAccountId) {
        return <Fragment/>
    }

    const pullQuotations = async () => {
       setState((currentState) => {
            return {...currentState, isReloading: true}
        })
        pullBrokerAccountQuotations(brokerAccountId);
    }

    const initialValue = state.brokerAccount?.initialValue ?? 0;
    const currentValue = state.brokerAccount?.currentValue ?? 0

    const currentValueLabel = state.brokerAccount ?
        formatMoneyByCurrencyCulture(currentValue, state.brokerAccount?.currency.name):
        "";

    const dividendsLabel = incomes ?
        formatMoneyByCurrencyCulture(incomes, state.brokerAccount?.currency.name):
        "";

    const {profitAndLoss, profitAndLossPercentage, color} = calculateDiff(currentValue, initialValue, state.brokerAccount?.currency.name);
    const profitAndLossWithDividends = calculateDiff(currentValue + incomes, initialValue, state.brokerAccount?.currency.name);

    if (!state.brokerAccount) {
        return <Fragment/>
    }

    //TODO: tabs style is duplicated

    return <Fragment>
        <Stack mb={4} alignItems={"end"} gapX={2} direction={"row"} color="text_primary">
            <Text fontSize="3xl" fontWeight={900}> {state.brokerAccount?.name}: </Text>
            <Text fontSize="3xl" fontWeight={900}> {currentValueLabel}</Text>
            { lastPullDate && <Text backgroundColor="background_primary" borderColor="border_primary" textAlign={'center'} minW={150} rounded={10} padding={2} background={'black.600'}>{formatPullDate(lastPullDate)}</Text>}
            <RefreshButton transparent isRefreshing={state.isReloading} onClick={pullQuotations}/>
        </Stack>
        <Stack direction={"row"} color="text_primary">
            <Text backgroundColor="background_primary" borderColor="border_primary" color={color} textAlign={'center'} minW={150} rounded={10} padding={2} background={'black.600'}>{t("broker_account_page_securities_profit_and_loss")}: {profitAndLoss} | {profitAndLossPercentage}%</Text>
            <Text backgroundColor="background_primary" borderColor="border_primary" textAlign={'center'} minW={150} rounded={10} padding={2}>{t("broker_account_page_dividends_earnings")}: {dividendsLabel}</Text>
            <Text backgroundColor="background_primary" borderColor="border_primary" color={profitAndLossWithDividends.color} textAlign={'center'} minW={150} rounded={10} padding={2}>{t("broker_account_page_total_profit_and_loss")}: {profitAndLossWithDividends.profitAndLoss}  | {profitAndLossWithDividends.profitAndLossPercentage}%</Text>
        </Stack>
        <BrokerAccountSecuritiesList ref={securitiesRef} brokerAccount={state.brokerAccount}/>
        <Tabs.Root variant="enclosed" defaultValue="stats">
            <Tabs.List background={"background_primary"}>
                 <Tabs.Trigger _selected={{bg: "action_primary"}} color="text_primary" value="stats">
                    <MdQueryStats />
                     {t("broker_account_page_stats_tab")}
                </Tabs.Trigger>
                <Tabs.Trigger _selected={{bg: "action_primary"}} color="text_primary" value="transactions">
                    <GrTransaction />
                    {t("broker_account_page_transactions_tab")}
                </Tabs.Trigger>
                <Tabs.Trigger _selected={{bg: "action_primary"}} color="text_primary" value="dividends">
                    <PiCoinsLight />
                     {t("broker_account_page_dividends_tab")}
                </Tabs.Trigger>
                <Tabs.Trigger _selected={{bg: "action_primary"}} color="text_primary" value="transfers">
                    <MdAttachMoney />
                     {t("broker_account_page_transfers_tab")}
                </Tabs.Trigger>
            </Tabs.List>
            <Tabs.Content value="stats">
               <BrokerAccountStats brokerAccount={state.brokerAccount}/>
            </Tabs.Content>
            <Tabs.Content value="transactions">
                <SecurityTransactionsList onDataReloaded={onDataReloaded} brokerAccountId={brokerAccountId}/>
            </Tabs.Content>
            <Tabs.Content value="dividends">
                <DividendPaymentsList onDividendsChanged={fetchBrokerAccount} brokerAccountId={brokerAccountId}/>
            </Tabs.Content>
            <Tabs.Content value="transfers">
                <BrokerAccountFundTransfersList onDataChanged={onBrokerAccountFundTransfersChanged} brokerAccountId={brokerAccountId}/>
            </Tabs.Content>
        </Tabs.Root>
    </Fragment>
}

export default BrokerAccountPage;