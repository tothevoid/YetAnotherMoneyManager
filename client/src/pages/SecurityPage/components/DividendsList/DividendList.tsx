import React, { Fragment, useEffect, useState } from 'react';
import { Box } from '@chakra-ui/react';
import Dividend from '../Dividend/Dividend';
import { useTranslation } from 'react-i18next';
import { DividendEntity } from '../../../../models/securities/DividendEntity';
import DividendModal, { CreateDividendContext, EditDividendContext } from '../../modals/DividendModal/DividendModal';
import { useDividends } from '../../hooks/useDividends';
import { ConfirmModal } from '../../../../shared/modals/ConfirmModal/ConfirmModal';
import { useEntityModal } from '../../../../shared/hooks/useEntityModal';
import { ActiveEntityMode } from '../../../../shared/enums/activeEntityMode';
import AddButton from '../../../../shared/components/AddButton/AddButton';
import { Nullable } from '../../../../shared/utilities/nullable';

interface Props {
	securityId: string
}

const DividendList: React.FC<Props> = (props) => {
	const { 
		activeEntity,
		modalRef,
		confirmModalRef,
		onAddClicked,
		onEditClicked,
		onDeleteClicked,
		mode,
		onActionEnded
	} = useEntityModal<DividendEntity>();

	const {
		dividends,
		createDividendEntity,
		updateDividendEntity,
		deleteDividendEntity
	} = useDividends({securityId: props.securityId});

	const { t } = useTranslation();
	const [context, setContext] = useState<Nullable<CreateDividendContext | EditDividendContext>>(null);

	useEffect(() => {
		const context = activeEntity ?
			{ dividend: activeEntity } as EditDividendContext:
			{ securityId: props.securityId } as CreateDividendContext

		setContext(context);
	}, [activeEntity, props.securityId ]);

	const onDividendSaved = async (dividend: DividendEntity) => {
		if (mode === ActiveEntityMode.Add) {
			await createDividendEntity(dividend);
		} else {
			await updateDividendEntity(dividend);
		}

		onActionEnded();
	}

	const onDeleteConfirmed = async () => {
		if (!activeEntity) {
			throw new Error("Deleted entity is not set")
		}

		await deleteDividendEntity(activeEntity);
		onActionEnded();
	}

	return (
		<Fragment>
			<AddButton buttonTitle={t("security_page_summary_add")} onClick={onAddClicked}/>
			<Box>
				{
					dividends.map((security: DividendEntity) => 
						<Dividend key={security.id} dividend={security} 
							onEditClicked={onEditClicked} 
							onDeleteClicked={onDeleteClicked}/>)
				}
			</Box>
			<ConfirmModal onConfirmed={onDeleteConfirmed}
				title={t("transaction_delete_title")}
				message={t("modals_delete_message")}
				confirmActionName={t("modals_delete_button")}
				ref={confirmModalRef}/>
			{context && <DividendModal context={context} modalRef={modalRef} onSaved={onDividendSaved}/>}
		</Fragment>
	);
}

export default DividendList;