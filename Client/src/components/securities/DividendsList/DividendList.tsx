import React, { Fragment, useEffect, useState } from 'react';
import { Box, Flex, Text } from '@chakra-ui/react';
import { DividendEntity } from '../../../models/securities/DividendEntity';
import { getDividends } from '../../../api/securities/dividendApi';
import Dividend from '../Dividend/Dividend';
import AddDividendButton from '../AddDividendButton/AddDividendButton';
import { useTranslation } from 'react-i18next';

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

    const { t } = useTranslation();
    
    return (
        <Fragment>
            <Text fontSize="2xl">{t("dividends_list_title")}</Text>
            <Flex direction={'column-reverse'} justifyContent="space-between" pt={5} pb={5}>
                <AddDividendButton securityId={props.securityId} onAdded={onDividendAdded}></AddDividendButton>
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