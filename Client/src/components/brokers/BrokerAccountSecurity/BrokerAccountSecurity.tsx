import { Button, Card, Flex, Icon, Link, Span, Stack, Text, Image } from '@chakra-ui/react';
import { MdDelete, MdEdit } from "react-icons/md";
import { Fragment, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { ConfirmModal } from '../../../modals/ConfirmModal/ConfirmModal';
import { BrokerAccountSecurityEntity } from '../../../models/brokers/BrokerAccountSecurityEntity';
import { deleteBrokerAccountSecurity, updateBrokerAccountSecurity } from '../../../api/brokers/brokerAccountSecurityApi';
import BrokerAccountSecurityModal from '../modals/BrokerAccountSecurityModal/BrokerAccountSecurityModal';
import { formatMoneyByCurrencyCulture } from '../../../formatters/moneyFormatter';
import { getIconUrl } from '../../../api/securities/securityApi';
import { HiOutlineBuildingOffice2 } from 'react-icons/hi2';
import { BaseModalRef } from '../../../common/ModalUtilities';

type Props = {
    brokerAccountSecurity: BrokerAccountSecurityEntity,
    onDeleteCallback: (account: BrokerAccountSecurityEntity) => void,
    onEditCallback: (account: BrokerAccountSecurityEntity) => void,
    onReloadBrokerAccounts: () => void
}

const BrokerAccountSecurity = (props: Props) => {
    const {price, quantity, security, brokerAccount} = props.brokerAccountSecurity;

    const confirmModalRef = useRef<BaseModalRef>(null);
    const editModalRef = useRef<BaseModalRef>(null);

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

    const actualPrice = security.actualPrice * quantity;
    const profitAndLoss = actualPrice - price;

    const color = profitAndLoss > 0 ?
        "green.600":
        "red.600";

    const percentage = profitAndLoss / actualPrice * 100;

    const securityLink = `../security/${security.id}`;

    //TODO: Fix duplication with SecurityPage
    const icon = security.iconKey ?
        <Image h={8} w={8} rounded={16} src={getIconUrl(security.iconKey)}/>:
        <HiOutlineBuildingOffice2 size={32} color="#aaa" />

    return <Fragment>
        <Card.Root backgroundColor="background_primary" borderColor="border_primary" >
            <Card.Body color="text_primary" boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
                <Flex justifyContent="space-between" alignItems="center">
                    <Stack>
                        <Stack justifyContent={"center"} direction={"row"}>
                            {icon}
                            <Link color="text_primary" href={securityLink} fontSize="xl" fontWeight={900}>{security?.name} ({security?.ticker})</Link>
                        </Stack>
                        <Text fontWeight={600}>{t("broker_account_security_card_security_quantity")}: {quantity}</Text>
                        <Text fontWeight={600}>{t("broker_account_security_card_security_initial_price")}: {formatMoneyByCurrencyCulture(price, security?.currency?.name)}</Text>
                        <Text fontWeight={600}>{t("broker_account_security_card_security_current_price")}: {formatMoneyByCurrencyCulture(actualPrice, security?.currency?.name)}</Text>
                        <Text fontWeight={600}>
                            {t("broker_account_security_card_security_p&l")}:
                            <Span paddingLeft={1.5} color={color}>{formatMoneyByCurrencyCulture(profitAndLoss, security?.currency?.name)} ({percentage.toFixed(2)}%)</Span>
                        </Text>
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
            ref={confirmModalRef}/>
        <BrokerAccountSecurityModal brokerAccountSecurity={props.brokerAccountSecurity} modalRef={editModalRef} onSaved={onBrokerAccountsSecurityUpdated}/>
    </Fragment>
};

export default BrokerAccountSecurity;