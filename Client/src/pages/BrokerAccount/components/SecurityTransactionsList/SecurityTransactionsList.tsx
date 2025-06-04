import React, { Fragment, useRef, useState } from 'react';
import { Flex } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';
import { SecurityTransactionEntity } from '../../../../models/securities/SecurityTransactionEntity';
import SecurityTransaction from '../../../SecurityPage/components/SecurityTransaction/SecurityTransaction';
import { createSecurityTransaction, getSecurityTransactions } from '../../../../api/securities/securityTransactionApi';
import ShowModalButton from '../../../../shared/components/ShowModalButton/ShowModalButton';
import { BaseModalRef } from '../../../../shared/utilities/modalUtilities';
import SecurityTransactionsPagination from '../../../SecuritiesPage/components/SecurityTransactionsPagination/SecurityTransactionsPagination';
import SecurityTransactionModal from '../../modals/SecurityTransactionModal/SecurityTransactionModal';

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

    const modalRef = useRef<BaseModalRef>(null);
    
    const onAdd = () => {
        modalRef.current?.openModal()
    };

    const onSecurityTransactionAdded = async (securityTransaction: SecurityTransactionEntity) => {
        const transaction = await createSecurityTransaction(securityTransaction);
        if (!transaction) {
            return;
        }

        onSecurityTransactionCreated(transaction);
    };

    const securityTransaction: SecurityTransactionEntity = {
        brokerAccount: {id: props.brokerAccountId}
    }

    return (
        <Fragment>
            <Flex alignItems="center" gapX={5}>
                <ShowModalButton buttonTitle={t("entity_securities_transaction_page_summary_add")} onClick={onAdd}>
                    <SecurityTransactionModal securityTransaction={securityTransaction} modalRef={modalRef} onSaved={onSecurityTransactionAdded}/>
                </ShowModalButton>
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