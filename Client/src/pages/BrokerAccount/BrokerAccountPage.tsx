import { Fragment, useEffect, useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import { useParams } from "react-router-dom";
import { BrokerAccountEntity } from "../../models/brokers/BrokerAccountEntity";
import { getBrokerAccountById } from "../../api/brokers/brokerAccountApi";
import { Span, Stack, Tabs, Text } from "@chakra-ui/react";
import { formatMoneyByCurrencyCulture } from "../../shared/utilities/formatters/moneyFormatter";
import { calculateDiff } from "../../shared/utilities/numericDiffsUtilities";
import { pullBrokerAccountQuotations } from "../../api/brokers/brokerAccountSecurityApi";
import BrokerAccountSecuritiesList, { BrokerAccountSecuritiesListRef } from "./components/BrokerAccountSecuritiesList/BrokerAccountSecuritiesList";
import { useSignalR } from "../../shared/hooks/SignalRHook";
import SecurityTransactionsList from "./components/SecurityTransactionsList/SecurityTransactionsList";
import RefreshButton from "../../shared/components/RefreshButton/RefreshButton";
import { GrTransaction } from "react-icons/gr";
import { PiCoinsLight } from "react-icons/pi";
import DividendPaymentsList from "./components/DividendPaymentsList/DividendPaymentsList";

interface Props {}

interface State {
    brokerAccount: BrokerAccountEntity | null,
    isReloading: boolean
}

const BrokerAccountPage: React.FC<Props> = () => {
    const { t } = useTranslation();
    const securitiesRef = useRef<BrokerAccountSecuritiesListRef>(null);

    const { brokerAccountId } = useParams(); // Получаем текущий таб из URL

    if (!brokerAccountId) {
        return <Fragment/>
    }

    const [state, setState] = useState<State>({ brokerAccount: null, isReloading: false })

    const onQuotesRecalculated = async (message: string) => {
        await onDataReloaded();
    }

    useSignalR(onQuotesRecalculated);

    const fetchBrokerAccount = async () => {
        const brokerAccount = await getBrokerAccountById(brokerAccountId);
        if (!brokerAccount) {
            return;
        }

        setState((currentState) => {
            return {...currentState, brokerAccount, isReloading: false}
        })
    }

    useEffect(() => {
        fetchBrokerAccount();
    }, []);

    const onDataReloaded = async () => {
        await fetchBrokerAccount();
        await securitiesRef.current?.reloadData();
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

    const {profitAndLoss, profitAndLossPercentage, color} = calculateDiff(currentValue, initialValue);

    if (!state.brokerAccount) {
        return <Fragment/>
    }

    return <Fragment>
        <Stack alignItems={"end"} gapX={2} direction={"row"} color="text_primary">
            <Text fontSize="3xl" fontWeight={900}> {state.brokerAccount?.name}: </Text>
            <Text fontSize="3xl" fontWeight={900}>
                {currentValueLabel} (<Span color={color}>{profitAndLoss.toFixed(2)} | {profitAndLossPercentage.toFixed(2)}%</Span>)
            </Text>
            <RefreshButton transparent isRefreshing={state.isReloading} onClick={pullQuotations}/>
        </Stack>
        <BrokerAccountSecuritiesList ref={securitiesRef} brokerAccount={state.brokerAccount}/>
        <Tabs.Root variant="enclosed" defaultValue="transactions">
            <Tabs.List background={"background_primary"}>
                <Tabs.Trigger color="text_primary" value="transactions">
                    <GrTransaction />
                    Transactions
                </Tabs.Trigger>
                <Tabs.Trigger color="text_primary" value="dividends">
                    <PiCoinsLight />
                    Dividends
                </Tabs.Trigger>
            </Tabs.List>
            <Tabs.Content value="transactions">
                <SecurityTransactionsList onDataReloaded={onDataReloaded} brokerAccountId={brokerAccountId}/>
            </Tabs.Content>
            <Tabs.Content value="dividends">
                <DividendPaymentsList onDividendsChanged={fetchBrokerAccount} brokerAccountId={brokerAccountId}/>
            </Tabs.Content>
        </Tabs.Root>
       
    </Fragment>
}


export default BrokerAccountPage;