import { Button, Card, Flex, Icon, Stack, Text, Image } from '@chakra-ui/react';
import { MdDelete, MdEdit } from "react-icons/md";
import { Fragment, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { ConfirmModal } from '../../../../shared/modals/ConfirmModal/ConfirmModal';
import { formatMoneyByCurrencyCulture } from '../../../../shared/utilities/formatters/moneyFormatter';
import { BaseModalRef } from '../../../../shared/utilities/modalUtilities';
import { CryptocurrencyEntity } from '../../../../models/crypto/CryptocurrencyEntity';
import { FaBitcoin } from "react-icons/fa";
import CryptocurrencyModal from '../../modals/CryptocurrencyModal';
import { getIconUrl } from '../../../../api/crypto/cryptocurrencyApi';

type Props = {
    cryptocurrency: CryptocurrencyEntity,
    onDeleteCallback: (cryptocurrency: CryptocurrencyEntity) => void,
    onEditCallback: (cryptocurrency: CryptocurrencyEntity, file: File | null) => void,
    onReloadSecurities: () => void
}

const Cryptocurrency = (props: Props) => {
    const {id, name, symbol, price, iconKey} = props.cryptocurrency;

    const confirmModalRef = useRef<BaseModalRef>(null);
    const editModalRef = useRef<BaseModalRef>(null);

    const onEditClicked = () => {
        editModalRef.current?.openModal()
    };

    const onDeleteClicked = () => {
        confirmModalRef.current?.openModal()
    };

    const { t } = useTranslation();

    const icon = iconKey ?
        <Image h={8} w={8} rounded={16} src={getIconUrl(iconKey)}/>:
        <FaBitcoin size={32} color="#aaa" />

    return <Fragment>
        <Card.Root backgroundColor="background_primary" borderColor="border_primary" >
            <Card.Body color="text_primary" boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
                <Flex justifyContent="space-between" alignItems="center">
                    <Stack>
                        <Stack justifyContent={"start"} direction="row">
                            {icon}
                            <Text fontSize="2xl" fontWeight={600} color="text_primary">{symbol}</Text>
                        </Stack>
                        <Text fontWeight={600}>{name}</Text>
                        <Text fontWeight={600}>{formatMoneyByCurrencyCulture(price, "USD")}</Text>
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
        <ConfirmModal onConfirmed={() => props.onDeleteCallback(props.cryptocurrency)}
            title={t("security_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}/>
        <CryptocurrencyModal cryptocurrency={props.cryptocurrency} modalRef={editModalRef} onSaved={props.onEditCallback}/>
    </Fragment>
};

export default Cryptocurrency;