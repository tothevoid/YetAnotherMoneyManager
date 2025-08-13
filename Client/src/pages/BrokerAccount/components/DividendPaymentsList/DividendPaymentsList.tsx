import React, { useRef } from 'react';
import { Box, Flex } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';
import { BaseModalRef } from '../../../../shared/utilities/modalUtilities';
import { useDividendPayments } from '../../hooks/useDividendPayments';
import { ClientDividendPaymentEntity } from '../../../../models/brokers/DividendPaymentEntity';
import DividendPayment from '../DividendPayment/DividendPayment';
import ShowModalButton from '../../../../shared/components/ShowModalButton/ShowModalButton';
import DividendPaymentModal from '../../modals/DividendPaymentModal/DividendPaymentModal';

interface Props {
	brokerAccountId: string,
	onDividendsChanged: () => void
}

const DividendPaymentsList: React.FC<Props> = (props) => {
	const { t } = useTranslation()

	const {
		dividendPayments,
		createDividendPaymentEntity,
		updateDividendPaymentEntity,
		deleteDividendPaymentEntity
	} = useDividendPayments({ brokerAccountId: props.brokerAccountId }, props.onDividendsChanged);

	const modalRef = useRef<BaseModalRef>(null);
	
	const onAdd = () => {
		 modalRef.current?.openModal()
	};

	const dividendPayment: ClientDividendPaymentEntity = {
		brokerAccount: {id: props.brokerAccountId}
	}

	return <Box>
		<Flex alignItems="center" gapX={5}>
			<ShowModalButton buttonTitle={t("broker_account_page_add_dividend_payment_button")} onClick={onAdd}>
				<DividendPaymentModal dividendPayment={dividendPayment} modalRef={modalRef} onSaved={createDividendPaymentEntity}/>
			</ShowModalButton>
		</Flex>
		<Box>
		{
			dividendPayments.map((dividendPayment: ClientDividendPaymentEntity) => 
				<DividendPayment key={dividendPayment.id} dividendPayment={dividendPayment} 
					onEditCallback={updateDividendPaymentEntity} 
					onDeleteCallback={deleteDividendPaymentEntity}/>)
		}
		</Box>
	</Box>
}

export default DividendPaymentsList;