import React, { useRef } from 'react';
import { Box, Flex } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';
import { BaseModalRef } from '../../../../shared/utilities/modalUtilities';
import { useDividendPayments } from '../../hooks/useDividendPayments';
import { ClientDividendPaymentEntity } from '../../../../models/brokers/DividendPaymentEntity';
import DividendPayment from '../DividendPayment/DividendPayment';

interface Props {
	brokerAccountId: string,
	onDataReloaded: () => void
}

const DividendPaymentsList: React.FC<Props> = (props) => {
	const { t } = useTranslation()

	const {
		dividendPayments,
		createDividendPaymentEntity,
		updateDividendPaymentEntity,
		deleteDividendPaymentEntity
	} = useDividendPayments({ brokerAccountId: props.brokerAccountId });

	const modalRef = useRef<BaseModalRef>(null);
	
	// const onAdd = () => {
	//	 modalRef.current?.openModal()
	// };

	// const securityTransaction: SecurityTransactionEntity = {
	//	 brokerAccount: {id: props.brokerAccountId}
	// }

	return <Box>
		<Flex alignItems="center" gapX={5}>
			{/* <ShowModalButton buttonTitle={t("entity_securities_transaction_page_summary_add")} onClick={onAdd}>
				<SecurityTransactionModal securityTransaction={securityTransaction} modalRef={modalRef} onSaved={createSecurityTransactionEntity}/>
			</ShowModalButton> */}
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