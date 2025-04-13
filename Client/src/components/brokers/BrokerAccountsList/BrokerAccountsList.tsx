import React, { Fragment, useEffect, useState } from 'react';
import { SimpleGrid } from '@chakra-ui/react/grid';
import { Flex } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';
import { getBrokerAccounts } from '../../../api/brokers/brokerAccountApi';
import BrokerAccount from '../BrokerAccount/BrokerAccount';
import AddBrokerAccountButton from '../AddBrokerAccountButton/AddBrokerAccountButton';
import { BrokerAccountEntity } from '../../../models/brokers/BrokerAccountEntity';

interface Props {
    
}

interface State {
    brokerAccounts: BrokerAccountEntity[]
}

const BrokerAccountsList: React.FC<Props> = (props) => {
    const { t } = useTranslation()

    const [state, setState] = useState<State>({ brokerAccounts: [] })

    useEffect(() => {
        const initData = async () => {
            await requestAccountsData();
        }
        initData();
    }, []);

    const requestAccountsData = async () => {
        const brokerAccounts = await getBrokerAccounts();
        setState((currentState) => {
            return {...currentState, brokerAccounts}
        })
    };

    const onBrokerAccountCreated = async (addedBrokerAccount: BrokerAccountEntity) => {
        if (!addedBrokerAccount) {
            return
        }

        setState((currentState: State) => {
            const accounts = state.brokerAccounts.concat(addedBrokerAccount);
            return {...currentState, accounts};
        });
    };

    const onBrokerAccountUpdated = async (updateBrokerAccount: BrokerAccountEntity) => {
        if (!updateBrokerAccount) {
            return
        }

        setState((currentState: State) => {
            const brokerAccounts = currentState.brokerAccounts.map((brokerAccount: BrokerAccountEntity) => {
                if (brokerAccount.id === updateBrokerAccount.id) {
                    return {...updateBrokerAccount}
                } 
                return brokerAccount
            });
            
            return {...currentState, brokerAccounts}
        });
    };

    const onBrokerAccountDeleted = async (deletedBrokerAccount: BrokerAccountEntity) => {
        if (!deletedBrokerAccount) {
            return;
        }

        setState((currentState: State) => {
            const accounts = currentState.brokerAccounts.filter((brokerAccount: BrokerAccountEntity) => brokerAccount.id !== deletedBrokerAccount.id)
            return { ...currentState, accounts }
        });
    };

    const onReloadBrokerAccounts = async () => {
        await requestAccountsData();
    }

    return (
        <Fragment>
            <Flex justifyContent="space-between" alignItems="center" pt={5} pb={5}>
                <AddBrokerAccountButton onAdded={onBrokerAccountCreated}></AddBrokerAccountButton>
            </Flex>
            <SimpleGrid pt={5} pb={5} gap={4} templateColumns='repeat(auto-fill, minmax(300px, 3fr))'>
                {
                state.brokerAccounts.map((brokerAccount: BrokerAccountEntity) => {
                    return <BrokerAccount onReloadBrokerAccounts={onReloadBrokerAccounts} brokerAccount={brokerAccount} onEditCallback={onBrokerAccountUpdated} 
                        onDeleteCallback={onBrokerAccountDeleted} key={brokerAccount.id}/>
                })
                }
            </SimpleGrid>
        </Fragment>
    );
}

export default BrokerAccountsList;