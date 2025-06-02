import { Button, Card, Flex, Icon, Stack, Text } from '@chakra-ui/react';
import { MdDelete, MdEdit, MdCompareArrows } from "react-icons/md";
import { Fragment, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { updateAccount, deleteAccount } from '../../../api/accounts/accountApi';
import { formatMoneyByCurrencyCulture } from '../../../formatters/moneyFormatter';
import AccountBalanceTransferModal from '../../../modals/AccountBalanceTransferModal/AccountBalanceTransferModal';
import AccountModal from '../../../modals/AccountModal/AccountModal';
import { ConfirmModal } from '../../../modals/ConfirmModal/ConfirmModal';
import { AccountEntity } from '../../../models/accounts/AccountEntity';
import { BaseModalRef } from '../../../common/ModalUtilities';

type Props = {
    account: AccountEntity,
    onDeleteCallback: (account: AccountEntity) => void,
    onEditCallback: (account: AccountEntity) => void,
    onReloadAccounts: () => void
}

const Account = (props: Props) => {
    const {name, balance, currency} = props.account;

    const confirmModalRef = useRef<BaseModalRef>(null);
    const editModalRef = useRef<BaseModalRef>(null);
    const transferModalRef = useRef<BaseModalRef>(null);

    const onTransferClicked = () => {
        transferModalRef.current?.openModal()
    }

    const onEditClicked = () => {
        editModalRef.current?.openModal()
    };

    const onDeleteClicked = () => {
        confirmModalRef.current?.openModal()
    };

    const onTransfered = () => {
        props.onReloadAccounts();
    }

    const onAccountUpdated = async (updatedAccount: AccountEntity) => {
        const isAccountUpdated = await updateAccount(updatedAccount);
        if (!isAccountUpdated) {
            return;
        }

        props.onEditCallback(updatedAccount);
    }

    const onDeletionConfirmed = async () => {
        const isAccountDeleted = await deleteAccount(props.account.id);
        if (!isAccountDeleted) {
            return;
        }

        props.onDeleteCallback(props.account);
    }

    const { t } = useTranslation();

    return <Fragment>
        <Card.Root backgroundColor="background_primary" borderColor="border_primary" >
            <Card.Body color="text_primary" boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
                <Flex justifyContent="space-between" alignItems="center">
                    <Stack>
                        <Text fontWeight={600}>{name}</Text>
                        <Text fontWeight={700}>{formatMoneyByCurrencyCulture(balance, currency.name)}</Text>
                    </Stack>
                    <Flex gap={1}>
                        <Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={onTransferClicked}>
                            <Icon color="card_action_icon_primary">
                                <MdCompareArrows/>
                            </Icon>
                        </Button>
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
            title={t("account_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}/>
        <AccountBalanceTransferModal from={props.account} modalRef={transferModalRef} onTransfered={onTransfered}/>
        <AccountModal account={props.account} modalRef={editModalRef} onSaved={onAccountUpdated}/>
    </Fragment>
};

export default Account;