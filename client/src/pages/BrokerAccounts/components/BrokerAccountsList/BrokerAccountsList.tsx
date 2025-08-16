import React, { Fragment, useRef, } from 'react';
import { SimpleGrid } from '@chakra-ui/react/grid';
import { Flex } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';
import BrokerAccount from '../BrokerAccount/BrokerAccount';
import { BrokerAccountEntity } from '../../../../models/brokers/BrokerAccountEntity';
import ShowModalButton from '../../../../shared/components/ShowModalButton/ShowModalButton';
import { BaseModalRef } from '../../../../shared/utilities/modalUtilities';
import BrokerAccountModal from '../../modals/BrokerAccountModal/BrokerAccountModal';
import { useBrokerAccounts } from '../../hooks/useBrokerAccounts';

const BrokerAccountsList: React.FC = () => {
	const { t } = useTranslation()

	const {
		brokerAccounts,
		createBrokerAccountEntity,
		updateBrokerAccountEntity,
		deleteBrokerAccountEntity,
		reloadBrokerAccounts
	} = useBrokerAccounts();

	const modalRef = useRef<BaseModalRef>(null);
	
	const onAdd = () => {
		modalRef.current?.openModal()
	};

	return (
		<Fragment>
			<Flex justifyContent="space-between" alignItems="center" pt={5} pb={5}>
				<ShowModalButton buttonTitle={t("broker_accounts_page_summary_add")} onClick={onAdd}>
					<BrokerAccountModal modalRef={modalRef} onSaved={createBrokerAccountEntity}/>
				</ShowModalButton>
			</Flex>
			<SimpleGrid pt={5} pb={5} gap={4} templateColumns='repeat(auto-fill, minmax(400px, 3fr))'>
				{
					brokerAccounts.map((brokerAccount: BrokerAccountEntity) => 
						<BrokerAccount onReloadBrokerAccounts={reloadBrokerAccounts} brokerAccount={brokerAccount} onEditCallback={updateBrokerAccountEntity} 
							onDeleteCallback={deleteBrokerAccountEntity} key={brokerAccount.id}/>)
				}
			</SimpleGrid>
		</Fragment>
	);
}

export default BrokerAccountsList;