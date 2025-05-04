import { Fragment, useEffect, useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import BrokerAccountSecuritiesList, { BrokerAccountSecuritiesListRef } from "../../components/brokers/BrokerAccountSecuritiesList/BrokerAccountSecuritiesList";
import { useParams } from "react-router-dom";
import { BrokerAccountEntity } from "../../models/brokers/BrokerAccountEntity";
import { getBrokerAccountById } from "../../api/brokers/brokerAccountApi";
import { Span, Stack, Text } from "@chakra-ui/react";
import SecurityTransactionsList from "../../components/securities/SecurityTransactionsList/SecurityTransactionsList";
import { formatMoneyByCurrencyCulture } from "../../formatters/moneyFormatter";
import { calculateDiff } from "../../utils/NumericDiffsUtilities";

interface Props {}

interface State {
    brokerAccount: BrokerAccountEntity | null
}

const BrokerAccountPage: React.FC<Props> = () => {
    const { t } = useTranslation();
    const securitiesRef = useRef<BrokerAccountSecuritiesListRef>(null);

    const { brokerAccountId } = useParams(); // Получаем текущий таб из URL

    if (!brokerAccountId) {
        return <Fragment/>
    }

    const [state, setState] = useState<State>({ brokerAccount: null })
   

    useEffect(() => {
        const initData = async () => {
            const brokerAccount = await getBrokerAccountById(brokerAccountId);
            if (!brokerAccount) {
                return;
            }

            setState((currentState) => {
                return {...currentState, brokerAccount}
            })
        }
        initData();
    }, []);

    const onDataReloaded = async () => {
        await securitiesRef.current?.reloadData();
    }

    const initialValue = state.brokerAccount?.initialValue ?? 0;
    const currentValue = state.brokerAccount?.currentValue ?? 0

    const currentValueLabel = state.brokerAccount ?
        formatMoneyByCurrencyCulture(currentValue, state.brokerAccount?.currency.name):
        "";

    const {profitAndLoss, profitAndLossPercentage, color} = calculateDiff(currentValue, initialValue);

    return (<Fragment>
        <Stack gapX={2} direction={"row"} color="text_primary">
            <Text fontSize="3xl" fontWeight={900}> {state.brokerAccount?.name}: </Text>
            <Text fontSize="3xl" fontWeight={900}>
                {currentValueLabel} (<Span color={color}>{profitAndLoss.toFixed(2)} | {profitAndLossPercentage.toFixed(2)}%</Span>)
            </Text>
        </Stack>
        <BrokerAccountSecuritiesList ref={securitiesRef} brokerAccountId={brokerAccountId}/>
        <SecurityTransactionsList onDataReloaded={onDataReloaded} brokerAccountId={brokerAccountId}/>
    </Fragment>
    )
}


export default BrokerAccountPage;