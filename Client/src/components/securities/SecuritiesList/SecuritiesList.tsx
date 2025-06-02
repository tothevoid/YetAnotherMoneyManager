import React, { Fragment, useEffect, useRef, useState } from 'react';
import { SimpleGrid } from '@chakra-ui/react/grid';
import { Flex } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';
import Security from '../Security/Security';
import { SecurityEntity } from '../../../models/securities/SecurityEntity';
import { createSecurity, getSecurities } from '../../../api/securities/securityApi';
import ShowModalButton from '../../common/ShowModalButton/ShowModalButton';
import { BaseModalRef } from '../../../common/ModalUtilities';
import SecurityModal from '../modals/SecurityModal/SecurityModal';

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

    const modalRef = useRef<BaseModalRef>(null);
    
    const onAdd = () => {
        modalRef.current?.openModal()
    };

    const onSecurityAdded = async (security: SecurityEntity, icon: File | null) => {
        const createdSecurity = await createSecurity(security, icon);
        if (!createdSecurity) {
            return;
        }

        onSecurityCreated(createdSecurity);
    };

    return (
        <Fragment>
            <Flex justifyContent="space-between" alignItems="center" pt={5} pb={5}>
                <ShowModalButton buttonTitle={t("security_page_summary_add")} onClick={onAdd}>
                    <SecurityModal modalRef={modalRef} onSaved={onSecurityAdded}/>
                </ShowModalButton>
            </Flex>
            <SimpleGrid pt={5} pb={5} gap={4} templateColumns='repeat(auto-fill, minmax(300px, 3fr))'>
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