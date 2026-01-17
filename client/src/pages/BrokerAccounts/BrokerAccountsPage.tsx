import { Fragment } from "react/jsx-runtime";
import BrokerAccountHeader from "../BrokerAccount/components/BrokerAccountHeader/BrokerAccountHeader";
import BrokerAccountSecuritiesList, { BrokerAccountSecuritiesListRef } from "../BrokerAccount/components/BrokerAccountSecuritiesList/BrokerAccountSecuritiesList";
import BrokerAccountTabs, { ChangeAction } from "../BrokerAccount/components/BrokerAccountTabs/BrokerAccountTabs";
import BrokerAccountValuesSummary from "../BrokerAccount/components/BrokerAccountValuesSummary/BrokerAccountValuesSummary";
import { useUserProfile } from "../../../features/UserProfileSettingsModal/hooks/UserProfileContext";
import { getLastPullDate, pullBrokerAccountQuotations } from "../../api/brokers/brokerAccountSecurityApi";
import { useSignalR } from "../../shared/hooks/SignalRHook";
import { useCallback, useEffect, useRef, useState } from "react";
import { getBrokerAccounts } from "../../api/brokers/brokerAccountApi";
import { useTranslation } from "react-i18next";
import { getEarningsByBrokerAccount } from "../../api/brokers/dividendPaymentApi";
import { getBrokerAccountTaxDeductions } from "../../api/brokers/BrokerAccountTaxDeductionApi";

interface State {
    isReloading: boolean
}

interface BrokerAccountsSummary {
    totalInitialValue: number,
    totalCurrentValue: number
    totalDividendIncomes: number,
    totalTaxDeductionIncomes: number
}

const BrokerAccountsPage: React.FC = () => {
    const { t } = useTranslation();
    const securitiesRef = useRef<BrokerAccountSecuritiesListRef>(null);
    const { user } = useUserProfile();
    // TODO: Currency can be different for each account
    const currencyName = user?.currency.name ?? "USD";

    const [state, setState] = useState<State>({ isReloading: false });

    const [mainCurrencyAmount, setMainCurrencyAmount] = useState<number>(0);

    const [lastPullDate, setLastPullDate] = useState<Date | null>(null);
    const [brokerAccountsSummary, setBrokerAccountsSummary] = useState<BrokerAccountsSummary>({
        totalInitialValue: 0,
        totalCurrentValue: 0,
        totalDividendIncomes: 0,
        totalTaxDeductionIncomes: 0
    });
    
    const fetchBrokerAccountsSummary = useCallback(async () => {
        const accounts = await getBrokerAccounts();

        const taxDeductions = (await getBrokerAccountTaxDeductions({brokerAccountId: null}))
            .reduce((state, tax) => state + tax.amount, 0);
        let currencyAmount = 0;

        let totalDividends = 0;
        for (const account of accounts) {
            const dividendIncomes = await getEarningsByBrokerAccount(account.id);
            totalDividends += dividendIncomes;
        }

        const summary = accounts.reduce((state, account) => {
            state.totalInitialValue += account.initialValue;
            state.totalCurrentValue += account.currentValue;
            currencyAmount += account.mainCurrencyAmount;
            return state;
        }, {
            totalInitialValue: 0,
            totalCurrentValue: 0,
            totalDividendIncomes: totalDividends,
            totalTaxDeductionIncomes: taxDeductions
        } as BrokerAccountsSummary)

        setMainCurrencyAmount(currencyAmount);
        setBrokerAccountsSummary(summary);
    }, []);
    
    const onTransactionsChanged = useCallback(async () => {
        await fetchBrokerAccountsSummary();
        await securitiesRef.current?.reloadData();
    }, []);

    const onQuotesRecalculated = async (message: string) => {
        const data = JSON.parse(message);
        
        if (data && data.date) {
            const newDate = new Date(data.date);
            setLastPullDate(newDate);
        }

        await onTransactionsChanged();
    }

    const fetchLastPullDate = async () => {
        const lastPullDate = await getLastPullDate();
        if (lastPullDate) {
            setLastPullDate(lastPullDate);
        }
    }

    useEffect(() => {
        const getData = async () => {
            await fetchBrokerAccountsSummary();
            await fetchLastPullDate();
        }

        getData();
    }, []);

    const pullQuotations = async () => {
        setState((currentState) => {
            return {...currentState, isReloading: true}
        })
        pullBrokerAccountQuotations();
    }
    
    useSignalR(onQuotesRecalculated);

    const onActionTriggered = async (action: ChangeAction) => {
        switch (action) {
            case ChangeAction.TransactionsChanged:
                await onTransactionsChanged();
                break;
            case ChangeAction.FundTransfersChanged:
            case ChangeAction.DividendsChanged:
            case ChangeAction.TaxDeductionsChanged:
                await fetchBrokerAccountsSummary();
                break;
        }
    }

    return <Fragment>
        <BrokerAccountHeader 
            name={t('all_broker_accounts_header')} 
            currencyName={currencyName} 
            currentValue={brokerAccountsSummary.totalCurrentValue} 
            onPullQuotations={pullQuotations} 
            lastPullDate={lastPullDate} 
            isReloading={state.isReloading}/>
        <BrokerAccountValuesSummary 
            initialValue={brokerAccountsSummary.totalInitialValue} 
            currentValue={brokerAccountsSummary.totalCurrentValue} 
            dividendIncomes={brokerAccountsSummary.totalDividendIncomes} 
            taxDeductionIncomes={brokerAccountsSummary.totalTaxDeductionIncomes} 
            currencyName={currencyName}/>
        <BrokerAccountSecuritiesList 
            ref={securitiesRef}
            mainCurrencyAmount={mainCurrencyAmount}
            mainCurrencyName={currencyName}/>
        <BrokerAccountTabs 
            currencyName={currencyName} 
            onActionTriggered={onActionTriggered}/>
    </Fragment>
}

export default BrokerAccountsPage;