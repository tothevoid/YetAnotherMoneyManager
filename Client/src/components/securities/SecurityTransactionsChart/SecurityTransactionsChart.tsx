import { ResponsiveContainer, LineChart, CartesianGrid, XAxis, YAxis, Tooltip, Legend, Line } from "recharts";
import { Box } from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { useEffect, useState } from "react";
import { getTransactionsBySecurity } from "../../../api/securities/securityTransactionApi";
import { SecurityEntity } from "../../../models/securities/SecurityEntity";
import { SecurityTransactionEntity } from "../../../models/securities/SecurityTransactionEntity";

interface Props {
    securityId: string
}

interface State {
    transactions: SecurityTransactionEntity[]
}

const SecurityTransactionsChart: React.FC<Props> = (props) => {
    const { t, i18n } = useTranslation();

    const [state, setState] = useState<State>({transactions: []})

    const initData = async () => {
        const transactions = await getTransactionsBySecurity(props.securityId);
        if (!transactions) {
            return;
        }

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
                <YAxis/>
                <Tooltip />
                <Legend />
                <Line type="monotone" dataKey="price" stroke="#8884d8" activeDot={{ r: 8 }} name=""/>
            </LineChart>
        </ResponsiveContainer>
    </Box>
}

export default SecurityTransactionsChart;