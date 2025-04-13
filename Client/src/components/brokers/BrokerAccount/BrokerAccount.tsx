import { Button, Card, Flex, Icon, Stack, Text } from '@chakra-ui/react';
import { MdDelete, MdEdit, MdCompareArrows } from "react-icons/md";
import { Fragment, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { deleteAccount } from '../../../api/accounts/accountApi';
import { formatMoneyByCurrencyCulture } from '../../../formatters/moneyFormatter';
import { AccountModalRef } from '../../../modals/AccountModal/AccountModal';
import { ConfirmModalRef, ConfirmModal } from '../../../modals/ConfirmModal/ConfirmModal';
import { BrokerAccountEntity } from '../../../models/brokers/BrokerAccountEntity';
import BrokerAccountModal from '../modals/BrokerAccountModal/BrokerAccountModal';
import { updateBrokerAccount } from '../../../api/brokers/brokerAccountApi';
import { formatDate } from '../../../formatters/dateFormatter';

type Props = {
    brokerAccount: BrokerAccountEntity,
    onDeleteCallback: (account: BrokerAccountEntity) => void,
    onEditCallback: (account: BrokerAccountEntity) => void,
    onReloadBrokerAccounts: () => void
}

const BrokerAccount = (props: Props) => {
    const {name, broker, currency, type, assetValue, lastUpdateAt} = props.brokerAccount;

    const confirmModalRef = useRef<ConfirmModalRef>(null);
    const editModalRef = useRef<AccountModalRef>(null);

    const onEditClicked = () => {
        editModalRef.current?.openModal()
    };

    const onDeleteClicked = () => {
        confirmModalRef.current?.openModal()
    };

    const onBrokerAccountsUpdated = async (updatedBrokerAccount: BrokerAccountEntity) => {
        const isAccountUpdated = await updateBrokerAccount(updatedBrokerAccount);
        if (!isAccountUpdated) {
            return;
        }

        props.onEditCallback(updatedBrokerAccount);
    }

    const onDeletionConfirmed = async () => {
        const isAccountDeleted = await deleteAccount(props.brokerAccount.id);
        if (!isAccountDeleted) {
            return;
        }

        props.onDeleteCallback(props.brokerAccount);
    }

    const { t, i18n } = useTranslation();

    return <Fragment>
        <Card.Root backgroundColor="background_primary" borderColor="border_primary" >
            <Card.Body color="text_primary" boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
                <Flex justifyContent="space-between" alignItems="center">
                    <Stack>
                        <Text fontWeight={600}>{name}</Text>
                        <Text fontWeight={600}>{broker.name}</Text>
                        <Text fontWeight={600}>{type.name}</Text>
                        <Text fontWeight={700}>{formatMoneyByCurrencyCulture(assetValue, currency.name)} ({formatDate(lastUpdateAt, i18n)})</Text>
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
            title={t("broker_account_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}>
        </ConfirmModal>
        <BrokerAccountModal brokerAccount={props.brokerAccount} ref={editModalRef} onSaved={onBrokerAccountsUpdated}></BrokerAccountModal>
    </Fragment>
};

export default BrokerAccount;