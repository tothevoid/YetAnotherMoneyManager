import React, { Fragment, useEffect, useRef, useState } from 'react';
import { Box, Flex, Text } from '@chakra-ui/react';
import { DividendEntity } from '../../../models/securities/DividendEntity';
import { createDividend, getDividends } from '../../../api/securities/dividendApi';
import Dividend from '../Dividend/Dividend';
import AddDividendButton from '../AddDividendButton/AddDividendButton';
import { useTranslation } from 'react-i18next';
import ShowModalButton from '../../common/ShowModalButton/ShowModalButton';
import DividendModal from '../../../modals/DividendModal/DividendModal';
import { BaseModalRef } from '../../../common/ModalUtilities';

interface Props {
    securityId: string
}

interface State {
    dividends: DividendEntity[]
}

const DividendList: React.FC<Props> = (props) => {
    const [state, setState] = useState<State>({ dividends: [] })

    useEffect(() => {
        const initData = async () => {
            await requestDividends();
        }
        initData();
    }, []);

    const requestDividends = async () => {
        const dividends = await getDividends(props.securityId);
        setState((currentState) => {
            return {...currentState, dividends}
        })
    };

    const onDividendAdded = async (addedDividend: DividendEntity) => {
        if (!addedDividend) {
            return
        }

        await onReloadDividends();
    };

    const onDividendUpdated = async (updatedDividend: DividendEntity) => {
        if (!updatedDividend) {
            return
        }

        await onReloadDividends();
    };

    const onDividendDeleted = async (deletedDividend: DividendEntity) => {
        if (!deletedDividend) {
            return;
        }

        await onReloadDividends();
    };

    const onReloadDividends = async () => {
        await requestDividends();
    }

    const modalRef = useRef<BaseModalRef>(null);
    
    const onAdd = () => {
        modalRef.current?.openModal()
    };

    const addDividend = async (dividend: DividendEntity) => {
        const createdDividend = await createDividend(dividend);
        if (!createdDividend) {
            return;
        }

        onDividendAdded(createdDividend);
    };

    const { t } = useTranslation();
    const dividend: DividendEntity = { security: {id: props.securityId} };

    return (
        <Fragment>
            <Text fontSize="2xl">{t("dividends_list_title")}</Text>
            <Flex direction={'column-reverse'} justifyContent="space-between" pt={5} pb={5}>
                <ShowModalButton buttonTitle={t("security_page_summary_add")} onClick={onAdd}>
                    <DividendModal dividend={dividend} modalRef={modalRef} onSaved={addDividend}/>
                </ShowModalButton>
            </Flex>
            <Box>
                {
                state.dividends.map((security: DividendEntity) => {
                    return <Dividend key={security.id} dividend={security} 
                        onEditCallback={onDividendUpdated} 
                        onDeleteCallback={onDividendDeleted}
                        onReloadDividends={onReloadDividends}/>
                })
                }
            </Box>
        </Fragment>
    );
}

export default DividendList;