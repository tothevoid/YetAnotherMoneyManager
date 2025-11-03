import { Box, Button, Icon, Input, Table } from "@chakra-ui/react";
import { useEffect, useRef, useState } from "react";
import { MdAdd, MdDelete } from "react-icons/md";
import { useTranslation } from "react-i18next";
import { ConfirmModal } from "../../../../shared/modals/ConfirmModal/ConfirmModal";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import { createBank, deleteBank, getBanks, updateBank } from "../../../../api/banks/bankApi";
import { BankEntity } from "../../../../models/banks/BankEntity";
import BankModal from "../../modals/BankModal/BankModal";

interface State {
    banks: BankEntity[],
    hasChanges: boolean,
    currentBankId: string | null
}

const BanksTable: React.FC = () => {
    const [state, setState] = useState<State>({banks: [], hasChanges: false, currentBankId: null});
    const { t } = useTranslation();

    const modalRef = useRef<BaseModalRef>(null);
    const confirmModalRef = useRef<BaseModalRef>(null);

    useEffect(() => {
        const initData = async () => { 
            const banks = await getBanks();
            setState((currentState) => {
                return {...currentState, banks}
            })
        }

        initData();
    }, []);

    const onNameChanged = (bankId: string, newValue: string) => {
        let hasChanges = false;

        const updatedBanks = state.banks.map((bank: BankEntity) => {
            if (bank.id !== bankId || bank.name === newValue) {
                return bank;
            }

            hasChanges = true;
            return {...bank, name: newValue};
        });

        if (!hasChanges) {
            return;
        }

        setState((currentState) => {
            return {...currentState, banks: updatedBanks, hasChanges: true}
        })
    }

    const onCellBlur = async (bankId: string) => {
        if (!state.hasChanges){
            return;
        }

        const bank = state.banks.find((bank: BankEntity) => {
            return bank.id === bankId;
        });
        if (!bank) {
            return;
        }

        await updateBank({...bank});
    }
    
    const onAdd = () => {
        modalRef.current?.openModal()
    };

    const onBankAdded = async (bank: BankEntity) => {
        const createdBank = await createBank(bank);
        if (!createdBank) {
            return;
        }

        setState((currentState) => {
            return {...currentState, banks: [...currentState.banks, createdBank]}
        })
    };

    const onDeleteClicked = async (bank: BankEntity) => {
        setState((currentState) => {
            return {...currentState, currentBankId: bank.id}
        })
        confirmModalRef.current?.openModal()
    }

    const onDeleteConfirmed = async () => {
        const {currentBankId} = state;

        if (!currentBankId){
            return;
        }

        const isDeleted = await deleteBank(currentBankId);

        if (!isDeleted) {
            return;
        }

        const banks = state.banks.filter((bank: BankEntity) => {
            return bank.id !== state.currentBankId;
        });

        setState((currentState) => {
            return {...currentState, banks, currentBankId: null}
        })
    }

    return <Box color="text_primary">
        <Table.Root>
            <Table.Header>
                <Table.Row border="none" bg="none" color="text_primary">
                    <Table.ColumnHeader color="text_primary">Name</Table.ColumnHeader>
                    <Table.ColumnHeader color="text_primary">Delete</Table.ColumnHeader>
                </Table.Row>
            </Table.Header>
            <Table.Body>
                {
                    state.banks.map((bank: BankEntity) => {
                        return <Table.Row border="none" bg="none" color="text_primary" key={bank.id}>
                            <Table.Cell>
                                <Input onBlur={() => onCellBlur(bank.id)} type="text" value={bank.name}
                                    onChange={(handler) => onNameChanged(bank.id, handler.target.value)}>
                                </Input>
                            </Table.Cell>
                            <Table.Cell width={10}>
                                <Button borderColor="background_secondary" 
                                    background="button_background_secondary" 
                                    size={'sm'} 
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
        <Box padding={4}>
            <Button background="action_primary" onClick={onAdd}>
                <Icon size='md'>
                    <MdAdd/>
                </Icon>
                {t("entity_bank_add")}
            </Button>
        </Box>
        <BankModal modalRef={modalRef} onSaved={onBankAdded}/>
        <ConfirmModal onConfirmed={onDeleteConfirmed}
            title={t("banks_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}/>
    </Box>
}

export default BanksTable;