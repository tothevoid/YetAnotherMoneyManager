import { Box, Button, Checkbox, Icon, Input, Table } from "@chakra-ui/react";
import { useEffect, useRef, useState } from "react";
import { MdAdd, MdDelete } from "react-icons/md";
import { useTranslation } from "react-i18next";
import { ConfirmModal, ConfirmModalRef } from "../../../../modals/ConfirmModal/ConfirmModal";
import { TransactionTypeEntity } from "../../../../models/transactions/TransactionTypeEntity";
import { createTransactionType, deleteTransactionType, getTransactionTypes, updateTransactionType } from "../../../../api/transactions/transactionTypeApi";
import TransactionTypeModal, { TransactionTypeModalRef } from "../../../../modals/TransactionTypeModal/TransactionTypeModal";

interface Props {}

interface State {
    transactionTypes: TransactionTypeEntity[],
    hasChanges: boolean,
    currentTransactionTypeId: string | null
}

const TransactionTypesTable: React.FC<Props> = () => {
    const [state, setState] = useState<State>({transactionTypes: [], hasChanges: false, currentTransactionTypeId: null});
    const { t } = useTranslation();
    const modalRef = useRef<TransactionTypeModalRef>(null);
    const confirmModalRef = useRef<ConfirmModalRef>(null);

    useEffect(() => {
        const initData = async () => { 
            const transactionTypes = await getTransactionTypes();
            setState((currentState) => {
                return {...currentState, transactionTypes}
            })
        }

        initData();
    }, []);

    const onCellChanged = (transactionTypeId: string, propertyName: string, newValue: any) => {
        let hasChanges = false;

        const updatedTransactionType = state.transactionTypes.map((transactionType: TransactionTypeEntity) => {
            if (transactionType.id !== transactionTypeId) {
                return transactionType;
            }

            const currentValue = transactionType[propertyName];
            if (currentValue === newValue) {
                return transactionType;
            }

            hasChanges = true;
            return {...transactionType, [propertyName]: newValue};
        });

        if (!hasChanges) {
            return;
        }

        setState((currentState) => {
            return {...currentState, transactionTypes: updatedTransactionType, hasChanges: true}
        })
    }

    const onCellBlur = async (transactionTypeId: string) => {
        if (!state.hasChanges){
            return;
        }

        const transactionType = state.transactionTypes.find((transactionType: TransactionTypeEntity) => {
            return transactionType.id === transactionTypeId;
        });
        if (!transactionType) {
            return;
        }

        await updateTransactionType({...transactionType});
    }
    
    const onAdd = () => {
        modalRef.current?.openModal()
    };

    const onTransactionTypeAdded = async (transactionType: TransactionTypeEntity) => {
        const createdTransactionTypeId = await createTransactionType(transactionType);
        if (!createdTransactionTypeId) {
            return;
        }

        transactionType.id = createdTransactionTypeId;

        setState((currentState) => {
            return {...currentState, transactionTypes: [...currentState.transactionTypes, transactionType]}
        })
    };

    const onDeleteClicked = async (transactionType: TransactionTypeEntity) => {
        setState((currentState) => {
            return {...currentState, currentTransactionTypeId: transactionType.id}
        })
        confirmModalRef.current?.openModal()
    }

    const onDeleteConfirmed = async () => {
        const {currentTransactionTypeId} = state;

        if (!currentTransactionTypeId){
            return;
        }

        const isDeleted = await deleteTransactionType(currentTransactionTypeId);
        
        if (!isDeleted) {
            return;
        }

        const transactionTypes = state.transactionTypes.filter((transactionType: TransactionTypeEntity) => {
            return transactionType.id !== state.currentTransactionTypeId;
        });

        setState((currentState) => {
            return {...currentState, transactionTypes: transactionTypes, currentTransactionTypeId: null}
        })
    }

    return <Box color="text_primary">
        <Table.Root>
            <Table.Header>
                <Table.Row border="none" bg="none" color="text_primary">
                    <Table.ColumnHeader color="text_primary">Name</Table.ColumnHeader>
                    <Table.ColumnHeader color="text_primary">Active</Table.ColumnHeader>
                    <Table.ColumnHeader color="text_primary">Delete</Table.ColumnHeader>
                </Table.Row>
            </Table.Header>
            <Table.Body>
                {
                    state.transactionTypes.map((transactionType: TransactionTypeEntity) => {
                        return <Table.Row border="none" bg="none" color="text_primary" key={transactionType.id}>
                            <Table.Cell>
                                <Input onBlur={() => onCellBlur(transactionType.id)} type="text" value={transactionType.name}
                                    onChange={(handler) => onCellChanged(transactionType.id, "name", handler.target.value)}>
                                </Input>
                            </Table.Cell>
                            <Table.Cell width={10}>
                                <Checkbox.Root onBlur={() => onCellBlur(transactionType.id)} checked={transactionType.active} variant="subtle"
                                    onCheckedChange={(data) => {onCellChanged(transactionType.id, "active", data.checked)}}>
                                    <Checkbox.HiddenInput />
                                    <Checkbox.Control />
                                </Checkbox.Root>
                            </Table.Cell>
                            <Table.Cell width={10}>
                                <Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={() => onDeleteClicked(transactionType)}>
                                    <Icon color="card_action_icon_danger">
                                        <MdDelete/>
                                    </Icon>
                                </Button>
                            </Table.Cell>
                        </Table.Row>
                    })
                }
            </Table.Body>
        </Table.Root>
        <Box padding={4}>
            <Button background="purple.600" onClick={onAdd}>
                <Icon size='md'>
                    <MdAdd/>
                </Icon>
                {t("transaction_type_data_add")}
            </Button>
        </Box>
        <TransactionTypeModal ref={modalRef} onSaved={onTransactionTypeAdded}></TransactionTypeModal>
        <ConfirmModal onConfirmed={onDeleteConfirmed}
                    title={t("transaction_type_delete_title")}
                    message={t("modals_delete_message")}
                    confirmActionName={t("modals_delete_button")}
                    ref={confirmModalRef}>
        </ConfirmModal>
    </Box>
}

export default TransactionTypesTable;