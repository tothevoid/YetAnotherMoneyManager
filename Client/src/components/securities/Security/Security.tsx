import { Button, Card, Flex, Icon, Stack, Text } from '@chakra-ui/react';
import { MdDelete, MdEdit } from "react-icons/md";
import { Fragment, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { AccountModalRef } from '../../../modals/AccountModal/AccountModal';
import { ConfirmModalRef, ConfirmModal } from '../../../modals/ConfirmModal/ConfirmModal';
import { SecurityEntity } from '../../../models/securities/SecurityEntity';
import { deleteSecurity, updateSecurity } from '../../../api/securities/securityApi';
import SecurityModal from '../modals/SecurityModal/SecurityModal';

type Props = {
    security: SecurityEntity,
    onDeleteCallback: (security: SecurityEntity) => void,
    onEditCallback: (security: SecurityEntity) => void,
    onReloadSecurities: () => void
}

const Security = (props: Props) => {
    const {name, ticker, type, actualPrice} = props.security;

    const confirmModalRef = useRef<ConfirmModalRef>(null);
    const editModalRef = useRef<AccountModalRef>(null);

    const onEditClicked = () => {
        editModalRef.current?.openModal()
    };

    const onDeleteClicked = () => {
        confirmModalRef.current?.openModal()
    };

    const onSecurityUpdated = async (security: SecurityEntity) => {
        const isAccountUpdated = await updateSecurity(security);
        if (!isAccountUpdated) {
            return;
        }

        props.onEditCallback(security);
    }

    const onDeletionConfirmed = async () => {
        const isAccountDeleted = await deleteSecurity(props.security.id);
        if (!isAccountDeleted) {
            return;
        }

        props.onDeleteCallback(props.security);
    }

    const { t, i18n } = useTranslation();

    return <Fragment>
        <Card.Root backgroundColor="background_primary" borderColor="border_primary" >
            <Card.Body color="text_primary" boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
                <Flex justifyContent="space-between" alignItems="center">
                    <Stack>
                        <Text fontSize={"2xl"} fontWeight={600}>{ticker}</Text>
                        <Text fontWeight={600}>{name}</Text>
                        <Text fontWeight={600}>{type.name}</Text>
                        <Text fontWeight={600}>{actualPrice}</Text>
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
            ref={confirmModalRef}>
        </ConfirmModal>
        <SecurityModal security={props.security} ref={editModalRef} onSaved={onSecurityUpdated}></SecurityModal>
    </Fragment>
};

export default Security;