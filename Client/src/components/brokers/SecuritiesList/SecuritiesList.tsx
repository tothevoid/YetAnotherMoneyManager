import React, { Fragment, useEffect, useState } from 'react';
import { SimpleGrid } from '@chakra-ui/react/grid';
import { Flex } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';
import Security from '../Security/Security';
import { SecurityEntity } from '../../../models/securities/SecurityEntity';
import AddSecurityButton from '../AddSecurityButton/AddSecurityButton';
import { getSecurities } from '../../../api/securities/securityApi';

interface Props {}

interface State {
    securities: SecurityEntity[]
}

const SecuritiesList: React.FC<Props> = (props) => {
    const { t } = useTranslation()

    const [state, setState] = useState<State>({ securities: [] })

    useEffect(() => {
        const initData = async () => {
            await requestSecuritiesData();
        }
        initData();
    }, []);

    const requestSecuritiesData = async () => {
        const securities = await getSecurities();
        setState((currentState) => {
            return {...currentState, securities}
        })
    };

    const onSecurityCreated = async (addedSecurity: SecurityEntity) => {
        if (!addedSecurity) {
            return
        }

        await onReloadSecurities();
    };

    const onSecurityUpdated = async (updatedSecurity: SecurityEntity) => {
        if (!updatedSecurity) {
            return
        }

        await onReloadSecurities();
    };

    const onSecurityDeleted = async (deleteSecurity: SecurityEntity) => {
        if (!deleteSecurity) {
            return;
        }

        await onReloadSecurities();
    };

    const onReloadSecurities = async () => {
        await requestSecuritiesData();
    }

    return (
        <Fragment>
            <Flex justifyContent="space-between" alignItems="center" pt={5} pb={5}>
                <AddSecurityButton onAdded={onSecurityCreated}></AddSecurityButton>
            </Flex>
            <SimpleGrid pt={5} pb={5} gap={4} templateColumns='repeat(auto-fill, minmax(400px, 3fr))'>
                {
                state.securities.map((security: SecurityEntity) => {
                    return <Security key={security.id} security={security} 
                        onEditCallback={onSecurityUpdated} 
                        onDeleteCallback={onSecurityDeleted}
                        onReloadSecurities={onReloadSecurities}/>
                })
                }
            </SimpleGrid>
        </Fragment>
    );
}

export default SecuritiesList;