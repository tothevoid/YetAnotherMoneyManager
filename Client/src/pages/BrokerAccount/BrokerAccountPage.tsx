import { Fragment, useEffect, useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import { useParams } from "react-router-dom";
import { BrokerAccountEntity } from "../../models/brokers/BrokerAccountEntity";
import { getBrokerAccountById } from "../../api/brokers/brokerAccountApi";
import { Button, Icon, Span, Stack, Text } from "@chakra-ui/react";
import { formatMoneyByCurrencyCulture } from "../../shared/utilities/formatters/moneyFormatter";
import { calculateDiff } from "../../shared/utilities/numericDiffsUtilities";
import { MdRefresh } from "react-icons/md";
import { pullBrokerAccountQuotations } from "../../api/brokers/brokerAccountSecurityApi";
import "./BrokerAccountPage.scss"
import BrokerAccountSecuritiesList, { BrokerAccountSecuritiesListRef } from "./components/BrokerAccountSecuritiesList/BrokerAccountSecuritiesList";
import { useSignalR } from "../../shared/hooks/SignalRHook";
import SecurityTransactionsList from "./components/SecurityTransactionsList/SecurityTransactionsList";

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

    const initData = async () => {
        const brokerAccount = await getBrokerAccountById(brokerAccountId);
        if (!brokerAccount) {
            return;
        }

        setState((currentState) => {
            return {...currentState, brokerAccount, isReloading: false}
        })
    }

    useEffect(() => {
        initData();
    }, []);
    

    const onDataReloaded = async () => {
        await initData();
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

    return (<Fragment>
        <Stack alignItems={"end"} gapX={2} direction={"row"} color="text_primary">
            <Text fontSize="3xl" fontWeight={900}> {state.brokerAccount?.name}: </Text>
            <Text fontSize="3xl" fontWeight={900}>
                {currentValueLabel} (<Span color={color}>{profitAndLoss.toFixed(2)} | {profitAndLossPercentage.toFixed(2)}%</Span>)
            </Text>
            <Button disabled={state.isReloading} background="transparent" onClick={pullQuotations}>
                <Icon 
                    transition="transform 0.3s ease"
                    animation={state.isReloading ? 'loading-spin 1.5s linear infinite' : 'none'}
                    size='md'>
                    <MdRefresh/>
                </Icon>
            </Button>
        </Stack>
        <BrokerAccountSecuritiesList ref={securitiesRef} brokerAccountId={brokerAccountId}/>
        <SecurityTransactionsList onDataReloaded={onDataReloaded} brokerAccountId={brokerAccountId}/>
    </Fragment>
    )
}


export default BrokerAccountPage;