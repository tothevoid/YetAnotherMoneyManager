import { Button, Card, CardBody, Flex, Icon, Stack, Text } from '@chakra-ui/react';
import { MdDelete, MdEdit } from "react-icons/md";
import { useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { DividendEntity } from '../../../../models/securities/DividendEntity';
import { ConfirmModal } from '../../../../shared/modals/ConfirmModal/ConfirmModal';
import { formatDate } from '../../../../shared/utilities/formatters/dateFormatter';
import { formatMoneyByCurrencyCulture } from '../../../../shared/utilities/formatters/moneyFormatter';
import { BaseModalRef } from '../../../../shared/utilities/modalUtilities';
import DividendModal, { EditDividendContext } from '../../modals/DividendModal/DividendModal';

type Props = {
    dividend: DividendEntity,
    onDeleteCallback: (dividend: DividendEntity) => void,
    onEditCallback: (dividend: DividendEntity) => void,
    onReloadDividends: () => void
}

const Dividend = (props: Props) => {
    const { amount, security, declarationDate, snapshotDate } = props.dividend;

    const confirmModalRef = useRef<BaseModalRef>(null);
    const editModalRef = useRef<BaseModalRef>(null);

    const onEditClicked = () => {
        editModalRef.current?.openModal()
    };

    const onDeleteClicked = () => {
        confirmModalRef.current?.openModal()
    };

    const context: EditDividendContext = {
        dividend: props.dividend
    }

    const { t, i18n } = useTranslation();
    return <Card.Root borderColor="border_primary" color="text_primary" backgroundColor="background_primary" 
        mt={5} mb={5} boxShadow={"sm"} _hover={{ boxShadow: "md" }}>
        <CardBody>
            <Flex justifyContent="space-between" alignItems="center">
                <Stack direction={'row'} alignItems="center">
                    <Stack ml={5}>
                        <Text>{t("entity_dividend_declaration_date")}: {formatDate(declarationDate, i18n)}</Text>
                        <Text>{t("entity_dividend_payment_date")}: {formatDate(snapshotDate, i18n)}</Text>
                    </Stack>
                </Stack>
                <Flex gap={2} justifyContent="space-between" alignItems="center">
                    <Text width={100}>{formatMoneyByCurrencyCulture(amount, security.currency.name)}</Text>
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
        <ConfirmModal onConfirmed={() => props.onDeleteCallback(props.dividend)}
            title={t("transaction_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}/>
        <DividendModal context={context} modalRef={editModalRef} onSaved={props.onEditCallback}/>
    </Card.Root>
};

export default Dividend;