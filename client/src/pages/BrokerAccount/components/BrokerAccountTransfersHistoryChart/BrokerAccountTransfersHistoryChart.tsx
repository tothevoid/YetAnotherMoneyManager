
import React, { useEffect, useMemo, useState } from "react";

import { BrokerAccountDayTransferEntity } from "../../../../models/brokers/BrokerAccountDayTransferEntity";
import { BrokerAccountMonthTransferEntity } from "../../../../models/brokers/BrokerAccountMonthTransferEntity";
import { getMonthTransfersHistory, getYearTransfersHistory } from "../../../../api/brokers/brokerAccountSummaryApi";
import { ResponsiveContainer, BarChart, Bar, XAxis, YAxis, Tooltip, Legend, CartesianGrid } from "recharts";
import { Box, Flex } from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import i18n from "../../../../i18n";
import BaseSelect from "../../../../shared/components/BaseSelect/BaseSelect";

interface Props {
    brokerAccountId: string;
}

const YEAR_RANGE = "YEAR_RANGE";
const MONTH_RANGE = "MONTH_RANGE";

interface RangeType {
    label: string;
    value: string;
}

interface NumericOption {
    label: string
    value: number
}

interface ChartDataItem {
    name: string
    income: number
    withdraw: number
};

const BrokerAccountTransfersHistoryChart: React.FC<Props> = ({ brokerAccountId }) => {
    const { t } = useTranslation();

    const rangeTypes: RangeType[] = useMemo(
        () => [
            { label: "year", value: YEAR_RANGE },
            { label: "month", value: MONTH_RANGE }
        ],
        [i18n.language, t]
    );

    const months: NumericOption[] = useMemo(
        () => Array.from({ length: 12 }, (_, i) => ({
            label: new Date(0, i).toLocaleString("default", { month: "short" }),
            value: i + 1
        })),
        []
    );

    const years: NumericOption[] = useMemo(
        () => Array.from({ length: 5 }, (_, i) => ({
            label: (new Date().getFullYear() - i).toString(),
            value: new Date().getFullYear() - i
        })),
        []
    );

    const [transfers, setTransfers] = useState<Array<BrokerAccountDayTransferEntity | BrokerAccountMonthTransferEntity>>([]);
   
    const [chartData, setChartData] = useState<ChartDataItem[]>([]);

    const [selectedRangeType, selectRangeType] = useState<RangeType>(rangeTypes[0]);
    const [selectedYear, selectYear] = useState<NumericOption>(years[0]);
    const [selectedMonth, selectMonth] = useState<NumericOption>(months[0]);

    useEffect(() => {
        const fetchTransfers = async () => {
            if (selectedRangeType?.value === MONTH_RANGE) {
                const data = await getMonthTransfersHistory(brokerAccountId,
                    selectedMonth.value, selectedYear.value);
                setTransfers(data);
            } else {
                const data = await getYearTransfersHistory(brokerAccountId, 
                    selectedYear.value);
                setTransfers(data);
            }
        };
        fetchTransfers();
    }, [brokerAccountId, selectedMonth, selectedYear, selectedRangeType]);

    useEffect(() => {
        let data: ChartDataItem[] = [];
        if (selectedRangeType?.value === MONTH_RANGE) {
            data = (transfers as BrokerAccountDayTransferEntity[]).map(tr => ({
                name: tr.dayIndex?.toString(),
                income: tr.totalDeposited,
                withdraw: tr.totalWithdrawn
            }));
        } else {
            data = (transfers as BrokerAccountMonthTransferEntity[]).map(tr => ({
                name: new Date(selectedYear.value, tr.monthIndex - 1, 1)
                    .toLocaleString("default", { month: "short" }),
                income: tr.totalDeposited,
                withdraw: tr.totalWithdrawn
            }));
        }
        setChartData(data);
    }, [selectedRangeType, transfers, selectedYear]);
   
    const onRangeSelected = (range: RangeType) => selectRangeType(range);
    const onYearSelected = (year: NumericOption) => selectYear(year);
    const onMonthSelected = (month: NumericOption) => selectMonth(month);

    return (
        <Box style={{ width: '100%', height: 400, marginBlock: 20 }}>
            <Flex mb={4} gap={4} width={550}>
                <BaseSelect placeholder="Select range"
                    selectedValue={selectedRangeType}
                    collection={rangeTypes}
                    onSelected={onRangeSelected}
                    labelSelector={(range => range.label)} 
                    valueSelector={(range => range.value)}/>
                <BaseSelect placeholder="Year"
                    selectedValue={selectedYear}
                    collection={years}
                    onSelected={onYearSelected}
                    labelSelector={(range => range.label)} 
                    valueSelector={(range => range.value)}/>
                {
                    selectedRangeType.value === MONTH_RANGE && <BaseSelect placeholder="Month"
                        selectedValue={selectedMonth}
                        collection={months}
                        onSelected={onMonthSelected}
                        labelSelector={(range => range.label)} 
                        valueSelector={(range => range.value)}/>
                }
            </Flex>
            <ResponsiveContainer>
                <BarChart data={chartData} margin={{ top: 20, right: 30, left: 0, bottom: 5 }}>
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="name" />
                    <YAxis />
                    <Tooltip />
                    <Legend />
                    <Bar dataKey="income" fill="#4CAF50" name={t('Ввод средств')} />
                    <Bar dataKey="withdraw" fill="#F44336" name={t('Вывод средств')} />
                </BarChart>
            </ResponsiveContainer>
        </Box>
    );
};

export default BrokerAccountTransfersHistoryChart;