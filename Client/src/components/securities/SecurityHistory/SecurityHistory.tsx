import { ResponsiveContainer, LineChart, CartesianGrid, XAxis, YAxis, Tooltip, Legend, Line } from "recharts";
import { Box } from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { useEffect, useState } from "react";
import { getTickerHistory } from "../../../api/securities/securityApi";
import { SecurityHistoryValue } from "../../../models/securities/SecurityHistoryValue";

interface Props {
    ticker: string
}

interface State {
    tickerHistoryValues: SecurityHistoryValue[]
}

const SecurityHistory: React.FC<Props> = (props) => {
    const { i18n } = useTranslation();

    const [state, setState] = useState<State>({tickerHistoryValues: []})

    const initData = async () => {
        const tickerHistoryValues = await getTickerHistory(props.ticker, i18n);
        if (!tickerHistoryValues) {
            return;
        }

        setState((currentState) => {
            return {...currentState, tickerHistoryValues}
        })
    }

    useEffect(() => {
        initData();
    }, []);

    return <Box style={{ width: '100%', height: 400 }}>
        <ResponsiveContainer>
            <LineChart data={state.tickerHistoryValues}
                margin={{
                    top: 20,
                    bottom: 20,
                }}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="date"/>
                <YAxis domain={['dataMin - 10', 'dataMax + 10']}/>
                <Tooltip />
                <Legend />
                <Line type="monotone" dataKey="value" stroke="#8884d8" activeDot={{ r: 8 }} name=""/>
            </LineChart>
        </ResponsiveContainer>
    </Box>
}

export default SecurityHistory;