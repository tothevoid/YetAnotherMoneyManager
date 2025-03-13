import './Fund.scss'
import { FundEntity } from '../../models/FundEntity';
import { Button, Card, Flex, Icon, Stack, Text } from '@chakra-ui/react';
import { MdDelete, MdEdit } from "react-icons/md";
import { Fragment, useRef } from 'react';
import FundModal, { FundModalRef } from '../../modals/FundModal/FundModal';
import { formatMoney } from '../../formatters/moneyFormatter';
import { ConfirmModal, ConfirmModalRef } from '../../modals/ConfirmModal/ConfirmModal';

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

    const onDeletionConfirmed = () => {
        props.onDeleteCallback(props.fund);
    }

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
            title="Delete fund"
            message="Are you sure? You can't undo this action afterwards"
            confirmActionName="Delete"
            ref={confirmModalRef}>
        </ConfirmModal>
        <FundModal fund={props.fund} ref={editModalRef} onSaved={props.onEditCallback}></FundModal>
    </Fragment>
};

export default Fund;