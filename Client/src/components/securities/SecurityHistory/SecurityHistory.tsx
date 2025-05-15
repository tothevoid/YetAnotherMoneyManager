import { ResponsiveContainer, LineChart, CartesianGrid, XAxis, YAxis, Tooltip, Legend, Line } from "recharts";
import { Box } from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { useEffect, useState } from "react";
import { getTickerHistory } from "../../../api/securities/securityApi";
import { ClientSecurityHistory } from "../../../models/securities/SecurityHistory";

interface Props {
    ticker: string
}

interface State {
    tickerHistoryValues: ClientSecurityHistory[]
}

const SecurityHistory: React.FC<Props> = (props) => {
    const { t, i18n } = useTranslation();

    const [state, setState] = useState({tickerHistoryValues: []})

    const initData = async () => {
        debugger;
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
                <YAxis/>
                <Tooltip />
                <Legend />
                <Line type="monotone" dataKey="value" stroke="#8884d8" activeDot={{ r: 8 }} name=""/>
            </LineChart>
        </ResponsiveContainer>
    </Box>
}

export default SecurityHistory;