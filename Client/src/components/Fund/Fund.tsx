import './Fund.scss'
import { FundEntity } from '../../models/FundEntity';
import { Button, Card, Flex, Icon, Stack, Text } from '@chakra-ui/react';
import { MdDelete, MdEdit } from "react-icons/md";
import { Fragment, useRef } from 'react';
import FundModal, { FundModalRef } from '../../modals/FundModal/FundModal';
import { formatMoney } from '../../formatters/moneyFormatter';
import { ConfirmModal, ConfirmModalRef } from '../../modals/ConfirmModal/ConfirmModal';
import { deleteFund, updateFund } from '../../api/fundApi';
import { useTranslation } from 'react-i18next';

type Props = {
    fund: FundEntity,
    onDeleteCallback: (fund: FundEntity) => void,
    onEditCallback: (fund: FundEntity) => void
}

const Fund = (props: Props) => {
    const {name, balance} = props.fund;

    const confirmModalRef = useRef<ConfirmModalRef>(null);
    const editModalRef = useRef<FundModalRef>(null);

    const onEditClicked = () => {
        editModalRef.current?.openModal()
    };

    const onDeleteClicked = () => {
        confirmModalRef.current?.openModal()
    };

    const onFundUpdated = async (updatedFund: FundEntity) => {
        const isFundUpdated = await updateFund(updatedFund);
        if (!isFundUpdated) {
            return;
        }

        props.onEditCallback(updatedFund);
    }

    const onDeletionConfirmed = async () => {
        const isFundDeleted = await deleteFund(props.fund);
        if (!isFundDeleted) {
            return;
        }

        props.onDeleteCallback(props.fund);
    }

    const { t } = useTranslation();

    return <Fragment>
        <Card.Root>
            <Card.Body boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
                <Flex justifyContent="space-between" alignItems="center">
                    <Stack>
                        <Text fontWeight={600}>{name}</Text>
                        <Text fontWeight={700}>{formatMoney(balance)}</Text>
                    </Stack>
                    <div>
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
            title={t("fund_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}>
        </ConfirmModal>
        <FundModal fund={props.fund} ref={editModalRef} onSaved={onFundUpdated}></FundModal>
    </Fragment>
};

export default Fund;