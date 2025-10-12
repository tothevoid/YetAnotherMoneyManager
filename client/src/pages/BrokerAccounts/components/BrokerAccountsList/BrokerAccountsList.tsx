import React, { Fragment, useRef, } from 'react';
import { SimpleGrid } from '@chakra-ui/react/grid';
import { Flex } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';
import BrokerAccount from '../BrokerAccount/BrokerAccount';
import { BrokerAccountEntity } from '../../../../models/brokers/BrokerAccountEntity';
import BrokerAccountModal from '../../modals/BrokerAccountModal/BrokerAccountModal';
import { useBrokerAccounts } from '../../hooks/useBrokerAccounts';
import Placeholder from '../../../../shared/components/Placeholder/Placeholder';
import { ConfirmModal } from '../../../../shared/modals/ConfirmModal/ConfirmModal';
import { useEntityModal } from '../../../../shared/hooks/useEntityModal';
import AddButton from '../../../../shared/components/AddButton/AddButton';
import { ActiveEntityMode } from '../../../../shared/enums/activeEntityMode';
import { BaseModalRef } from '../../../../shared/utilities/modalUtilities';
import TopUpBrokerAccountModal from '../../modals/TopUpBrokerAccountModal/TopUpBrokerAccountModal';

const BrokerAccountsList: React.FC = () => {
	const { t } = useTranslation()

	const {
		activeEntity,
		modalRef,
		confirmModalRef,
		onAddClicked,
		onEditClicked,
		onDeleteClicked,
		mode,
		onActionEnded,
		setActiveEntity
	} = useEntityModal<BrokerAccountEntity>();

	const topUpModalRef = useRef<BaseModalRef>(null);
	
	const onTopUpClicked = (brokerAccount: BrokerAccountEntity) => {
		setActiveEntity(brokerAccount);
		topUpModalRef.current?.openModal();
	};

	const {
		brokerAccounts,
		createBrokerAccountEntity,
		updateBrokerAccountEntity,
		deleteBrokerAccountEntity,
		reloadBrokerAccounts
	} = useBrokerAccounts();

	const getAddButton = () => {
		return <AddButton buttonTitle={t("broker_accounts_page_summary_add")} onClick={onAddClicked}/>
	}

	const onBrokerAccountSaved = async (brokerAccount: BrokerAccountEntity) => {
		if (mode === ActiveEntityMode.Add) {
			await createBrokerAccountEntity(brokerAccount);
		} else if (mode === ActiveEntityMode.Edit) {
			await updateBrokerAccountEntity(brokerAccount);
		}
		onActionEnded();
	}

	const onDeleteConfirmed = async () => {
		if (!activeEntity) {
			throw new Error("Deleted entity is not set")
		}

		await deleteBrokerAccountEntity(activeEntity);
		onActionEnded();
	}
	
	const onDeposited = async () => {
		await reloadBrokerAccounts();
		onActionEnded();
	}

	if (!brokerAccounts.length) {
		return <Placeholder text={t("broker_accounts_page_no_accounts")}>
			{getAddButton()}
		</Placeholder>
	}

	return (
		<Fragment>
			<Flex justifyContent="space-between" alignItems="center" pt={5} pb={5}>
				{getAddButton()}
			</Flex>
			<SimpleGrid pt={5} pb={5} gap={4} templateColumns='repeat(auto-fill, minmax(400px, 3fr))'>
				{
					brokerAccounts.map((brokerAccount: BrokerAccountEntity) => 
						<BrokerAccount brokerAccount={brokerAccount}
							onTopUpClick={onTopUpClicked}
							onEditClick={onEditClicked} 
							onDeleteClick={onDeleteClicked} 
							key={brokerAccount.id}/>)
				}
			</SimpleGrid>
			<ConfirmModal onConfirmed={onDeleteConfirmed}
				title={t("broker_account_delete_title")}
				message={t("modals_delete_message")}
				confirmActionName={t("modals_delete_button")}
				ref={confirmModalRef}/>
			<BrokerAccountModal brokerAccount={activeEntity} modalRef={modalRef} onSaved={onBrokerAccountSaved}/>
			<TopUpBrokerAccountModal onDeposited={onDeposited} modalRef={topUpModalRef} brokerAccount={activeEntity}/>
		</Fragment>
	);
}

export default BrokerAccountsList;