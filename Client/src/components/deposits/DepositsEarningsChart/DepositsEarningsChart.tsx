import { ResponsiveContainer, LineChart, CartesianGrid, XAxis, YAxis, Tooltip, Legend, Line } from "recharts";
import { DepositMonthSummary, DepositPayment } from "../DepositStats/depositMonthSummary";
import { Box } from "@chakra-ui/react";
import { useTranslation } from "react-i18next";

interface Props {
	data: DepositMonthSummary
}

const DepositsEarningsChart = (props: Props) => {
	const { t } = useTranslation();

	const data = props.data.payments.map(payment => {
		const sum = payment.payments.reduce((accumulator: number, currentValue: DepositPayment) => {
			return accumulator + currentValue.value;
		}, 0);
		return { date: payment.period, value: sum};
	})

	return <Box style={{ width: '100%', height: 400 }}>
		<ResponsiveContainer>
			<LineChart data={data}
				margin={{
					top: 20,
					right: 30,
					left: 30,
					bottom: 20,
				}}>
				<CartesianGrid strokeDasharray="3 3" />
				<XAxis dataKey="date"/>
				<YAxis/>
				<Tooltip />
				<Legend />
				<Line type="monotone" dataKey="value" stroke="#8884d8" activeDot={{ r: 8 }} name={t("earnings_chart_data_title")}/>
			</LineChart>
		</ResponsiveContainer>
	</Box>
}

export default DepositsEarningsChart;