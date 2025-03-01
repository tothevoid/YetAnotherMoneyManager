import { EditIcon, DeleteIcon, CopyIcon } from "@chakra-ui/icons";
import { Card, CardBody, Flex, Stack, Button, Text, Container } from "@chakra-ui/react";
import { formatDate, formatNumericDate } from "../../formatters/dateFormatter";
import { formatMoney } from "../../formatters/moneyFormatter";
import { DepositEntity } from "../../models/DepositEntity";
import DepositModal, { DepositModalRef } from "../../modals/DepositModal/DepositModal";
import { useRef } from "react";
import { ConfirmModal, ConfirmModalRef } from "../../modals/ConfirmModal/ConfirmModal";
import { createDeposit, deleteDeposit, updateDeposit } from "../../api/depositApi";

interface Props {
    deposit: DepositEntity
    onUpdated: (deposit: DepositEntity) => void,
    onCloned: (deposit: DepositEntity) => void,
    onDeleted: (deposit: DepositEntity) => void
}

const Deposit: React.FC<Props> = ({deposit, onUpdated, onCloned, onDeleted}) => {
    const confirmDeleteModalRef = useRef<ConfirmModalRef>(null);
    const editModalRef = useRef<DepositModalRef>(null);

    const showEditDepositModal = () => {
        editModalRef.current?.openModal()
    };

    const onDepositSaved = async (updatedDeposit: DepositEntity) => {
        const isUpdated = await updateDeposit(updatedDeposit);
        if (!isUpdated) {
            return;
        }

        onUpdated(updatedDeposit);
    }

    const onDeleteClicked = () => {
        confirmDeleteModalRef.current?.openModal()
    }

    const onDeletionConfirmed = async () => {
        const isDeleted = await deleteDeposit(deposit);
        if (!isDeleted) {
            return;
        }
        onDeleted(deposit);
    }

    const onCloneClick = async () => {
        onCloned(deposit);
    }

    const datesColor = deposit.to > new Date() ?
        "green.500":
        "red.500";

    return <Card>
        <CardBody boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
            <Stack>
                <Text fontSize={"xl"} fontWeight={600}>{deposit.name}</Text>
                <Container padding={0}>
                    <Flex justifyContent="space-between">
                        <Text color={"gray.500"}>Initial amount:</Text>
                        <Text>{formatMoney(deposit.initialAmount)}</Text>
                    </Flex>
                    <Flex justifyContent="space-between">
                        <Text color={"gray.500"}>Percentage:</Text>
                        <Text >{deposit.percentage}%</Text>
                    </Flex>
                    <Flex justifyContent="space-between">
                        <Text color={"gray.500"}>Dates:</Text>
                        <Text color={datesColor}>{`${formatNumericDate(deposit.from)} - ${formatNumericDate(deposit.to)}`}</Text>
                    </Flex>
                    <Flex paddingTop={4} justifyContent="end">
                        <Button onClick={showEditDepositModal} background={'white'} size={'sm'}>
                            <EditIcon/>
                        </Button>
                        <Button onClick={onCloneClick} background={'white'} size={'sm'}>
                            <CopyIcon/>
                        </Button>
                        <Button onClick={onDeleteClicked} background={'white'} size={'sm'}>
                            <DeleteIcon color={"red.600"}/>
                        </Button>
                    </Flex>
                </Container>
            </Stack>
        </CardBody>
        <ConfirmModal onConfirmed={onDeletionConfirmed}
            title="Delete fund"
            message="Are you sure? You can't undo this action afterwards"
            confirmActionName="Delete"
            ref={confirmDeleteModalRef}>
        </ConfirmModal>
        <DepositModal deposit={deposit} ref={editModalRef} onSaved={onDepositSaved}/>
    </Card>
}

export default Deposit;