import React, { Fragment, useRef } from 'react';
import { Box } from '@chakra-ui/react';
import Dividend from '../Dividend/Dividend';
import { useTranslation } from 'react-i18next';
import { DividendEntity } from '../../../../models/securities/DividendEntity';
import ShowModalButton from '../../../../shared/components/ShowModalButton/ShowModalButton';
import { BaseModalRef } from '../../../../shared/utilities/modalUtilities';
import DividendModal, { CreateDividendContext } from '../../modals/DividendModal/DividendModal';
import { useDividends } from '../../hooks/useDividends';

interface Props {
	securityId: string
}

const DividendList: React.FC<Props> = (props) => {
	const {
		dividends,
		createDividendEntity,
		updateDividendEntity,
		deleteDividendEntity,
		reloadDividends
	} = useDividends({securityId: props.securityId});

	const onReloadDividends = async () => {
		await reloadDividends();
	}

	const modalRef = useRef<BaseModalRef>(null);
	
	const onAdd = () => {
		modalRef.current?.openModal()
	};

	const { t } = useTranslation();
	const dividend: CreateDividendContext = { 
		securityId: props.securityId
	};

	return (
		<Fragment>
			<ShowModalButton buttonTitle={t("security_page_summary_add")} onClick={onAdd}>
				<DividendModal context={dividend} modalRef={modalRef} onSaved={createDividendEntity}/>
			</ShowModalButton>
			<Box>
				{
					dividends.map((security: DividendEntity) => 
						<Dividend key={security.id} dividend={security} 
							onEditCallback={updateDividendEntity} 
							onDeleteCallback={deleteDividendEntity}
							onReloadDividends={onReloadDividends}/>)
				}
			</Box>
		</Fragment>
	);
}

export default DividendList;