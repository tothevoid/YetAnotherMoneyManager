import { Fragment, useEffect, useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import BrokerAccountSecuritiesList, { BrokerAccountSecuritiesListRef } from "../../components/brokers/BrokerAccountSecuritiesList/BrokerAccountSecuritiesList";
import { useParams } from "react-router-dom";
import { BrokerAccountEntity } from "../../models/brokers/BrokerAccountEntity";
import { getBrokerAccountById } from "../../api/brokers/brokerAccountApi";
import { Text } from "@chakra-ui/react";
import SecurityTransactionsList from "../../components/securities/SecurityTransactionsList/SecurityTransactionsList";

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

    return (<Fragment>
        <Text fontSize="3xl" fontWeight={900} color="text_primary">{state.brokerAccount?.name}</Text>
        <BrokerAccountSecuritiesList ref={securitiesRef} brokerAccountId={brokerAccountId}/>
        <SecurityTransactionsList onDataReloaded={onDataReloaded} brokerAccountId={brokerAccountId}/>
    </Fragment>
    )
}


export default BrokerAccountPage;