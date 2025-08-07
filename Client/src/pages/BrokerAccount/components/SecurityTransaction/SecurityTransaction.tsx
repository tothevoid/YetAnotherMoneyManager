import { Button, Card, CardBody, Flex, Icon, Span, Stack, Text } from '@chakra-ui/react';
import { MdDelete, MdEdit } from "react-icons/md";
import { useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { ConfirmModal } from '../../../../shared/modals/ConfirmModal/ConfirmModal';
import { SecurityTransactionEntity } from '../../../../models/securities/SecurityTransactionEntity';
import { formatDateTime } from '../../../../shared/utilities/formatters/dateFormatter';
import { formatMoneyByCurrencyCulture } from '../../../../shared/utilities/formatters/moneyFormatter';
import { BaseModalRef } from '../../../../shared/utilities/modalUtilities';
import SecurityTransactionModal from '../../modals/SecurityTransactionModal/SecurityTransactionModal';

type Props = {
    securityTransaction: SecurityTransactionEntity,
    onDeleteCallback: (account: SecurityTransactionEntity) => void,
    onEditCallback: (account: SecurityTransactionEntity) => void,
    onReloadSecurityTransactions: () => void
}

const SecurityTransaction = (props: Props) => {
    const { security, date, price, quantity } = props.securityTransaction;

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
                <Stack direction={'row'} alignItems="center">
                    <Text textAlign={'center'} w={150} rounded={10} padding={1} background={'purple.600'}>{formatDateTime(date, i18n, false)}</Text>
                    <Text fontWeight={700}>{security?.name} ({security?.ticker})</Text>
                </Stack>
                <Flex gap={2} justifyContent="space-between" alignItems="center">
                    <Text>
                        <Span>{formatMoneyByCurrencyCulture(price * quantity, security.currency.name)} </Span>
                        <Span pl={2.5} pr={2.5}>({formatMoneyByCurrencyCulture(price, security.currency.name)} x {quantity})</Span>
                        
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
        <ConfirmModal onConfirmed={() => props.onDeleteCallback(props.securityTransaction)}
            title={t("entity_securities_transaction_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}/>
        <SecurityTransactionModal securityTransaction={props.securityTransaction}  modalRef={editModalRef} onSaved={props.onEditCallback}/>
    </Card.Root>
};

export default SecurityTransaction;