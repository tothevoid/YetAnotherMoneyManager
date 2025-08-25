import React, { useEffect, useState } from 'react';
import { Box, Flex } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';
import { useDividendPayments } from '../../hooks/useDividendPayments';
import { DividendPaymentEntity } from '../../../../models/brokers/DividendPaymentEntity';
import DividendPayment from '../DividendPayment/DividendPayment';
import DividendPaymentModal, { CreateDividendPaymentContext, EditDividendPaymentContext } from '../../modals/DividendPaymentModal/DividendPaymentModal';
import { ConfirmModal } from '../../../../shared/modals/ConfirmModal/ConfirmModal';
import { useEntityModal } from '../../../../shared/hooks/useEntityModal';
import AddButton from '../../../../shared/components/AddButton/AddButton';
import { ActiveEntityMode } from '../../../../shared/enums/activeEntityMode';
import { Nullable } from '../../../../shared/utilities/nullable';

interface Props {
	brokerAccountId: string,
	onDividendsChanged: () => void
}

const DividendPaymentsList: React.FC<Props> = (props) => {
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
	} = useEntityModal<DividendPaymentEntity>();

	const {
		dividendPayments,
		createDividendPaymentEntity,
		updateDividendPaymentEntity,
		deleteDividendPaymentEntity
	} = useDividendPayments({ brokerAccountId: props.brokerAccountId }, props.onDividendsChanged);

	const [context, setContext] = useState<Nullable<CreateDividendPaymentContext | EditDividendPaymentContext>>(null);

	useEffect(() => {
		if (!props.brokerAccountId && !activeEntity) {
			setContext(null);
		}
		const context: CreateDividendPaymentContext | EditDividendPaymentContext = activeEntity ?
			{ dividendPayment: activeEntity } as EditDividendPaymentContext:
			{ brokerAccountId: props.brokerAccountId } as CreateDividendPaymentContext;
		setContext(context);
	}, [props.brokerAccountId, activeEntity]);

	const dividendPaymentSaved = (dividendPayment: DividendPaymentEntity) => {
		if (mode === ActiveEntityMode.Add) {
			createDividendPaymentEntity(dividendPayment);
		} else {
			updateDividendPaymentEntity(dividendPayment);
		}
		onActionEnded();
	}

	const onDeleteConfirmed = async () => {
		if (!activeEntity) {
			throw new Error("Deleted entity is not set")
		}

		await deleteDividendPaymentEntity(activeEntity);
		onActionEnded();
	}

	return <Box>
		<Flex alignItems="center" gapX={5}>
			<AddButton buttonTitle={t("broker_account_page_add_dividend_payment_button")} onClick={onAddClicked}/>
		</Flex>
		<Box>
		{
			dividendPayments.map((dividendPayment: DividendPaymentEntity) => 
				<DividendPayment key={dividendPayment.id}
					dividendPayment={dividendPayment} 
					onEditClicked={onEditClicked} 
					onDeleteClicked={onDeleteClicked}/>)
		}
		</Box>
		<ConfirmModal onConfirmed={onDeleteConfirmed}
			title={t("entity_securities_transaction_delete_title")}
			message={t("modals_delete_message")}
			confirmActionName={t("modals_delete_button")}
			ref={confirmModalRef}/>
		{context && <DividendPaymentModal context={context!} modalRef={modalRef} onSaved={dividendPaymentSaved}/>}
	</Box>
}

export default DividendPaymentsList;