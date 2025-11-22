import { ResponsiveContainer, LineChart, CartesianGrid, XAxis, YAxis, Tooltip, Legend, Line } from "recharts";
import { Box } from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { useEffect, useState } from "react";
import { getTickerHistory } from "../../../../api/securities/securityApi";
import { SecurityHistoryValue } from "../../../../models/securities/SecurityHistoryValue";
import { formatMoneyByCurrencyCulture } from "../../../../shared/utilities/formatters/moneyFormatter";
import { getChartLabelConfig } from "../../../../shared/utilities/chartUtilities";

interface Props {
    ticker: string
    currencyName: string
}

interface State {
    tickerHistoryValues: SecurityHistoryValue[]
}

const SecurityHistory: React.FC<Props> = (props) => {
    const { t, i18n } = useTranslation();

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
                <Tooltip contentStyle={getChartLabelConfig()} formatter={(value: number) => formatMoneyByCurrencyCulture(value, props.currencyName)} />
                <Legend />
                <Line type="monotone" dataKey="value" stroke="#16a34a" activeDot={{ r: 8 }} name={t("security_history_chart_value_name")}/>
            </LineChart>
        </ResponsiveContainer>
    </Box>
}

export default SecurityHistory;