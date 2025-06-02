import { ResponsiveContainer, LineChart, CartesianGrid, XAxis, YAxis, Tooltip, Legend, Line } from "recharts";
import { Box } from "@chakra-ui/react";
import { useEffect, useState } from "react";
import { getTransactionsBySecurity } from "../../../api/securities/securityTransactionApi";
import { SecurityTransactionEntity } from "../../../models/securities/SecurityTransactionEntity";
import { formatDate } from "../../../formatters/dateFormatter";
import { useTranslation } from "react-i18next";

interface Props {
    securityId: string
}

interface State {
    transactions: SecurityTransactionEntity[]
}

const SecurityTransactionsChart: React.FC<Props> = (props) => {
    const [state, setState] = useState<State>({transactions: []})

    const { i18n } = useTranslation();

    const initData = async () => {
        const transactions = await getTransactionsBySecurity(props.securityId);
        if (!transactions) {
            return;
        }

        const fixedTransactions = transactions.map(transaction => {
            transaction.date = formatDate(transaction.date, i18n)
            return transaction;
        })

        setState((currentState) => {
            return {...currentState, transactions: fixedTransactions}
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
                <Tooltip />
                <Legend />
                <Line type="monotone" dataKey="price" stroke="#8884d8" activeDot={{ r: 8 }} name=""/>
            </LineChart>
        </ResponsiveContainer>
    </Box>
}

export default SecurityTransactionsChart;