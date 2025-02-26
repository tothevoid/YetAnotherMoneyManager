import { CircularProgress, Flex, Stack } from '@chakra-ui/react'
import { useEffect, useState } from 'react';
import { BarChart, Bar, XAxis, YAxis, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import { DepositMonthSummary, DepositPayment } from './depositMonthSummary';
import { getDepositsSummary } from '../../api/depositApi';

const colors = [
    "#ff7c43",
    "#ffa600",
    "#003f5c",
    "#2f4b7c",
    "#665191",
    "#a05195",
    "#d45087",
    "#f95d6a"
]


const DepositStats = () => {
    const [chartData, setChartData] = useState<DepositMonthSummary>(null!);

    useEffect(() => {
        const getData = async () => {
            //TODO: add deposits change rerender
            const data = await getDepositsSummary();
            if (data) {
                setChartData(data)
            }
        }
        getData();
    }, []);

    if (!chartData) {
        return <Flex padding={5} justifyContent="center">
            <CircularProgress isIndeterminate color="purple"></CircularProgress>
        </Flex>
    }

    const data = chartData.payments.map(payment => {
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
            <Tooltip />
            <Legend />
            {
                chartData.deposits.map((deposit: string, index: number) => {
                    //TODO: fix color out of range
                    return <Bar dataKey={deposit} stackId="a" fill={colors[index]}/>
                })
            }
        </BarChart>
        </ResponsiveContainer>
    </Stack>
}

export default DepositStats;