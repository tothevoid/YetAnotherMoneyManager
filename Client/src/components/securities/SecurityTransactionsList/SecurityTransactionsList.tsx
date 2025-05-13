import React, { Fragment, useState } from 'react';
import { Flex } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';
import { SecurityTransactionEntity } from '../../../models/securities/SecurityTransactionEntity';
import AddSecurityTransactionButton from '../AddSecurityTransactionButton/AddSecurityTransactionButton';
import SecurityTransaction from '../SecurityTransaction/SecurityTransaction';
import SecurityTransactionsPagination from '../SecurityTransactionsPagination/SecurityTransactionsPagination';
import { getSecurityTransactions } from '../../../api/securities/securityTransactionApi';

interface Props {
    brokerAccountId: string,
    onDataReloaded: () => void
}

interface State {
    currentPage: number,
    pageSize: number,
    transactions: SecurityTransactionEntity[]
}

const SecurityTransactionsList: React.FC<Props> = (props) => {
    const { t } = useTranslation()

    const [state, setState] = useState<State>({ transactions: [], currentPage: 1, pageSize: -1 })

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
        const transactions = await requestTransactions(state.currentPage, state.pageSize)
        setState((currentState) => {
            return {...currentState, transactions}
        })
        props.onDataReloaded();
    }

    const requestTransactions = async (currentPage: number, pageSize: number) => {
        return await getSecurityTransactions({
            brokerAccountId: props.brokerAccountId,
            recordsQuantity: pageSize,
            pageIndex: currentPage,
        });
    }

    const onPageChanged = async (pageSize: number, currentPage: number) => {
        const transactions = await requestTransactions(currentPage, pageSize);
        setState((currentState) => {
            return {...currentState, currentPage, pageSize, transactions}
        })
    }
    
    return (
        <Fragment>
            <Flex alignItems="center" gapX={5}>
                <AddSecurityTransactionButton brokerAccountId={props.brokerAccountId} onAdded={onSecurityTransactionCreated}/>
            </Flex>
           
            <div>
            {
                state.transactions.map((security: SecurityTransactionEntity) => {
                    return <SecurityTransaction key={security.id} securityTransaction={security} 
                        onEditCallback={onSecurityTransactionUpdated} 
                        onDeleteCallback={onSecurityTransactionDeleted}
                        onReloadSecurityTransactions={onReloadSecurityTransactions}/>
                })
                }
            </div>
            <Flex justifyContent={"center"}>
                <SecurityTransactionsPagination brokerAccountId={props.brokerAccountId} onPageChanged={onPageChanged}/>
            </Flex>
        </Fragment>
    );
}

export default SecurityTransactionsList;