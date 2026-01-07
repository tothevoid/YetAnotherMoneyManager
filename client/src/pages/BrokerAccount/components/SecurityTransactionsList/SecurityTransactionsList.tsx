import React, { useCallback, useEffect, useState } from 'react';
import { Box, Flex } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';
import { SecurityTransactionEntity } from '../../../../models/securities/SecurityTransactionEntity';
import SecurityTransaction from '../SecurityTransaction/SecurityTransaction';
import SecurityTransactionModal, { CreateSecurityTransactionContext, EditSecurityTransactionContext } from '../../modals/SecurityTransactionModal/SecurityTransactionModal';
import { useSecurityTransactions } from '../../hooks/useSecurityTransactions';
import { useEntityModal } from '../../../../shared/hooks/useEntityModal';
import { ConfirmModal } from '../../../../shared/modals/ConfirmModal/ConfirmModal';
import { ActiveEntityMode } from '../../../../shared/enums/activeEntityMode';
import AddButton from '../../../../shared/components/AddButton/AddButton';
import { Nullable } from '../../../../shared/utilities/nullable';
import CollectionPagination from '../../../../shared/components/CollectionPagination/CollectionPagination';
import { getSecurityTransactionsPagination } from '../../../../api/securities/securityTransactionApi';

interface Props {
	brokerAccountId: Nullable<string>,
	onTransactionsChanged: () => void
}

const SecurityTransactionsList: React.FC<Props> = (props) => {
	const { t } = useTranslation()

	const { 
		activeEntity,
		modalRef,
		confirmModalRef,
		onAddClicked,
		onEditClicked,
		onDeleteClicked,
		mode,
		onActionEnded
	} = useEntityModal<SecurityTransactionEntity>();

	const {
		securityTransactions,
		createSecurityTransactionEntity,
		updatedSecurityTransactionEntity,
		deleteSecurityTransactionEntity,
		securityTransactionsQueryParameters, 
		setSecurityTransactionsQueryParameters
	} = useSecurityTransactions({pageIndex: 1, recordsQuantity: -1, brokerAccountId: props.brokerAccountId });

	useEffect(() => {
		props.onTransactionsChanged();
	}, [securityTransactions])

	const [context, setContext] = useState<Nullable<CreateSecurityTransactionContext | EditSecurityTransactionContext>>(null);

	const onPageChanged = async (recordsQuantity: number, pageIndex: number) => {
		setSecurityTransactionsQueryParameters({recordsQuantity, pageIndex, brokerAccountId: securityTransactionsQueryParameters.brokerAccountId});
	}

	useEffect(() => {
		const context = activeEntity ?
			{ securityTransaction: activeEntity } as EditSecurityTransactionContext:
			{ brokerAccountId: props.brokerAccountId } as CreateSecurityTransactionContext;
		setContext(context);
	}, [props.brokerAccountId, activeEntity]);

	const onSecurityTransactionSaved = async (securityTransaction: SecurityTransactionEntity) => {
		if (mode === ActiveEntityMode.Add) {
			await createSecurityTransactionEntity(securityTransaction);
		} else if (mode === ActiveEntityMode.Edit) {
			await updatedSecurityTransactionEntity(securityTransaction);
		}
		onActionEnded();
	}

	const onDeleteConfirmed = async () => {
		if (!activeEntity) {
			throw new Error("Deleted entity is not set")
		}

		await deleteSecurityTransactionEntity(activeEntity);
		onActionEnded();
	}

	const getPagination = useCallback(() => {
		return getSecurityTransactionsPagination(props.brokerAccountId);
	}, [props.brokerAccountId]);

	return <Box>
		<Flex alignItems="center" gapX={5}>
			<AddButton buttonTitle={t("entity_securities_transaction_page_summary_add")} onClick={onAddClicked}/>
		</Flex>
		<Box>
		{
			securityTransactions.map((security: SecurityTransactionEntity) => 
				<SecurityTransaction key={security.id} securityTransaction={security} 
					onEditClicked={onEditClicked} 
					onDeleteClicked={onDeleteClicked}/>)
		}
		</Box>
		<CollectionPagination getPaginationConfig={getPagination} onPageChanged={onPageChanged}/>
		<ConfirmModal onConfirmed={onDeleteConfirmed}
            title={t("entity_securities_transaction_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}/>
        {context && <SecurityTransactionModal context={context} modalRef={modalRef} onSaved={onSecurityTransactionSaved}/>}
	</Box>
}

export default SecurityTransactionsList;