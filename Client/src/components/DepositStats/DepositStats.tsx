import { ProgressCircle, Flex, Stack, Slider } from '@chakra-ui/react'
import { useEffect, useState } from 'react';
import { BarChart, Bar, XAxis, YAxis, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import { DepositMonthSummary, DepositPayment } from './depositMonthSummary';
import { getDepositsSummary } from '../../api/depositApi';
import { CHARTS_COLORS } from './chartsColors';

interface Props {
    selectedMinMonths: number,
    selectedMaxMonths: number,
}

interface State {
    summary: DepositMonthSummary | null,
}

const DepositStats = (props: Props) => {
    const [state, setState] = useState<State>({
        summary: null});

    useEffect(() => {
        const getData = async () => {
            if (!props.selectedMinMonths || !props.selectedMaxMonths) {
                return;
            }

            const summary = await getDepositsSummary(props.selectedMinMonths, props.selectedMaxMonths);
            if (summary) {
                setState((currentState) => {
                    return {...currentState, summary }
                })
            }
        }
        getData();
    }, [props.selectedMinMonths, props.selectedMaxMonths]);

    if (!state.summary) {
        return <Flex padding={5} justifyContent="center">
            <ProgressCircle.Root color="purple">
                <ProgressCircle.Circle>
                    <ProgressCircle.Track />
                    <ProgressCircle.Range />
                </ProgressCircle.Circle>
            </ProgressCircle.Root>
        </Flex>
    }

    const data = state.summary.payments.map(payment => {
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
                state.summary.deposits.map((deposit: string, index: number) => {
                    //TODO: fix color out of range
                    return <Bar dataKey={deposit} stackId="a" fill={CHARTS_COLORS[index]}/>
                })
            }
        </BarChart>
        </ResponsiveContainer>
    </Stack>
}

export default DepositStats;