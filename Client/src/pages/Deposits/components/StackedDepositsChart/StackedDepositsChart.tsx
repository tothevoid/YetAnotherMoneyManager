import { Stack } from "@chakra-ui/react"
import { Bar, BarChart, Legend, ResponsiveContainer, Tooltip, XAxis, YAxis } from "recharts"
import { CHARTS_COLORS } from "../DepositStats/chartsColors"
import { DepositMonthSummary } from "../DepositStats/depositMonthSummary"
import { getChartLabelConfig } from "../../../../shared/utilities/chartUtilities"
import { formatMoneyByCurrencyCulture } from "../../../../shared/utilities/formatters/moneyFormatter"
import { formatMonth } from "../../../../shared/utilities/formatters/dateFormatter"
import { useTranslation } from "react-i18next"

interface Props {
	data: DepositMonthSummary,
	currencyName: string
}

interface ChartBar {
	[key: string]: number,
}

type ChartBarData = ChartBar & {
    date: string
}


const StackedDepositsChart = (props: Props) => {
	const { i18n } = useTranslation();

	const deposits = new Map<string, string>();

	const data = props.data.payments.map(payment => {
		const output: ChartBarData = { date: payment.period };
		
		payment.payments.forEach((payment) => {
			output[payment.depositId] = payment.value;

			if (!deposits.has(payment.depositId)) {
				deposits.set(payment.depositId, payment.name);
			}
		});

		return output;
	})

	const getDepositTitle = (value: string) => deposits.get(value) ?? "";

	const getMonthTitle = (value: string) => {
		const [month, year] = value.split(".");
		return `${formatMonth(Number.parseInt(month), i18n)} ${year}`
	}

	return <Stack>
		<ResponsiveContainer width="100%" height={300}>
		<BarChart data={data} margin={{ top: 20, right: 30, left: 20, bottom: 5 }}>
			<XAxis dataKey="date" />
			<YAxis />
			<Tooltip labelFormatter={getMonthTitle}  contentStyle={getChartLabelConfig()} formatter={(value, name) => [formatMoneyByCurrencyCulture(value, props.currencyName), getDepositTitle(name)]} />
			<Legend formatter={getDepositTitle} />
			{
				[...deposits.keys()].map((deposit: string, index: number) => {
					//TODO: fix color out of range
					return <Bar  key={deposit} dataKey={deposit} stackId="a" fill={CHARTS_COLORS[index]}/>
				})
			}
		</BarChart>
		</ResponsiveContainer>
	</Stack>

}

export default StackedDepositsChart;