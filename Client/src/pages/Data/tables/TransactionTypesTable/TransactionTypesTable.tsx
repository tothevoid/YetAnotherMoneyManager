import { Box, Button, Checkbox, Icon, Table, Text, Image } from "@chakra-ui/react";
import { useEffect, useRef, useState } from "react";
import { MdAdd, MdDelete, MdEdit, MdOutlinePayment } from "react-icons/md";
import { useTranslation } from "react-i18next";
import { ConfirmModal } from "../../../../shared/modals/ConfirmModal/ConfirmModal";
import { TransactionTypeEntity } from "../../../../models/transactions/TransactionTypeEntity";
import { createTransactionType, deleteTransactionType, getTransactionTypeIconUrl, getTransactionTypes, updateTransactionType } from "../../../../api/transactions/transactionTypeApi";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import TransactionTypeModal from "../../modals/TransactionTypeModal/TransactionTypeModal";

interface Props {}

interface State {
    transactionTypes: TransactionTypeEntity[]
}

const TransactionTypesTable: React.FC<Props> = () => {
    const [state, setState] = useState<State>({
        transactionTypes: []});

    const [transactionTypeToDeleteId, setTransactionTypeToDeleteId] = useState<string | null>();
    const [updatedTransactionType, setUpdatedTransactionType] = useState<TransactionTypeEntity | null>();

    const { t } = useTranslation();
    const modalRef = useRef<BaseModalRef>(null);
    const confirmModalRef = useRef<BaseModalRef>(null);

    useEffect(() => {
        const initData = async () => { 
            const transactionTypes = await getTransactionTypes();
            setState((currentState) => {
                return {...currentState, transactionTypes}
            })
        }

        initData();
    }, []);

    useEffect(() => {
        if (transactionTypeToDeleteId) {
            confirmModalRef.current?.openModal();
        }
    }, [transactionTypeToDeleteId]);

    useEffect(() => {
        if (updatedTransactionType) {
            modalRef.current?.openModal(); 
        }
    }, [updatedTransactionType]);
    
    const onAdd = () => {
        modalRef.current?.openModal()
    };

    const onEditClicked = (transactionType: TransactionTypeEntity) => {
        setUpdatedTransactionType(transactionType);
    }

    const onTransactionTypeSaved = async (savedTransactionType: TransactionTypeEntity, icon: File | null) => {
        const isModified = state.transactionTypes
            .findIndex(transactionType => transactionType.id === savedTransactionType.id) >= 0;

        if (isModified) {
            await onTransactionTypeUpdated(savedTransactionType, icon);
        } else {
            await onTransactionTypeAdded(savedTransactionType, icon);
        }
    };

    const onTransactionTypeAdded = async (savedTransactionType: TransactionTypeEntity, icon: File | null) => {
        const addedTransactionType = await createTransactionType(savedTransactionType, icon);
        if (!addedTransactionType) {
            return;
        }

        setState((currentState) => {
            return {...currentState, transactionTypes: [...currentState.transactionTypes, addedTransactionType]}
        })
    }

    const onTransactionTypeUpdated = async (savedTransactionType: TransactionTypeEntity, icon: File | null) => {
        const updatedTransactionType = await updateTransactionType(savedTransactionType, icon);
        if (!updatedTransactionType) {
            return;
        }

        setState((currentState) => {
            return {...currentState, transactionTypes: currentState.transactionTypes.map(transactionType => 
                transactionType.id !== savedTransactionType.id ?
                    transactionType:
                    updatedTransactionType
            )}
        })
        setUpdatedTransactionType(null)
    }

    const onDeleteClicked = async (transactionType: TransactionTypeEntity) => {
        setTransactionTypeToDeleteId(transactionType.id)
    }

    const onDeleteConfirmed = async () => {
        if (!transactionTypeToDeleteId){
            return;
        }

        const isDeleted = await deleteTransactionType(transactionTypeToDeleteId);
        
        if (!isDeleted) {
            return;
        }

        const transactionTypes = state.transactionTypes.filter((transactionType: TransactionTypeEntity) => {
            return transactionType.id !== transactionTypeToDeleteId;
        });

        setState((currentState) => {
            return {...currentState, transactionTypes: transactionTypes}
        })
        setTransactionTypeToDeleteId(null);
    }

    return <Box color="text_primary">
        <Box>
            <Button background="purple.600" onClick={onAdd}>
                <Icon size='md'>
                    <MdAdd/>
                </Icon>
                {t("transaction_type_data_add")}
            </Button>
        </Box>
        <Table.Root>
            <Table.Header>
                <Table.Row border="none" bg="none" color="text_primary">
                    <Table.ColumnHeader w={"10px"}/>
                    <Table.ColumnHeader color="text_primary">Name</Table.ColumnHeader>
                    <Table.ColumnHeader color="text_primary">Active</Table.ColumnHeader>
                    <Table.ColumnHeader/>
                    <Table.ColumnHeader/>
                </Table.Row>
            </Table.Header>
            <Table.Body>
                {
                    state.transactionTypes.map((transactionType: TransactionTypeEntity) => {
                        return <Table.Row border="none" bg="none" color="text_primary" key={transactionType.id}>
                            <Table.Cell>
                                {
                                    transactionType.iconKey ?
                                        <Image h={8} w={8} rounded={16} src={getTransactionTypeIconUrl(transactionType?.iconKey)}
                                            objectFit="contain"
                                            borderColor="gray.200"
                                            borderRadius="md">
                                        </Image>:
                                        <MdOutlinePayment size={32} color="#aaa"/>
                                }
                            </Table.Cell>
                            <Table.Cell>
                                <Text>
                                    {transactionType.name}
                                </Text>
                            </Table.Cell>
                            <Table.Cell width={10}>
                                <Checkbox.Root disabled checked={transactionType.active} variant="subtle">
                                    <Checkbox.HiddenInput />
                                    <Checkbox.Control />
                                </Checkbox.Root>
                            </Table.Cell>
                            <Table.Cell width={10}>
                                <Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={() => onEditClicked(transactionType)}>
                                    <Icon color="card_action_icon_primary">
                                        <MdEdit/>
                                    </Icon>
                                </Button>
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
                }modalRef
            </Table.Body>
        </Table.Root>
        <TransactionTypeModal transactionType={updatedTransactionType} modalRef={modalRef} onSaved={onTransactionTypeSaved}/>
        <ConfirmModal onConfirmed={onDeleteConfirmed}
            title={t("transaction_type_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}/>
    </Box>
}

export default TransactionTypesTable;