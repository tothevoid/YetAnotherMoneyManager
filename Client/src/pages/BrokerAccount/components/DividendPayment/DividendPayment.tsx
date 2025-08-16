import { Card, Flex, Span, Stack, Text, CardBody, Button, Icon } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';
import { formatMoneyByCurrencyCulture } from '../../../../shared/utilities/formatters/moneyFormatter';
import { MdEdit, MdDelete } from 'react-icons/md';
import { DividendPaymentEntity } from '../../../../models/brokers/DividendPaymentEntity';
import { ConfirmModal } from '../../../../shared/modals/ConfirmModal/ConfirmModal';
import DividendPaymentModal, { EditDividendPaymentContext } from '../../modals/DividendPaymentModal/DividendPaymentModal';
import { useRef } from 'react';
import { BaseModalRef } from '../../../../shared/utilities/modalUtilities';

type Props = {
	dividendPayment: DividendPaymentEntity,
	onDeleteCallback: (dividendPayment: DividendPaymentEntity) => void,
	onEditCallback: (dividendPayment: DividendPaymentEntity) => void
}

const DividendPayment = (props: Props) => {
	const { dividend, securitiesQuantity, tax} = props.dividendPayment;

	const { t } = useTranslation();

	const confirmModalRef = useRef<BaseModalRef>(null);
	const editModalRef = useRef<BaseModalRef>(null);
	
	const onEditClicked = () => {
		editModalRef.current?.openModal()
	};

	const onDeleteClicked = () => {
		confirmModalRef.current?.openModal()
	};

    const paymentWithoutTax = securitiesQuantity * dividend.amount - tax;

    const context: EditDividendPaymentContext = {
        dividendPayment: props.dividendPayment
    }

	return <Card.Root borderColor="border_primary" color="text_primary" backgroundColor="background_primary" 
        mt={5} mb={5} boxShadow={"sm"} _hover={{ boxShadow: "md" }}>
        <CardBody>
            <Flex justifyContent="space-between" alignItems="center">
                <Stack direction={'row'} alignItems="center">
                    {/* <Text textAlign={'center'} w={150} rounded={10} padding={1} background={'purple.600'}>{formatDateTime(date, i18n, false)}</Text> */}
                    <Text fontWeight={700}>{dividend.security?.name} ({dividend.security?.ticker})</Text>
                </Stack>
                <Flex gap={2} justifyContent="space-between" alignItems="center">
                    <Text>
                        <Span>{formatMoneyByCurrencyCulture(paymentWithoutTax, dividend.security.currency.name)} </Span>
                        <Span pl={2.5} pr={2.5}>({formatMoneyByCurrencyCulture(dividend.amount, dividend.security.currency.name)} x {securitiesQuantity})</Span>
                    </Text>
                    <Button background={'background_secondary'} size={'sm'} onClick={onEditClicked}>
                        <Icon color="card_action_icon_primary">
                            <MdEdit/>
                        </Icon>
                    </Button>
                    <Button background={'background_secondary'} size={'sm'} onClick={onDeleteClicked}>
                        <Icon color="card_action_icon_danger">
                            <MdDelete/>
                        </Icon>
                    </Button>
                </Flex>
            </Flex>
        </CardBody>	
        <ConfirmModal onConfirmed={() => props.onDeleteCallback(props.dividendPayment)}
            title={t("entity_securities_transaction_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}/>
        <DividendPaymentModal context={context} modalRef={editModalRef} onSaved={props.onEditCallback}/>
    </Card.Root>
};

export default DividendPayment;