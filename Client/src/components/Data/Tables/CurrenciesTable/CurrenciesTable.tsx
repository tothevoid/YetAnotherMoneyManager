import { Box, Button, Checkbox, Icon, Input, Table } from "@chakra-ui/react";
import { CurrencyEntity } from "../../../../models/currencies/CurrencyEntity";
import { useEffect, useRef, useState } from "react";
import { createCurrency, deleteCurrency, getCurrencies, updateCurrency } from "../../../../api/currencyApi";
import { MdAdd, MdDelete } from "react-icons/md";
import { useTranslation } from "react-i18next";
import CurrencyModal, { CurrencyModalRef } from "../../../../modals/CurrencyModal/CurrencyModal";
import { ConfirmModal, ConfirmModalRef } from "../../../../modals/ConfirmModal/ConfirmModal";

interface Props {
}

interface State {
    currencies: CurrencyEntity[],
    hasChanges: boolean,
    currentCurrencyId: string | null
}

const CurrenciesTable: React.FC<Props> = () => {
    const [state, setState] = useState<State>({currencies: [], hasChanges: false, currentCurrencyId: null});
    const { t } = useTranslation();
    const modalRef = useRef<CurrencyModalRef>(null);
    const confirmModalRef = useRef<ConfirmModalRef>(null);

    useEffect(() => {
        const initData = async () => { 
            const currencies = await getCurrencies();
            setState((currentState) => {
                return {...currentState, currencies}
            })
        }

        initData();
    }, []);

    const onCellChanged = (currencyId: string, propertyName: string, newValue: any) => {
        let hasChanges = false;

        const updatedCurrencies = state.currencies.map((currency: CurrencyEntity) => {
            if (currency.id !== currencyId) {
                return currency;
            }

            const currentValue = currency[propertyName];
            if (currentValue === newValue) {
                return currency;
            }

            hasChanges = true;
            return {...currency, [propertyName]: newValue};
        });

        if (!hasChanges) {
            return;
        }

        setState((currentState) => {
            return {...currentState, currencies: updatedCurrencies, hasChanges: true}
        })
    }

    const onCellBlur = async (currencyId: string) => {
        if (!state.hasChanges){
            return;
        }

        const currency = state.currencies.find((currency: CurrencyEntity) => {
            return currency.id === currencyId;
        });
        if (!currency) {
            return;
        }

        await updateCurrency({...currency});
    }
    
    const onAdd = () => {
        modalRef.current?.openModal()
    };

    const onCurrencyAdded = async (currency: CurrencyEntity) => {
        const cureatedCurrencyId = await createCurrency(currency);
        if (!cureatedCurrencyId) {
            return;
        }

        currency.id = cureatedCurrencyId;

        setState((currentState) => {
            return {...currentState, currencies: [...currentState.currencies, currency]}
        })
    };

    const onDeleteClicked = async (currency: CurrencyEntity) => {
        setState((currentState) => {
            return {...currentState, currentCurrencyId: currency.id}
        })
        confirmModalRef.current?.openModal()
    }

    const onDeleteConfirmed = async () => {
        const {currentCurrencyId} = state;

        if (!currentCurrencyId){
            return;
        }

        const isDeleted = await deleteCurrency(currentCurrencyId);
        
        if (!isDeleted) {
            return;
        }

        const currencies = state.currencies.filter((currency: CurrencyEntity) => {
            return currency.id !== state.currentCurrencyId;
        });

        setState((currentState) => {
            return {...currentState, currencies, currentCurrencyId: null}
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
                    state.currencies.map((currency: CurrencyEntity) => {
                        return <Table.Row border="none" bg="none" color="text_primary" key={currency.id}>
                            <Table.Cell>
                                <Input onBlur={() => onCellBlur(currency.id)} type="text" value={currency.name}
                                    onChange={(handler) => onCellChanged(currency.id, "name", handler.target.value)}>
                                </Input>
                            </Table.Cell>
                            <Table.Cell width={10}>
                                <Checkbox.Root onBlur={() => onCellBlur(currency.id)} checked={currency.active} variant="subtle"
                                    onCheckedChange={(data) => {onCellChanged(currency.id, "active", data.checked)}}>
                                    <Checkbox.HiddenInput />
                                    <Checkbox.Control />
                                </Checkbox.Root>
                            </Table.Cell>
                            <Table.Cell width={10}>
                                <Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={() => onDeleteClicked(currency)}>
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
                {t("currencies_data_add")}
            </Button>
        </Box>
        <CurrencyModal ref={modalRef} onSaved={onCurrencyAdded}></CurrencyModal>
        <ConfirmModal onConfirmed={onDeleteConfirmed}
                    title={t("currencies_delete_title")}
                    message={t("modals_delete_message")}
                    confirmActionName={t("modals_delete_button")}
                    ref={confirmModalRef}>
        </ConfirmModal>
    </Box>
}

export default CurrenciesTable;