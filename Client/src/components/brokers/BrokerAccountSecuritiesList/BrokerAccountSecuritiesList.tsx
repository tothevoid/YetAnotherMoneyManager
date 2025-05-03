import { forwardRef, Fragment, useEffect, useImperativeHandle, useState } from 'react';
import { SimpleGrid } from '@chakra-ui/react/grid';
import { useTranslation } from 'react-i18next';
import BrokerAccountSecurity from '../BrokerAccountSecurity/BrokerAccountSecurity';
import { BrokerAccountSecurityEntity } from '../../../models/brokers/BrokerAccountSecurityEntity';
import { getSecuritiesByBrokerAccount } from '../../../api/brokers/brokerAccountSecurityApi';

interface Props {
    brokerAccountId: string
}

interface State {
    brokerAccountSecurities: BrokerAccountSecurityEntity[]
}

export interface BrokerAccountSecuritiesListRef {
    reloadData: () => Promise<void>
}

const BrokerAccountSecuritiesList = forwardRef<BrokerAccountSecuritiesListRef, Props>((props: Props, ref)=> {
    const { t } = useTranslation()

    const [state, setState] = useState<State>({ brokerAccountSecurities: [] })

    useEffect(() => {
        const initData = async () => {
            await requestBrokerAccountSecurityData();
        }
        initData();
    }, []);

    useImperativeHandle(ref, () => ({
        reloadData: requestBrokerAccountSecurityData,
    }));

    const requestBrokerAccountSecurityData = async () => {
        const brokerAccountSecurities = await getSecuritiesByBrokerAccount(props.brokerAccountId);
        setState((currentState) => {
            return {...currentState, brokerAccountSecurities}
        })
    };

    const onBrokerAccountSecurityUpdated = async (updateBrokerAccountSecurity: BrokerAccountSecurityEntity) => {
        if (!updateBrokerAccountSecurity) {
            return
        }

        await onReloadBrokerAccountSecurities();
    };

    const onBrokerAccountSecurityDeleted = async (deletedBrokerAccount: BrokerAccountSecurityEntity) => {
        if (!deletedBrokerAccount) {
            return;
        }

        await onReloadBrokerAccountSecurities();
    };

    const onReloadBrokerAccountSecurities = async () => {
        await requestBrokerAccountSecurityData();
    }

    return (
        <Fragment>
            <SimpleGrid pt={5} pb={5} gap={4} templateColumns='repeat(auto-fill, minmax(400px, 3fr))'>
                {
                state.brokerAccountSecurities.map((brokerAccountSecurity: BrokerAccountSecurityEntity) => {
                    return <BrokerAccountSecurity onReloadBrokerAccounts={onReloadBrokerAccountSecurities} 
                        brokerAccountSecurity={brokerAccountSecurity} onEditCallback={onBrokerAccountSecurityUpdated} 
                        onDeleteCallback={onBrokerAccountSecurityDeleted} key={brokerAccountSecurity.id}/>
                })
                }
            </SimpleGrid>
        </Fragment>
    );
});

export default BrokerAccountSecuritiesList;