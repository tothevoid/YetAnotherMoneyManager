import { Button, Card, Flex, Icon, Stack, Text } from '@chakra-ui/react';
import { MdDelete, MdEdit } from "react-icons/md";
import { Fragment, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { ConfirmModal } from '../../../../shared/modals/ConfirmModal/ConfirmModal';
import { formatMoneyByCurrencyCulture } from '../../../../shared/utilities/formatters/moneyFormatter';
import { deleteDebt, updateDebt } from '../../../../api/debts/debtApi';
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
    const { debt, date, amount, targetAccount, transaction } = props.debt;

    const confirmModalRef = useRef<BaseModalRef>(null);
    const editModalRef = useRef<BaseModalRef>(null);

    const onEditClicked = () => {
        editModalRef.current?.openModal()
    };

    const onDeleteClicked = () => {
        confirmModalRef.current?.openModal()
    };

    const onDebtUpdated = async (debt: ClientDebtPaymentEntity) => {
        const debtUpdated = await updateDebt(debt);
        if (!debtUpdated) {
            return;
        }

        props.onEditCallback(debt);
    }

    const onDeletionConfirmed = async () => {
        const deleted = await deleteDebt(props.debt.id);
        if (!deleted) {
            return;
        }

        props.onDeleteCallback(props.debt);
    }

    const { t, i18n } = useTranslation();

    return <Fragment>
        <Card.Root backgroundColor="background_primary" borderColor="border_primary" >
            <Card.Body color="text_primary" boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
                <Flex justifyContent="space-between" alignItems="center">
                    <Stack>
                        <Text fontWeight={600}>{formatMoneyByCurrencyCulture(amount, debt.currency.name)}</Text>
                        <Text fontWeight={600}>{formatDate(date, i18n)}</Text>
                    </Stack>
                    <Flex gap={1}>
                        <Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={onEditClicked}>
                            <Icon color="card_action_icon_primary">
                                <MdEdit/>
                            </Icon>
                        </Button>
                        <Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={onDeleteClicked}>
                            <Icon color="card_action_icon_danger">
                                <MdDelete/>
                            </Icon>
                        </Button>
                    </Flex>
                </Flex>
            </Card.Body>
        </Card.Root>
        <ConfirmModal onConfirmed={onDeletionConfirmed}
            title={t("security_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}/>
        <DebtPaymentModal debt={props.debt} modalRef={editModalRef} onSaved={onDebtUpdated}/>
    </Fragment>
};

export default DebtPayment;