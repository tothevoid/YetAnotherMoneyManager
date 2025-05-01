import React, { Fragment, useEffect, useState } from 'react';
import { SimpleGrid } from '@chakra-ui/react/grid';
import { Flex } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';
import { SecurityTransactionEntity } from '../../../models/securities/SecurityTransactionEntity';
import { getSecurityTransactionsByBrokerAccount } from '../../../api/securities/securityTransactionApi';
import AddSecurityTransactionButton from '../AddSecurityTransactionButton/AddSecurityTransactionButton';
import SecurityTransaction from '../SecurityTransaction/SecurityTransaction';

interface Props {
    brokerAccountId: string
}

interface State {
    securities: SecurityTransactionEntity[]
}

const SecurityTransactionsList: React.FC<Props> = (props) => {
    const { t } = useTranslation()

    const [state, setState] = useState<State>({ securities: [] })

    useEffect(() => {
        const initData = async () => {
            await requestSecurityTransactions();
        }
        initData();
    }, []);

    const requestSecurityTransactions = async () => {
        const securityTransactions = await getSecurityTransactionsByBrokerAccount(props.brokerAccountId);
        setState((currentState) => {
            return {...currentState, securities: securityTransactions}
        })
    };

    const onSecurityTransactionCreated = async (addedSecurityTransaction: SecurityTransactionEntity) => {
        if (!addedSecurityTransaction) {
            return
        }

        await onReloadSecurityTransactions();
    };

    const onSecurityTransactionUpdated = async (updatedSecurityTransaction: SecurityTransactionEntity) => {
        if (!updatedSecurityTransaction) {
            return
        }

        await onReloadSecurityTransactions();
    };

    const onSecurityTransactionDeleted = async (deletedSecurity: SecurityTransactionEntity) => {
        if (!deletedSecurity) {
            return;
        }

        await onReloadSecurityTransactions();
    };

    const onReloadSecurityTransactions = async () => {
        await requestSecurityTransactions();
    }

    
    return (
        <Fragment>
            <Flex justifyContent="space-between" alignItems="center" pt={5} pb={5}>
                <AddSecurityTransactionButton onAdded={onSecurityTransactionCreated}></AddSecurityTransactionButton>
            </Flex>
            <div>
            {
                state.securities.map((security: SecurityTransactionEntity) => {
                    return <SecurityTransaction key={security.id} securityTransaction={security} 
                        onEditCallback={onSecurityTransactionUpdated} 
                        onDeleteCallback={onSecurityTransactionDeleted}
                        onReloadSecurityTransactions={onReloadSecurityTransactions}/>
                })
                }
            </div>

            
        </Fragment>
    );
}

export default SecurityTransactionsList;