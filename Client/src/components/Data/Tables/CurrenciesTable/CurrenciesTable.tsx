import { Box, Checkbox, Input, Table } from "@chakra-ui/react";
import { CurrencyEntity } from "../../../../models/CurrencyEntity";
import { useEffect, useState } from "react";
import { getCurrencies, updateCurrency } from "../../../../api/currencyApi";

interface Props {
}

interface State {
    currencies: CurrencyEntity[],
    hasChanges: boolean
}

const CurrenciesTable: React.FC<Props> = () => {
    const [state, setState] = useState<State>({currencies: [], hasChanges: false})

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

    return <Box color="text_primary">
        <Table.Root>
            <Table.Header>
                <Table.Row border="none" bg="none" color="text_primary">
                    <Table.ColumnHeader color="text_primary">Name</Table.ColumnHeader>
                    <Table.ColumnHeader color="text_primary">Active</Table.ColumnHeader>
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
                            <Table.Cell>
                                <Checkbox.Root onBlur={() => onCellBlur(currency.id)} checked={currency.active} variant="subtle"
                                    onCheckedChange={(data) => {onCellChanged(currency.id, "active", data.checked)}}>
                                    <Checkbox.HiddenInput />
                                    <Checkbox.Control />
                                </Checkbox.Root>
                            </Table.Cell>
                        </Table.Row>
                    })
                }
            </Table.Body>
        </Table.Root>
    </Box>
}

export default CurrenciesTable;