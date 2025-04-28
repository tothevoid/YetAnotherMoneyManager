import { Button, Card, Flex, Icon, Stack, Text } from '@chakra-ui/react';
import { MdDelete, MdEdit } from "react-icons/md";
import { Fragment, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { AccountModalRef } from '../../../modals/AccountModal/AccountModal';
import { ConfirmModalRef, ConfirmModal } from '../../../modals/ConfirmModal/ConfirmModal';
import { BrokerAccountSecurityEntity } from '../../../models/brokers/BrokerAccountSecurityEntity';
import { deleteBrokerAccountSecurity, updateBrokerAccountSecurity } from '../../../api/brokers/brokerAccountSecurityApi';
import BrokerAccountSecurityModal from '../modals/BrokerAccountSecurityModal/BrokerAccountSecurityModal';
import { formatMoneyByCurrencyCulture } from '../../../formatters/moneyFormatter';

type Props = {
    brokerAccountSecurity: BrokerAccountSecurityEntity,
    onDeleteCallback: (account: BrokerAccountSecurityEntity) => void,
    onEditCallback: (account: BrokerAccountSecurityEntity) => void,
    onReloadBrokerAccounts: () => void
}

const BrokerAccountSecurity = (props: Props) => {
    const {initialPrice, quantity, security, brokerAccount} = props.brokerAccountSecurity;

    const confirmModalRef = useRef<ConfirmModalRef>(null);
    const editModalRef = useRef<AccountModalRef>(null);

    const onEditClicked = () => {
        editModalRef.current?.openModal()
    };

    const onDeleteClicked = () => {
        confirmModalRef.current?.openModal()
    };

    const onBrokerAccountsSecurityUpdated = async (updatedBrokerAccountSecurity: BrokerAccountSecurityEntity) => {
        const brokerAccountSecurityUpdated = await updateBrokerAccountSecurity(updatedBrokerAccountSecurity);
        if (!brokerAccountSecurityUpdated) {
            return;
        }

        props.onEditCallback(updatedBrokerAccountSecurity);
    }

    const onDeletionConfirmed = async () => {
        const brokerAccountSecurityDeleted = await deleteBrokerAccountSecurity(props.brokerAccountSecurity.id);
        if (!brokerAccountSecurityDeleted) {
            return;
        }

        props.onDeleteCallback(props.brokerAccountSecurity);
    }

    const { t, i18n } = useTranslation();

    return <Fragment>
        <Card.Root backgroundColor="background_primary" borderColor="border_primary" >
            <Card.Body color="text_primary" boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
                <Flex justifyContent="space-between" alignItems="center">
                    <Stack>
                        <Text fontSize="2xl" fontWeight={900}>{security?.name} ({security?.ticker})</Text>
                        <Text fontWeight={600}>{t("broker_account_security_card_security_quantity")}: {quantity}</Text>
                        <Text fontWeight={600}>{t("broker_account_security_card_security_price")}: {formatMoneyByCurrencyCulture(initialPrice, brokerAccount?.currency?.name)}</Text>
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
            title={t("broker_account_securities_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}>
        </ConfirmModal>
        <BrokerAccountSecurityModal brokerAccountSecurity={props.brokerAccountSecurity} 
            ref={editModalRef} onSaved={onBrokerAccountsSecurityUpdated}></BrokerAccountSecurityModal>
    </Fragment>
};

export default BrokerAccountSecurity;