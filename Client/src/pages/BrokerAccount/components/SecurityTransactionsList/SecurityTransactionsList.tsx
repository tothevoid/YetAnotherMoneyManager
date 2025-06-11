import React, { Fragment, useEffect, useRef } from 'react';
import { Box, Flex } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';
import { SecurityTransactionEntity } from '../../../../models/securities/SecurityTransactionEntity';
import SecurityTransaction from '../SecurityTransaction/SecurityTransaction';
import ShowModalButton from '../../../../shared/components/ShowModalButton/ShowModalButton';
import { BaseModalRef } from '../../../../shared/utilities/modalUtilities';
import SecurityTransactionsPagination from '../SecurityTransactionsPagination/SecurityTransactionsPagination';
import SecurityTransactionModal from '../../modals/SecurityTransactionModal/SecurityTransactionModal';
import { useSecurityTransactions } from '../../hooks/useSecurityTransactions';

interface Props {
	brokerAccountId: string,
	onDataReloaded: () => void
}

const SecurityTransactionsList: React.FC<Props> = (props) => {
	const { t } = useTranslation()

	const {
		securityTransactions,
		isSecurityTransactionsLoading,
		createSecurityTransactionEntity,
		updatedSecurityTransactionEntity,
		deleteSecurityTransactionEntity,
		securityTransactionsQueryParameters, 
		setSecurityTransactionsQueryParameters,
		reloadSecurityTransactions
	} = useSecurityTransactions({currentPage: 1, pageSize: -1, brokerAccountId: props.brokerAccountId });

	useEffect(() => {
		props.onDataReloaded();
	}, [securityTransactions])

	const onReloadSecurityTransactions = async () => {
		await reloadSecurityTransactions();
		props.onDataReloaded();
	}

	const onPageChanged = async (pageSize: number, currentPage: number) => {
		setSecurityTransactionsQueryParameters({pageSize, currentPage, brokerAccountId: securityTransactionsQueryParameters.brokerAccountId});
	}

	const modalRef = useRef<BaseModalRef>(null);
	
	const onAdd = () => {
		modalRef.current?.openModal()
	};

	const securityTransaction: SecurityTransactionEntity = {
		brokerAccount: {id: props.brokerAccountId}
	}

	return (
		<Fragment>
			<Flex alignItems="center" gapX={5}>
				<ShowModalButton buttonTitle={t("entity_securities_transaction_page_summary_add")} onClick={onAdd}>
					<SecurityTransactionModal securityTransaction={securityTransaction} modalRef={modalRef} onSaved={createSecurityTransactionEntity}/>
				</ShowModalButton>
			</Flex>
			<Box>
			{
				securityTransactions.map((security: SecurityTransactionEntity) => 
					<SecurityTransaction key={security.id} securityTransaction={security} 
						onEditCallback={updatedSecurityTransactionEntity} 
						onDeleteCallback={deleteSecurityTransactionEntity}
						onReloadSecurityTransactions={onReloadSecurityTransactions}/>)
			}
			</Box>
			<Flex justifyContent={"center"}>
				<SecurityTransactionsPagination brokerAccountId={props.brokerAccountId} onPageChanged={onPageChanged}/>
			</Flex>
		</Fragment>
	);
}

export default SecurityTransactionsList;