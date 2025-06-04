import { Button, Card, Flex, Icon, Link, Stack, Text, Image } from '@chakra-ui/react';
import { MdDelete, MdEdit } from "react-icons/md";
import { Fragment, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { HiOutlineBuildingOffice2 } from 'react-icons/hi2';
import { updateSecurity, deleteSecurity, getIconUrl } from '../../../../api/securities/securityApi';
import { SecurityEntity } from '../../../../models/securities/SecurityEntity';
import { ConfirmModal } from '../../../../shared/modals/ConfirmModal/ConfirmModal';
import { formatMoneyByCurrencyCulture } from '../../../../shared/utilities/formatters/moneyFormatter';
import { BaseModalRef } from '../../../../shared/utilities/modalUtilities';
import SecurityModal from '../../modals/SecurityModal/SecurityModal';

type Props = {
    security: SecurityEntity,
    onDeleteCallback: (security: SecurityEntity) => void,
    onEditCallback: (security: SecurityEntity) => void,
    onReloadSecurities: () => void
}

const Security = (props: Props) => {
    const {id, name, ticker, type, actualPrice, currency, iconKey} = props.security;

    const confirmModalRef = useRef<BaseModalRef>(null);
    const editModalRef = useRef<BaseModalRef>(null);

    const onEditClicked = () => {
        editModalRef.current?.openModal()
    };

    const onDeleteClicked = () => {
        confirmModalRef.current?.openModal()
    };

    const onSecurityUpdated = async (security: SecurityEntity, icon: File | null) => {
        const isAccountUpdated = await updateSecurity(security, icon);
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

    const securityLink = `../security/${id}`;

    const icon = iconKey ?
        <Image h={8} w={8} rounded={16} src={getIconUrl(iconKey)}/>:
        <HiOutlineBuildingOffice2 size={32} color="#aaa" />

    return <Fragment>
        <Card.Root backgroundColor="background_primary" borderColor="border_primary" >
            <Card.Body color="text_primary" boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
                <Flex justifyContent="space-between" alignItems="center">
                    <Stack>
                        <Stack justifyContent={"start"} direction="row">
                            {icon}
                            <Link fontSize="2xl" fontWeight={600} color="text_primary" href={securityLink}>{ticker}</Link>
                        </Stack>
                        <Text fontWeight={600}>{name}</Text>
                        <Text fontWeight={600}>{type.name}</Text>
                        <Text fontWeight={600}>{formatMoneyByCurrencyCulture(actualPrice, currency.name)}</Text>
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
        <SecurityModal security={props.security} modalRef={editModalRef} onSaved={onSecurityUpdated}/>
    </Fragment>
};

export default Security;