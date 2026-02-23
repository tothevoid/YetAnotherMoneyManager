import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Stack, Text, Table, Card } from "@chakra-ui/react";
import { getAccountById } from "../../api/accounts/accountApi";
import { getCurrencyTransactionsByAccountId } from "../../api/transactions/currencyTransactionApi";
import { CurrencyTransactionEntity } from "../../models/transactions/CurrencyTransactionEntity";
import { useTranslation } from "react-i18next";
import { formatMoneyByCurrencyCulture } from "../../shared/utilities/formatters/moneyFormatter";
import { formatDate, formatNumericDate, formatShortDateTime } from "../../shared/utilities/formatters/dateFormatter";
import { getCurrencies } from "../../api/currencies/currencyApi";
import { calculateDiff, getDiffColor } from "../../shared/utilities/numericDiffsUtilities";
import { AccountEntity } from "../../models/accounts/AccountEntity";
import { useUserProfile } from "../../../features/UserProfileSettingsModal/hooks/UserProfileContext";

interface State {
    currencyTransactions: CurrencyTransactionEntity[],
}

const CashAccountPage: React.FC = () => {
    const { cashAccountId } = useParams();
    const { t, i18n } = useTranslation();

    const { user } = useUserProfile()

    const [state, setState] = useState<State>({ currencyTransactions: [] })
    const [currenciesMap, setCurrenciesMap] = useState<Record<string, number>>({});

    useEffect(() => {
        const initData = async () => {
            if (!cashAccountId) return;
            const account = await getAccountById(cashAccountId);
            setAccount(account);
            const currencyTransactions = await getCurrencyTransactionsByAccountId(cashAccountId);
            const currencies = await getCurrencies();
            const map: Record<string, number> = {};
            currencies.forEach(c => { map[c.id] = c.rate });
            setCurrenciesMap(map);
            setState({ currencyTransactions });
        }
        initData();
    }, [cashAccountId]);

    const [totalPnl, setTotalPnl] = useState<number>(0);

    useEffect(() => {
        if (!state.currencyTransactions || state.currencyTransactions.length === 0) {
            return;
        }

        const totalPnl = state.currencyTransactions.reduce((acc, transaction) => {
            const currentRate = currenciesMap[transaction.destinationAccount.currency.id];
            const transactionRate = transaction.rate;
            const diffResult = calculateDiff(
                currentRate * transaction.amount,
                transactionRate * transaction.amount,
                // TODO: Fix currency
                transaction.sourceAccount.currency.name
            );
            return acc + diffResult.rawProfitAndLoss;
        }, 0);
        setTotalPnl(totalPnl);
    }, [state.currencyTransactions, currenciesMap])

    const [account, setAccount] = useState<AccountEntity | null>(null);

    return (
        <Stack p={6} gap={4}>
            <Stack alignItems={"end"} gapX={2} direction={"row"} color="text_primary">
                <Text fontSize="3xl" fontWeight={900}>
                    {account ? account.name : t("currency_transactions_title")}
                </Text>
                <Text color={getDiffColor(totalPnl)} backgroundColor="background_primary" borderColor="border_primary" textAlign={'center'} minW={150} rounded={10} padding={2} background={'black.600'}> {totalPnl > 0 ? "+" : ""}{formatMoneyByCurrencyCulture(totalPnl, user?.currency.name)}</Text>
            </Stack>
            <Card.Root>
                <Table.Root variant="outline" size="lg">
                    <Table.Header>
                        <Table.Row backgroundColor="background_secondary">
                            <Table.ColumnHeader color="text_primary">
                                {t("entity_currency_transaction_date")}
                            </Table.ColumnHeader>
                            <Table.ColumnHeader color="text_primary">
                                {t("entity_currency_transaction_name")}
                            </Table.ColumnHeader>
                            <Table.ColumnHeader color="text_primary">
                                {t("entity_currency_transaction_source_account")}
                            </Table.ColumnHeader>
                            <Table.ColumnHeader color="text_primary">
                                {t("entity_currency_transaction_destination_account")}
                            </Table.ColumnHeader>
                            <Table.ColumnHeader color="text_primary">
                                {t("entity_currency_transaction_amount")}
                            </Table.ColumnHeader>
                            <Table.ColumnHeader color="text_primary">
                                {t("entity_currency_transaction_rate")}
                            </Table.ColumnHeader>
                            <Table.ColumnHeader color="text_primary">
                                {t("currency_transaction_total")}
                            </Table.ColumnHeader>
                            <Table.ColumnHeader color="text_primary">
                                {t("currency_transactions_current_rate")}
                            </Table.ColumnHeader>
                            <Table.ColumnHeader color="text_primary">
                                {t("currency_transactions_diff")}
                            </Table.ColumnHeader>
                        </Table.Row>
                    </Table.Header>
                    <Table.Body>
                        {state.currencyTransactions.length === 0 ? (
                            <Table.Row>
                                <Table.Cell colSpan={8} textAlign="center" color="text_secondary" py={8}>
                                    {t("manager_transactions_empty")}
                                </Table.Cell>
                            </Table.Row>
                        ) : (
                            state.currencyTransactions.map((transaction) => {
                                const currentRate = currenciesMap[transaction.destinationAccount.currency.id];
                                const transactionRate = transaction.rate;
                                const diffResult = calculateDiff(currentRate * transaction.amount, transactionRate * transaction.amount,
                                    transaction.sourceAccount.currency.name);
                                return (
                                    <Table.Row key={transaction.id} color="text_primary" backgroundColor="background_primary">
                                        <Table.Cell>
                                            {formatNumericDate(transaction.date, i18n)}
                                        </Table.Cell>
                                        <Table.Cell>
                                            {transaction.name}
                                        </Table.Cell>
                                        <Table.Cell>
                                            {transaction.sourceAccount.name}
                                        </Table.Cell>
                                        <Table.Cell>
                                            {transaction.destinationAccount.name}
                                        </Table.Cell>
                                        <Table.Cell>
                                            {formatMoneyByCurrencyCulture(
                                                transaction.amount, 
                                                transaction.destinationAccount.currency.name
                                            )}
                                        </Table.Cell>
                                        <Table.Cell>
                                           {formatMoneyByCurrencyCulture(
                                                transaction.rate, 
                                                transaction.sourceAccount.currency.name
                                            )}
                                        </Table.Cell>
                                        <Table.Cell>
                                            {formatMoneyByCurrencyCulture(
                                                transaction.amount * transaction.rate, 
                                                transaction.sourceAccount.currency.name
                                            )}
                                        </Table.Cell>
                                        <Table.Cell>
                                            {formatMoneyByCurrencyCulture(
                                                currentRate, 
                                                user?.currency.name
                                            )}
                                        </Table.Cell>
                                        <Table.Cell>
                                            <Text color={diffResult.color} fontWeight={700}>
                                                {diffResult.rawProfitAndLoss > 0 ? "+" : ""}{diffResult.profitAndLoss}
                                            </Text>
                                        </Table.Cell>
                                    </Table.Row>
                                );
                            })
                        )}
                    </Table.Body>
                </Table.Root>
            </Card.Root>
        </Stack>
    );
}

export default CashAccountPage;