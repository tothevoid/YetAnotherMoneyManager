import { Button, Card, Flex, Icon, Stack, Text } from '@chakra-ui/react';
import { MdDelete, MdEdit } from "react-icons/md";
import { Fragment, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { AccountModalRef } from '../../../modals/AccountModal/AccountModal';
import { ConfirmModalRef, ConfirmModal } from '../../../modals/ConfirmModal/ConfirmModal';
import { SecurityTransactionEntity } from '../../../models/securities/SecurityTransactionEntity';
import SecurityTransactionModal from '../modals/SecurityTransactionModal/SecurityTransactionModal';
import { deleteSecurityTransaction, updateSecurityTransaction } from '../../../api/securities/securityTransactionApi';
import { formatDate } from '../../../formatters/dateFormatter';

type Props = {
    securityTransaction: SecurityTransactionEntity,
    onDeleteCallback: (account: SecurityTransactionEntity) => void,
    onEditCallback: (account: SecurityTransactionEntity) => void,
    onReloadSecurityTransactions: () => void
}

const SecurityTransaction = (props: Props) => {
    const { security, commission, date, price, quantity, tax } = props.securityTransaction;

    const confirmModalRef = useRef<ConfirmModalRef>(null);
    const editModalRef = useRef<AccountModalRef>(null);

    const onEditClicked = () => {
        editModalRef.current?.openModal()
    };

    const onDeleteClicked = () => {
        confirmModalRef.current?.openModal()
    };

    const onSecurityTransactionUpdated = async (updatedSecurityTransaction: SecurityTransactionEntity) => {
        const securityTransactionUpdated = await updateSecurityTransaction(updatedSecurityTransaction);
        if (!securityTransactionUpdated) {
            return;
        }

        props.onEditCallback(updatedSecurityTransaction);
    }

    const onDeletionConfirmed = async () => {
        const securityTransactionDeleted = await deleteSecurityTransaction(props.securityTransaction.id);
        if (!securityTransactionDeleted) {
            return;
        }

        props.onDeleteCallback(props.securityTransaction);
    }

    const { t, i18n } = useTranslation();

    return <Fragment>
        <Card.Root backgroundColor="background_primary" borderColor="border_primary" >
            <Card.Body color="text_primary" boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
                <Flex justifyContent="space-between" alignItems="center">
                    <Stack>
                        <Text fontSize="2xl" fontWeight={900}>{security?.name}</Text>
                        <Text fontWeight={600}>{commission}</Text>
                        <Text fontWeight={600}>{price}</Text>
                        <Text fontWeight={600}>{quantity}</Text>
                        <Text fontWeight={600}>{tax}</Text>
                        <Text fontWeight={600}>{formatDate(date, i18n, false)}</Text>
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
            title={t("entity_securities_transaction_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}>
        </ConfirmModal>
        <SecurityTransactionModal securityTransaction={props.securityTransaction} 
            ref={editModalRef} onSaved={onSecurityTransactionUpdated}></SecurityTransactionModal>
    </Fragment>
};

export default SecurityTransaction;