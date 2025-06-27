import { ResponsiveContainer, LineChart, CartesianGrid, XAxis, YAxis, Tooltip, Legend, Line } from "recharts";
import { Box } from "@chakra-ui/react";
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { getTransactionsBySecurity } from "../../../../api/securities/securityTransactionApi";
import { formatDate } from "../../../../shared/utilities/formatters/dateFormatter";
import { formatMoneyByCurrencyCulture } from "../../../../shared/utilities/formatters/moneyFormatter";
import { getChartLabelConfig } from "../../../../shared/utilities/chartUtilities";
import { SecurityTransactionsHistory } from "../../../../models/securities/SecurityTransactionsHistory";

interface Props {
    securityId: string
    currencyName: string
}

interface State {
    transactions: SecurityTransactionsHistory[],
}

const SecurityTransactionsChart: React.FC<Props> = (props) => {
    const [state, setState] = useState<State>({transactions: []})

    const { t, i18n } = useTranslation();

    const initData = async () => {
        const historyValues = await getTransactionsBySecurity(props.securityId);
        if (!historyValues) {
            return;
        }

        const transactions = historyValues.map(historyValue => {
            historyValue.date = formatDate(new Date(historyValue.date), i18n)
            return historyValue;
        })

        setState((currentState) => {
            return {...currentState, transactions}
        })
    }

    useEffect(() => {
        initData();
    }, []);

    return <Box style={{ width: '100%', height: 400 }}>
        <ResponsiveContainer>
            <LineChart data={state.transactions}
                margin={{
                    top: 20,
                    bottom: 20,
                }}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="date"/>
                <YAxis domain={['dataMin - 10', 'dataMax + 10']}/>
                <Tooltip contentStyle={getChartLabelConfig()} formatter={(value: number) => formatMoneyByCurrencyCulture(value, props.currencyName)} />
                <Legend />
                <Line type="monotone" dataKey="valueWithPayments" stroke="#16a34a" activeDot={{ r: 8 }} name={t("securities_transactions_with_dividends")}/>
                <Line type="monotone" dataKey="valueWithoutPayments" stroke="#8884d8" activeDot={{ r: 8 }} name={t("securities_transactions_without_dividends")}/>
            </LineChart>
        </ResponsiveContainer>
    </Box>
}

export default SecurityTransactionsChart;