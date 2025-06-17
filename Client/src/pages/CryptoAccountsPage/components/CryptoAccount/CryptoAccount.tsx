import { Button, Card, Flex, Icon, Link, Stack, Text } from '@chakra-ui/react';
import { MdDelete, MdEdit } from "react-icons/md";
import { Fragment, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { ConfirmModal } from '../../../../shared/modals/ConfirmModal/ConfirmModal';
import { BaseModalRef } from '../../../../shared/utilities/modalUtilities';
import { ClientCryptoAccountEntity } from '../../../../models/crypto/CryptoAccountEntity';
import CryptoAccountModal from '../../modals/CryptoAccountModal/CryptoAccountModal';

type Props = {
    cryptoAccount: ClientCryptoAccountEntity,
    onDeleteCallback: (cryptoAccount: ClientCryptoAccountEntity) => void,
    onEditCallback: (cryptoAccount: ClientCryptoAccountEntity) => void,
    onReloadCryptoAccounts: () => void
}

const CryptoAccount = (props: Props) => {
    const {id, name, cryptoProvider} = props.cryptoAccount;

    const confirmModalRef = useRef<BaseModalRef>(null);
    const editModalRef = useRef<BaseModalRef>(null);

    const onEditClicked = () => {
        editModalRef.current?.openModal()
    };

    const onDeleteClicked = () => {
        confirmModalRef.current?.openModal()
    };

    const { t } = useTranslation();

    const accountLink = `../crypto_account/${id}`;

    return <Fragment>
        <Card.Root backgroundColor="background_primary" borderColor="border_primary" >
            <Card.Body color="text_primary" boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
                <Flex justifyContent="space-between" alignItems="center">
                    <Stack>
                        <Link fontSize="2xl" fontWeight={900} color="text_primary" href={accountLink}>{name}</Link>
                        <Text fontWeight={600}>{cryptoProvider.name}</Text>
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
        <ConfirmModal onConfirmed={() => props.onDeleteCallback(props.cryptoAccount)}
            title={t("broker_account_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}/>
        <CryptoAccountModal cryptoAccount={props.cryptoAccount} modalRef={editModalRef} onSaved={props.onEditCallback}/>
    </Fragment>
};

export default CryptoAccount;