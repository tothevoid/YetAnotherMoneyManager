import { Box, Button, Icon, Table, Text, Image } from "@chakra-ui/react";
import { useEffect, useRef, useState } from "react";
import { MdDelete, MdEdit } from "react-icons/md";
import { useTranslation } from "react-i18next";
import { ConfirmModal } from "../../../../shared/modals/ConfirmModal/ConfirmModal";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import { Nullable } from "../../../../shared/utilities/nullable";
import { createBank, deleteBank, getBankIconUrl, getBanks, updateBank } from "../../../../api/banks/bankApi";
import { BankEntity } from "../../../../models/banks/BankEntity";
import BankModal from "../../modals/BankModal/BankModal";
import { BsBank } from "react-icons/bs";
import AddButton from "../../../../shared/components/AddButton/AddButton";

interface State {
    banks: BankEntity[]
}

const BanksTable: React.FC = () => {
    const [state, setState] = useState<State>({
        banks: []});

    const [bankToDeletedId, setBankToDeleteId] = useState<Nullable<string>>();
    const [updatedBank, setUpdatedBank] = useState<BankEntity | null>();

    const { t } = useTranslation();
    const modalRef = useRef<BaseModalRef>(null);
    const confirmModalRef = useRef<BaseModalRef>(null);

    const fetchBanks = async () => { 
        const banks = await getBanks();
        setState((currentState) => {
            return {...currentState, banks}
        })
    }

    useEffect(() => {
        fetchBanks();
    }, []);

    useEffect(() => {
        if (bankToDeletedId) {
            confirmModalRef.current?.openModal();
        }
    }, [bankToDeletedId]);

    useEffect(() => {
        if (updatedBank) {
            modalRef.current?.openModal(); 
        }
    }, [updatedBank]);
    
    const onAdd = () => {
        modalRef.current?.openModal()
    };

    const onEditClicked = (bank: BankEntity) => {
        setUpdatedBank(bank);
    }

    const onModalClosed = () => {
        setUpdatedBank(null);
    }

    const onBankSaved = async (savedBank: BankEntity, icon: Nullable<File>) => {
        const isModified = state.banks
            .findIndex(transactionType => transactionType.id === savedBank.id) >= 0;

        if (isModified) {
            await onBankUpdated(savedBank, icon);
        } else {
            await onBankAdded(savedBank, icon);
        }
    };

    const onBankAdded = async (bank: BankEntity, icon: Nullable<File>) => {
        const addedBank = await createBank(bank, icon);
        if (!addedBank) {
            return;
        }

        setState((currentState) => {
            return {...currentState, banks: [...currentState.banks, addedBank]}
        })
    }

    const onBankUpdated = async (bankToUpdate: BankEntity, icon: Nullable<File>) => {
        const updatedBank = await updateBank(bankToUpdate, icon);
        if (!updatedBank) {
            return;
        }

        setState((currentState) => {
            return {...currentState, banks: currentState.banks.map(bank => 
                bankToUpdate.id !== bank.id ?
                    bank:
                    updatedBank
            )}
        })
    }

    const onDeleteClicked = async (bank: BankEntity) => {
        setBankToDeleteId(bank.id)
    }

    const onDeleteConfirmed = async () => {
        if (!bankToDeletedId){
            return;
        }

        const isDeleted = await deleteBank(bankToDeletedId);
        
        if (!isDeleted) {
            return;
        }

        const banks = state.banks.filter((bank: BankEntity) => {
            return bank.id !== bankToDeletedId;
        });

        setState((currentState) => {
            return {...currentState, banks: banks}
        })
        setBankToDeleteId(null);
    }

    return <Box color="text_primary">
        <AddButton buttonTitle={t("entity_bank_add")} onClick={onAdd}/>
        <Table.Root>
            <Table.Header>
                <Table.Row border="none" bg="none" color="text_primary">
                    <Table.ColumnHeader w={"50px"}/>
                    <Table.ColumnHeader color="text_primary">Name</Table.ColumnHeader>
                    <Table.ColumnHeader/>
                    <Table.ColumnHeader/>
                </Table.Row>
            </Table.Header>
            <Table.Body>
                {
                    state.banks.map((bank: BankEntity) => {
                        return <Table.Row border="none" bg="none" color="text_primary" key={bank.id}>
                            <Table.Cell>
                                {
                                    bank.iconKey ?
                                        <Image h={8} w={8} rounded={16} src={getBankIconUrl(bank?.iconKey)}
                                            objectFit="contain"
                                            borderColor="gray.200"
                                            borderRadius="md">
                                        </Image>:
                                        <BsBank size={32} color="#aaa"/>
                                }
                            </Table.Cell>
                            <Table.Cell>
                                <Text>
                                    {bank.name}
                                </Text>
                            </Table.Cell>
                            <Table.Cell width={10}>
                                <Button borderColor="background_secondary" 
                                    background="button_background_secondary" size={'sm'} 
                                    onClick={() => onEditClicked(bank)}>
                                    <Icon color="card_action_icon_primary">
                                        <MdEdit/>
                                    </Icon>
                                </Button>
                            </Table.Cell>
                            <Table.Cell width={10}>
                                <Button borderColor="background_secondary" 
                                    background="button_background_secondary" size={'sm'} 
                                    onClick={() => onDeleteClicked(bank)}>
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
        <BankModal onModalClosed={onModalClosed} bank={updatedBank} modalRef={modalRef} onSaved={onBankSaved}/>
        <ConfirmModal onConfirmed={onDeleteConfirmed}
            title={t("transaction_type_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}/>
    </Box>
}

export default BanksTable;