import { Fragment, useCallback, useEffect, useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import { useParams } from "react-router-dom";
import { BrokerAccountEntity } from "../../models/brokers/BrokerAccountEntity";
import { getBrokerAccountById } from "../../api/brokers/brokerAccountApi";
import { Stack, Text } from "@chakra-ui/react";
import { formatMoneyByCurrencyCulture } from "../../shared/utilities/formatters/moneyFormatter";
import { calculateDiff } from "../../shared/utilities/numericDiffsUtilities";
import { getLastPullDate, pullBrokerAccountQuotations } from "../../api/brokers/brokerAccountSecurityApi";
import BrokerAccountSecuritiesList, { BrokerAccountSecuritiesListRef } from "./components/BrokerAccountSecuritiesList/BrokerAccountSecuritiesList";
import { useSignalR } from "../../shared/hooks/SignalRHook";
import { getEarningsByBrokerAccount } from "../../api/brokers/dividendPaymentApi";
import { getAmountByBrokerAccount } from "../../api/brokers/BrokerAccountTaxDeductionApi";
import BrokerAccountTabs, { ChangeAction } from "./components/BrokerAccountTabs/BrokerAccountTabs";
import BrokerAccountHeader from "./components/BrokerAccountHeader/BrokerAccountHeader";
import BrokerAccountValuesSummary from "./components/BrokerAccountValuesSummary/BrokerAccountValuesSummary";

interface State {
    brokerAccount: BrokerAccountEntity | null,
    isReloading: boolean
}

const BrokerAccountPage: React.FC = () => {
    const securitiesRef = useRef<BrokerAccountSecuritiesListRef>(null);

    const { brokerAccountId } = useParams();

    const [state, setState] = useState<State>({ brokerAccount: null, isReloading: false });
    
    const [dividendIncomes, setDividendIncomes] = useState<number>(0);

    const [taxDeductionIncomes, setTaxDeductionIncomes] = useState<number>(0);

    const [lastPullDate, setLastPullDate] = useState<Date | null>(null);

    const onQuotesRecalculated = async (message: string) => {
        const data = JSON.parse(message);
        
        if (data && data.date) {
            const newDate = new Date(data.date);
            setLastPullDate(newDate);
        }

        await onTransactionsChanged();
    }

    const fetchBrokerAccount = useCallback(async () => {
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
    }, [brokerAccountId])

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

        const getDividendsEarnings = async () => {
            const dividendIncomes = await getEarningsByBrokerAccount(account.id);
            setDividendIncomes(dividendIncomes);
        }

        const getTaxDeductions = async () => {
            const taxDeductions = await getAmountByBrokerAccount(account.id);
            setTaxDeductionIncomes(taxDeductions);
        }

        getTaxDeductions();
        getDividendsEarnings();
    }, [state.brokerAccount]);

    const onTransactionsChanged = useCallback(async () => {
        await fetchBrokerAccount();
        await securitiesRef.current?.reloadData();
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

    const onActionTriggered = async (action: ChangeAction) => {
        switch (action) {
            case ChangeAction.TransactionsChanged:
                await onTransactionsChanged();
                break;
            case ChangeAction.FundTransfersChanged:
            case ChangeAction.DividendsChanged:
            case ChangeAction.TaxDeductionsChanged:
                await fetchBrokerAccount();
                break;
        }
    }

    if (!state.brokerAccount) {
        return <Fragment/>
    }

    return <Fragment>
        <BrokerAccountHeader name={state.brokerAccount?.name} currencyName={state.brokerAccount?.currency?.name} currentValue={state.brokerAccount?.currentValue} onPullQuotations={pullQuotations} lastPullDate={lastPullDate} isReloading={state.isReloading} />
        <BrokerAccountValuesSummary initialValue={state.brokerAccount?.initialValue ?? 0} currentValue={state.brokerAccount?.currentValue ?? 0} dividendIncomes={dividendIncomes} taxDeductionIncomes={taxDeductionIncomes} currencyName={state.brokerAccount?.currency?.name ?? ""} />
        <BrokerAccountSecuritiesList ref={securitiesRef} brokerAccount={state.brokerAccount}/>
        <BrokerAccountTabs currencyName={state?.brokerAccount?.currency?.name} brokerAccountId={brokerAccountId} onActionTriggered={onActionTriggered}/>
    </Fragment>
}

export default BrokerAccountPage;