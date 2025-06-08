import React, { Fragment, useRef } from 'react';
import { SimpleGrid } from '@chakra-ui/react/grid';
import { Flex } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';
import Security from '../Security/Security';
import { SecurityEntity } from '../../../../models/securities/SecurityEntity';
import ShowModalButton from '../../../../shared/components/ShowModalButton/ShowModalButton';
import { BaseModalRef } from '../../../../shared/utilities/modalUtilities';
import SecurityModal from '../../modals/SecurityModal/SecurityModal';
import { useSecurities } from '../../hooks/useSecurities';

interface Props {}

const SecuritiesList: React.FC<Props> = (props) => {
	const { t } = useTranslation()

	const {
		securities,
		createSecurityEntity,
		updateSecurityEntity,
		deleteSecurityEntity,
		reloadSecurities
	} = useSecurities();

	const onReloadSecurities = async () => {
		await reloadSecurities();
	}

	const modalRef = useRef<BaseModalRef>(null);
	
	const onAdd = () => {
		modalRef.current?.openModal()
	};

	return (
		<Fragment>
			<Flex justifyContent="space-between" alignItems="center" pt={5} pb={5}>
				<ShowModalButton buttonTitle={t("security_page_summary_add")} onClick={onAdd}>
					<SecurityModal modalRef={modalRef} onSaved={createSecurityEntity}/>
				</ShowModalButton>
			</Flex>
			<SimpleGrid pt={5} pb={5} gap={4} templateColumns='repeat(auto-fill, minmax(300px, 3fr))'>
				{
					securities.map((security: SecurityEntity) => 
						<Security key={security.id} security={security} 
							onEditCallback={updateSecurityEntity} 
							onDeleteCallback={deleteSecurityEntity}
							onReloadSecurities={onReloadSecurities}/>)
				}
			</SimpleGrid>
		</Fragment>
	);
}

export default SecuritiesList;