import { AccountEntity } from '../../models/AccountEntity';
import { Button, Card, Flex, Icon, Stack, Text } from '@chakra-ui/react';
import { MdDelete, MdEdit, MdCompareArrows } from "react-icons/md";
import { Fragment, useRef } from 'react';
import AccountModal, { AccountModalRef } from '../../modals/AccountModal/AccountModal';
import { formatMoneyByCurrencyCulture } from '../../formatters/moneyFormatter';
import { ConfirmModal, ConfirmModalRef } from '../../modals/ConfirmModal/ConfirmModal';
import { deleteAccount, updateAccount } from '../../api/accountApi';
import { useTranslation } from 'react-i18next';
import AccountBalanceTransferModal, { TransferModalRef } from '../../modals/AccountBalanceTransferModal/AccountBalanceTransferModal';

type Props = {
    account: AccountEntity,
    onDeleteCallback: (account: AccountEntity) => void,
    onEditCallback: (account: AccountEntity) => void,
    onReloadAccounts: () => void
}

const Account = (props: Props) => {
    const {name, balance, currency} = props.account;

    const confirmModalRef = useRef<ConfirmModalRef>(null);
    const editModalRef = useRef<AccountModalRef>(null);
    const transferModalRef = useRef<TransferModalRef>(null);

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
        <Card.Root>
            <Card.Body boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
                <Flex justifyContent="space-between" alignItems="center">
                    <Stack>
                        <Text fontWeight={600}>{name}</Text>
                        <Text fontWeight={700}>{formatMoneyByCurrencyCulture(balance, currency.name)}</Text>
                    </Stack>
                    <div>
                        <Button background={'white'} size={'sm'} onClick={onTransferClicked}>
                            <Icon color='blackAlpha.800'>
                                <MdCompareArrows/>
                            </Icon>
                        </Button>
                        <Button background={'white'} size={'sm'} onClick={onEditClicked}>
                            <Icon color='blackAlpha.800'>
                                <MdEdit/>
                            </Icon>
                        </Button>
                        <Button background={'white'} size={'sm'} onClick={onDeleteClicked}>
                            <Icon color={"red.600"}>
                                <MdDelete/>
                            </Icon>
                        </Button>
                    </div>
                </Flex>
            </Card.Body>
        </Card.Root>
        <ConfirmModal onConfirmed={onDeletionConfirmed}
            title={t("account_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}>
        </ConfirmModal>
        <AccountBalanceTransferModal from={props.account} ref={transferModalRef} onTransfered={onTransfered}></AccountBalanceTransferModal>
        <AccountModal account={props.account} ref={editModalRef} onSaved={onAccountUpdated}></AccountModal>
    </Fragment>
};

export default Account;