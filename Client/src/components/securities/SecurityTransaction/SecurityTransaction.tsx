import { Button, Card, CardBody, Flex, Icon, Span, Stack, Text } from '@chakra-ui/react';
import { MdDelete, MdEdit } from "react-icons/md";
import { Fragment, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { AccountModalRef } from '../../../modals/AccountModal/AccountModal';
import { ConfirmModalRef, ConfirmModal } from '../../../modals/ConfirmModal/ConfirmModal';
import { SecurityTransactionEntity } from '../../../models/securities/SecurityTransactionEntity';
import SecurityTransactionModal from '../modals/SecurityTransactionModal/SecurityTransactionModal';
import { deleteSecurityTransaction, updateSecurityTransaction } from '../../../api/securities/securityTransactionApi';
import { formatDate } from '../../../formatters/dateFormatter';
import { formatMoneyByCurrencyCulture } from '../../../formatters/moneyFormatter';

type Props = {
    securityTransaction: SecurityTransactionEntity,
    onDeleteCallback: (account: SecurityTransactionEntity) => void,
    onEditCallback: (account: SecurityTransactionEntity) => void,
    onReloadSecurityTransactions: () => void
}

const SecurityTransaction = (props: Props) => {
    const { brokerAccount, security, commission, date, price, quantity, tax } = props.securityTransaction;

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

    return <Card.Root borderColor="border_primary" color="text_primary" backgroundColor="background_primary" 
        mt={5} mb={5} boxShadow={"sm"} _hover={{ boxShadow: "md" }}>
        <CardBody>
            <Flex justifyContent="space-between" alignItems="center">
                <Stack direction={'row'} alignItems="center">
                    <Text textAlign={'center'} w={125} rounded={10} padding={1} background={'purple.600'}>{formatDate(date, i18n, false)}</Text>
                    <Text fontWeight={700}>{security?.name} ({security?.ticker})</Text>
                </Stack>
                <Flex gap={2} justifyContent="space-between" alignItems="center">
                    <Text>
                        <Span>{formatMoneyByCurrencyCulture(price * quantity, brokerAccount.currency.name)} </Span>
                        <Span pl={2.5} pr={2.5}>({formatMoneyByCurrencyCulture(price, brokerAccount.currency.name)} x {quantity})</Span>
                        
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
        <ConfirmModal onConfirmed={onDeletionConfirmed}
            title={t("entity_securities_transaction_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}>
        </ConfirmModal>
        <SecurityTransactionModal securityTransaction={props.securityTransaction} 
            ref={editModalRef} onSaved={onSecurityTransactionUpdated}></SecurityTransactionModal>
    </Card.Root>
};

export default SecurityTransaction;