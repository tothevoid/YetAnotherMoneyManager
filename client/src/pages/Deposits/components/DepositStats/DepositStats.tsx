import { ProgressCircle, Flex, Box } from '@chakra-ui/react'
import { Fragment, useEffect, useState } from 'react';
import { DepositMonthSummary } from './depositMonthSummary';
import { getDepositsSummary } from '../../../../api/deposits/depositApi';
import StackedDepositsChart from '../StackedDepositsChart/StackedDepositsChart';
import DepositsEarningsChart from '../DepositsEarningsChart/DepositsEarningsChart';
import { useTranslation } from 'react-i18next';
import { useUserProfile } from '../../../../../features/UserProfileSettingsModal/hooks/UserProfileContext';
import ButtonGroup, { ButtonGroupOption } from '../../../../shared/components/ButtonGroup/ButtonGroup';

enum ChartType {
	Earnings = 0,
	Stacked = 1
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
	const { user } = useUserProfile();

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
	}, [props.selectedMinMonths, props.selectedMaxMonths, props.onlyActive]);

	if (!state.summary) {
		return <Flex padding={5} justifyContent="center">
			<ProgressCircle.Root color="spinner_primary">
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

	if (!user?.currency?.name) {
		return <Fragment/>
	}

	const chartTypeOptions: ButtonGroupOption<ChartType>[] = [
		{ value: ChartType.Earnings, label: t("deposits_chart_type_earnings") },
		{ value: ChartType.Stacked, label: t("deposits_chart_type_stacked") },
	];

	return (
		<Box>
			<Box mb={4}>
				<ButtonGroup<ChartType>
					options={chartTypeOptions}
					value={state.selectedChartType}
					onChange={switchActiveChart}
				/>
			</Box>
			{state.selectedChartType === ChartType.Stacked ? (
				<StackedDepositsChart currencyName={user?.currency.name} data={state.summary} />
			) : (
				<DepositsEarningsChart currencyName={user?.currency.name} data={state.summary} />
			)}
		</Box>
	);
}

export default DepositStats;