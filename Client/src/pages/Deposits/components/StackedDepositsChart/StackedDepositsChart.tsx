import { Stack } from "@chakra-ui/react"
import { Bar, BarChart, Legend, ResponsiveContainer, Tooltip, XAxis, YAxis } from "recharts"
import { CHARTS_COLORS } from "../DepositStats/chartsColors"
import { DepositMonthSummary, DepositPayment } from "../DepositStats/depositMonthSummary"
import { getChartLabelConfig } from "../../../../shared/utilities/chartUtilities"
import { formatMoneyByCurrencyCulture } from "../../../../shared/utilities/formatters/moneyFormatter"

interface Props {
    data: DepositMonthSummary,
    currencyName: string
}

const StackedDepositsChart = (props: Props) => {
    const data = props.data.payments.map(payment => {
        const payments = payment.payments.reduce((accumulator: Map<string, number>, currentValue: DepositPayment) => {
            if (currentValue?.name && currentValue.value) {
                accumulator.set(currentValue.name, currentValue.value);
            }
            return accumulator;
        }, new Map<string, number>());
        return { date: payment.period, ...Object.fromEntries(payments)};
    })

    return <Stack>
        <ResponsiveContainer width="100%" height={300}>
        <BarChart data={data} margin={{ top: 20, right: 30, left: 20, bottom: 5 }}>
            <XAxis dataKey="date" />
            <YAxis />
            <Tooltip contentStyle={getChartLabelConfig()} formatter={(value, name) => [formatMoneyByCurrencyCulture(value, props.currencyName), name]} />
            <Legend />
            {
                props.data.deposits.map((deposit: string, index: number) => {
                    //TODO: fix color out of range
                    return <Bar dataKey={deposit} stackId="a" fill={CHARTS_COLORS[index]}/>
                })
            }
        </BarChart>
        </ResponsiveContainer>
    </Stack>

}

export default StackedDepositsChart;