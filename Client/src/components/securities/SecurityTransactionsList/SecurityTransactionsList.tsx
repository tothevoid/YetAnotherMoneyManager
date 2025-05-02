import React, { Fragment, useState } from 'react';
import { Flex } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';
import { SecurityTransactionEntity } from '../../../models/securities/SecurityTransactionEntity';
import AddSecurityTransactionButton from '../AddSecurityTransactionButton/AddSecurityTransactionButton';
import SecurityTransaction from '../SecurityTransaction/SecurityTransaction';
import SecurityTransactionsPagination from '../SecurityTransactionsPagination/SecurityTransactionsPagination';
import { getSecurityTransactions } from '../../../api/securities/securityTransactionApi';

interface Props {
    brokerAccountId: string
}

interface State {
    transactions: SecurityTransactionEntity[]
}

const SecurityTransactionsList: React.FC<Props> = (props) => {
    const { t } = useTranslation()

    const [state, setState] = useState<State>({ transactions: [] })

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
        // await requestSecurityTransactions();
    }

    const onPageChanged = async (pageSize: number, currentPage: number) => {
        const transactions = await getSecurityTransactions({
            brokerAccountId: props.brokerAccountId,
            recordsQuantity: pageSize,
            pageIndex: currentPage
        });
        setState((currentState) => {
            return {...currentState, transactions}
        })
    }
    
    return (
        <Fragment>
            <Flex justifyContent="space-between" alignItems="center" pt={5} pb={5}>
                <AddSecurityTransactionButton onAdded={onSecurityTransactionCreated}></AddSecurityTransactionButton>
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