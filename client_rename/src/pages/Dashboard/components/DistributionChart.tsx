import { Stack } from '@chakra-ui/react'
import { Cell, Legend, Pie, PieChart, ResponsiveContainer, Tooltip } from 'recharts';
import { DistributionModel } from '../../../models/dashboard/DashboardEntity';
import { getChartLabelConfig } from '../../../shared/utilities/chartUtilities';
import { formatMoneyByCurrencyCulture } from '../../../shared/utilities/formatters/moneyFormatter';
import { Payload } from 'recharts/types/component/DefaultTooltipContent';

const getPossibleColors = () => {
    return [
        "#FF6347", "#2E8B57", "#D2691E",
        "#DCDCDC", "#A52A2A", "#5F9EA0", "#FF4500",
        "#6A5ACD", "#C71585", "#4682B4", "#8B4513",
        "#B8860B", "#20B2AA", "#FF8C00", "#6B8E23", "#483D8B"
    ]
}


type Props = {
    data: DistributionModel[]
    mainCurrency: string
}

const DistributionChart = (props: Props) => {
    const colors = getPossibleColors();

    const formatLabel = (_value: number, _key: string, data: Payload<number, string>) => {
        const {currency, amount, convertedAmount} = data.payload as DistributionModel;

        const convertedValue = formatMoneyByCurrencyCulture(convertedAmount, props.mainCurrency);

        return amount !== convertedAmount ? 
            `${formatMoneyByCurrencyCulture(amount, currency)} (${convertedValue})`:
            convertedValue;
    }

    return <Stack>
        <div style={{ width: '100%', height: 400 }}>
            <ResponsiveContainer width="100%" height="100%">
                <PieChart>
                    <Pie data={props.data} cx="50%" cy="50%" outerRadius={100} dataKey="convertedAmount">
                    {
                        props.data.map((_, index) => (
                            <Cell key={`cell-${index}`} fill={colors[index % colors.length]} />
                        )) 
                    }
                    </Pie>
                    <Tooltip contentStyle={getChartLabelConfig()} itemStyle={{ color: "#E0E0E0" }} formatter={formatLabel}/>
                    <Legend
                        formatter={(value, _, index) => {
                            const total = props.data.reduce((sum, item) => sum + item.convertedAmount, 0);
                            const percent = ((props.data[index].convertedAmount / total) * 100).toFixed(1);
                            return `${value} (${percent}%)`;
                        }}
                    />
                </PieChart>
            </ResponsiveContainer>
        </div>
    </Stack>
}

export default DistributionChart;