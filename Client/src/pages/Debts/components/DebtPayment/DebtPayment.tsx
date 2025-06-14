import { Button, Card, CardBody, Flex, Icon, Stack, Text } from '@chakra-ui/react';
import { MdDelete, MdEdit } from "react-icons/md";
import { useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { ConfirmModal } from '../../../../shared/modals/ConfirmModal/ConfirmModal';
import { formatMoneyByCurrencyCulture } from '../../../../shared/utilities/formatters/moneyFormatter';
import { formatDate } from '../../../../shared/utilities/formatters/dateFormatter';
import { ClientDebtPaymentEntity } from '../../../../models/debts/DebtPaymentEntity';
import DebtPaymentModal from '../../modals/DebtPaymentModal/DebtPaymentModal';
import { BaseModalRef } from '../../../../shared/utilities/modalUtilities';

type Props = {
    debtPayment: ClientDebtPaymentEntity,
    onEditCallback: (debt: ClientDebtPaymentEntity) => void,
    onDeleteCallback: (debt: ClientDebtPaymentEntity) => void,
}

const DebtPayment = (props: Props) => {
    const { debt, date, amount, targetAccount } = props.debtPayment;

    const confirmModalRef = useRef<BaseModalRef>(null);
    const editModalRef = useRef<BaseModalRef>(null);

    const onEditClicked = () => {
        editModalRef.current?.openModal()
    };

    const onDeleteClicked = () => {
        confirmModalRef.current?.openModal()
    };

    const { t, i18n } = useTranslation();


    return <Card.Root borderColor="border_primary" color="text_primary" backgroundColor="background_primary" 
            mt={5} mb={5} boxShadow={"sm"} _hover={{ boxShadow: "md" }}>
            <CardBody>
                <Flex justifyContent="space-between" alignItems="center">
                    <Stack direction={"row"}>
                        <Text textAlign={'center'} w={150} rounded={10} padding={1} background={'purple.600'}>{formatDate(date, i18n, false)}</Text>
                        <Text fontWeight={600}>Выплата {formatMoneyByCurrencyCulture(amount, debt.currency.name)} по "{debt.name}" на "{targetAccount.name}"</Text>
                    </Stack>
                    <Flex gap={2} justifyContent="space-between" alignItems="center">
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
            <ConfirmModal onConfirmed={() => props.onDeleteCallback(props.debtPayment)}
                title={t("security_delete_title")}
                message={t("modals_delete_message")}
                confirmActionName={t("modals_delete_button")}
                ref={confirmModalRef}/>
            <DebtPaymentModal debtPayment={props.debtPayment} modalRef={editModalRef} onSaved={props.onEditCallback}/>
        </Card.Root>
};

export default DebtPayment;