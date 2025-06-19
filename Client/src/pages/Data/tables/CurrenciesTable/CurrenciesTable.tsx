import { Box, Button, Checkbox, Icon, Input, Stack, Table, Text } from "@chakra-ui/react";
import { CurrencyEntity } from "../../../../models/currencies/CurrencyEntity";
import { useEffect, useRef, useState } from "react";
import { MdAdd, MdDelete, MdRefresh } from "react-icons/md";
import { useTranslation } from "react-i18next";
import { ConfirmModal } from "../../../../shared/modals/ConfirmModal/ConfirmModal";
import { getCurrencies, updateCurrency, createCurrency, deleteCurrency, syncRates } from "../../../../api/currencies/currencyApi";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import CurrencyModal from "../../modals/CurrencyModal/CurrencyModal";
import { useUserProfile } from "../../../../../features/UserProfileSettingsModal/hooks/UserProfileContext";
import { formatMoneyByCurrencyCulture } from "../../../../shared/utilities/formatters/moneyFormatter";
import RefreshButton from "../../../../shared/components/RefreshButton/RefreshButton";

interface Props {}

interface State {
    currencies: CurrencyEntity[],
    hasChanges: boolean,
    currentCurrencyId: string | null
}

const CurrenciesTable: React.FC<Props> = () => {
    const [state, setState] = useState<State>({currencies: [], hasChanges: false, currentCurrencyId: null});
    const { t } = useTranslation();
    
    const { user } = useUserProfile()

    const [isSyncing, setSyncing] = useState(false);

    const modalRef = useRef<BaseModalRef>(null);
    const confirmModalRef = useRef<BaseModalRef>(null);

    useEffect(() => {
        fetchCurrencies();
    }, []);

     const fetchCurrencies = async () => { 
        const currencies = await getCurrencies();
        setState((currentState) => {
            return {...currentState, currencies}
        })
    }

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

    const onSyncRates = async () => {
        setSyncing(true);
        await syncRates();
        await fetchCurrencies();
        setSyncing(false);
    }

    return <Box color="text_primary">
        <Table.Root>
            <Table.Header>
                <Table.Row border="none" bg="none" color="text_primary">
                    <Table.ColumnHeader color="text_primary">{t("entity_currency_name")}</Table.ColumnHeader>
                    <Table.ColumnHeader color="text_primary">{t("entity_currency_rate")}</Table.ColumnHeader>
                    <Table.ColumnHeader color="text_primary">{t("entity_currency_active")}</Table.ColumnHeader>
                    <Table.ColumnHeader color="text_primary"></Table.ColumnHeader>
                </Table.Row>
            </Table.Header>
            <Table.Body>
                {
                    state.currencies.map((currency: CurrencyEntity) => {
                        return <Table.Row border="none" bg="none" color="text_primary" key={currency.id}>
                            <Table.Cell>
                                <Text>{currency.name}</Text>
                            </Table.Cell>
                            <Table.Cell>
                                <Text>{formatMoneyByCurrencyCulture(1, currency.name)} = {formatMoneyByCurrencyCulture(currency.rate, user?.currency.name, 3)}</Text>
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
        <Stack direction={"row"} padding={4} gapX={2}>
            <Button background="purple.600" onClick={onAdd}>
                <Icon size='md'>
                    <MdAdd/>
                </Icon>
                {t("currencies_data_add")}
            </Button>
            <RefreshButton isRefreshing={isSyncing} title={t("currencies_data_sync_rates")} onClick={onSyncRates} />
        </Stack>
        <CurrencyModal modalRef={modalRef} onSaved={onCurrencyAdded}/>
        <ConfirmModal onConfirmed={onDeleteConfirmed}
            title={t("currencies_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}/>
    </Box>
}

export default CurrenciesTable;