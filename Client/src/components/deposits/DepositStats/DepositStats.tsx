import { ProgressCircle, Flex, Box, Stack, Button } from '@chakra-ui/react'
import { useEffect, useState } from 'react';
import { DepositMonthSummary } from './depositMonthSummary';
import { getDepositsSummary } from '../../../api/deposits/depositApi';
import StackedDepositsChart from '../StackedDepositsChart/StackedDepositsChart';
import DepositsEarningsChart from '../DepositsEarningsChart/DepositsEarningsChart';
import { useTranslation } from 'react-i18next';

enum ChartType {
    Earnings,
    Stacked
}

interface Props {
    selectedMinMonths: number,
    selectedMaxMonths: number,
    onlyActive: boolean
}

interface State {
    summary: DepositMonthSummary | null,
    selectedChartType : ChartType
}

const DepositStats = (props: Props) => {
    const [state, setState] = useState<State>({
        summary: null,
        selectedChartType: ChartType.Earnings
    });
    const { t } = useTranslation();

    useEffect(() => {
        const getData = async () => {
            if (!props.selectedMinMonths || !props.selectedMaxMonths) {
                return;
            }

            const summary = await getDepositsSummary(props.selectedMinMonths, props.selectedMaxMonths, props.onlyActive);
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

    const switchActiveChart = (newType: ChartType) => {
        setState(currentState => {
            return {...currentState, selectedChartType: newType};
        })
    }

    return <Box paddingTop={10}>
        <Stack direction="row">
            <Button background='purple.600' disabled={state.selectedChartType === ChartType.Earnings} 
                onClick={() => {switchActiveChart(ChartType.Earnings)}}>
                {t("deposits_chart_type_earnings")}
            </Button>
            <Button background='purple.600' disabled={state.selectedChartType === ChartType.Stacked} 
                onClick={() => {switchActiveChart(ChartType.Stacked)}}>
                {t("deposits_chart_type_stacked")}
            </Button>
        </Stack>
        {
            state.selectedChartType === ChartType.Stacked ?
                <StackedDepositsChart data={state.summary} /> :
                <DepositsEarningsChart data={state.summary} />
        }
    </Box>
}

export default DepositStats;