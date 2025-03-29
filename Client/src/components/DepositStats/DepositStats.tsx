import { ProgressCircle, Flex, Stack, Slider } from '@chakra-ui/react'
import { useEffect, useState } from 'react';
import { BarChart, Bar, XAxis, YAxis, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import { DepositMonthSummary, DepositPayment } from './depositMonthSummary';
import { getDepositsRange, getDepositsSummary } from '../../api/depositApi';
import { CHARTS_COLORS } from './chartsColors';
import { DepositsRange } from '../../api/models/depositsRange';

interface State {
    summary: DepositMonthSummary | null,
    minMonths: number | null,
    maxMonths: number | null,
    selectedMinMonths: number | null,
    selectedMaxMonths: number | null,
    marks: {label: string, values: string}[]
}

const convertRange = (range: DepositsRange) => {
    debugger;
    const minDate = new Date(range.from);
    const minDateMonth = minDate.getMonth() + 1;
    const minMonths = minDate.getFullYear() * 12 + minDateMonth;
    const maxDate = new Date(range.to);
    const maxDateMonth =  maxDate.getMonth() + 1;
    const maxMonths = maxDate.getFullYear() * 12 + maxDateMonth;

    //TODO: use culture to display date
    const marks = [
        { value: minMonths, label: `${minDateMonth}-${minDate.getFullYear()}` },
        { value: maxMonths, label: `${maxDateMonth}-${maxDate.getFullYear()}` },
    ]

    debugger;
    return {minMonths, maxMonths, marks, selectedMinMonths: minMonths, selectedMaxMonths: maxMonths};
}

const DepositStats = () => {
    
    const [state, setState] = useState<State>({
        summary: null,
        minMonths: null, 
        maxMonths: null, 
        selectedMinMonths: null, 
        selectedMaxMonths: null, 
        marks: []});

    useEffect(() => {
        const getData = async () => {
            //TODO: add deposits change rerender
            const range = await getDepositsRange();
            if (!range) {
                return;
            }

            const ranges = convertRange(range);
            const summary = await getDepositsSummary();
            if (summary) {
                setState((currentState) => {
                    return {...currentState, ...ranges, summary}
                })
            }
        }
        getData();
    }, []);

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
        {
            <Slider.Root width={"100%"} min={state.minMonths!} max={state.maxMonths!} onChange={(selectedValues) => {debugger;}} 
                defaultValue={[state.selectedMinMonths!, state.selectedMaxMonths!]} step={1}>
                <Slider.Control>
                <Slider.Track>
                    <Slider.Range background={"purple.600"} />
                </Slider.Track>
                <Slider.Thumbs />
                <Slider.Marks marks={state.marks}/>
                </Slider.Control>
            </Slider.Root>
        }
    </Stack>
}

export default DepositStats;