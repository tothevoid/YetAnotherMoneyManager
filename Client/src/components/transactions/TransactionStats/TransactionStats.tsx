import { AccountEntity } from '../../../models/accounts/AccountEntity'
import { Button, Flex, Stack } from '@chakra-ui/react'
import { Text } from '@chakra-ui/react';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Cell, Legend, Pie, PieChart, ResponsiveContainer, Tooltip } from 'recharts';
import { TransactionEntity } from '../../../models/transactions/TransactionEntity';

type Props = {
    accounts: AccountEntity[]
    transactions: TransactionEntity[],
}

type PieChartData = {
    name: string;
    value: number;
}

type GroupingConfig = {
    group:DataGrouping;
    caption: string;
    dataFunc: (transactions: TransactionEntity[]) => PieChartData[];
}

enum DataGrouping {
    BySource = 0,
    ByType = 1
}


const getGraphData = (transactions: TransactionEntity[],
    keySelector: (transaction: TransactionEntity) => string,
    keyMapping: ((key: string) => string) | null = null) => {
    const data: Array<PieChartData> = []

    transactions
        .filter(transaction => transaction.moneyQuantity < 0)
        .reduce(
        (accumulator: Map<string, number>, currentValue: TransactionEntity) => {
            const key = keySelector(currentValue);
            let currentQuantity = currentValue.moneyQuantity;
            if (accumulator.has(key)) {
                currentQuantity += accumulator.get(key) ?? 0;
            }

            accumulator.set(key, currentQuantity);
            return accumulator;
        }, new Map<string, number>(),
    ).forEach((value, key) => {
        const mappedName = keyMapping ? keyMapping(key): key;
        data.push({name: mappedName, value: Math.abs(value)});
    })
    return data;
}

const getPossibleColors = () => {
    return [
        "#FF6347", "#2E8B57", "#D2691E",
        "#DCDCDC", "#A52A2A", "#5F9EA0", "#FF4500",
        "#6A5ACD", "#C71585", "#4682B4", "#8B4513",
        "#B8860B", "#20B2AA", "#FF8C00", "#6B8E23", "#483D8B"
    ]
}

const getNameMapping = <CollectionType,>(
    collection: Array<CollectionType>,
    keySelector: (item: CollectionType) => string,
    valueSelector: (item: CollectionType) => string) => {
    return collection.reduce((accumulator, item) => {
        accumulator.set(keySelector(item), valueSelector(item));
        return accumulator;
    }, new Map<string, string>());
};

const TransactionStats = (props: Props) => {
    const accountsMapping = getNameMapping(props.accounts, (account) => account.id, (account) => account.name );
    const [selectedGrouping, setSelectedGrouping] = useState(DataGrouping.BySource);
    const [chartData, setChartData] = useState([] as PieChartData[]);
    const { t } = useTranslation();

    const groupingConfig = new Map<DataGrouping, GroupingConfig>([
        [ DataGrouping.BySource, {
            group: DataGrouping.BySource,
            caption: t("manager_stats_by_source"),
            dataFunc: (transactions: TransactionEntity[]) => getGraphData(transactions, (transaction) => transaction.account.id,
                (key) => accountsMapping.get(key) ?? "Incorrect source")
        }],
        [ DataGrouping.ByType, {
            group: DataGrouping.ByType,
            caption: t("manager_stats_by_type"),
            dataFunc: (transactions: TransactionEntity[]) => getGraphData(transactions, (transaction) => transaction.transactionType.name)
        }]
    ]);

    useEffect(() => {
        const data = groupingConfig.get(selectedGrouping)?.dataFunc(props.transactions) ?? [];
        setChartData(data);
    }, [props.transactions, selectedGrouping]);


    const colors = getPossibleColors();

    return <Stack>
        <Text fontSize="2xl" fontWeight={600}>{t("manager_stats_title")}</Text>
        <Flex gap={4} alignItems={'center'}>
            {
                [...groupingConfig.values()].map(groupingConfig => {
                    return <Button key={groupingConfig.group} background='purple.600' disabled={groupingConfig.group == selectedGrouping} 
                        onClick={() => {setSelectedGrouping(groupingConfig.group)}}>
                        {groupingConfig.caption}
                    </Button>
                })
            }
        </Flex>

        <div style={{ width: '100%', height: 300 }}>
            <ResponsiveContainer width="100%" height="100%">
                <PieChart>
                    <Pie data={chartData} cx="50%" cy="50%" outerRadius={100} dataKey="value">
                    {chartData.map((_, index) => (
                        <Cell key={`cell-${index}`} fill={colors[index % colors.length]} />
                        ))}
                    </Pie>
                    <Tooltip />
                    <Legend/>
                </PieChart>
            </ResponsiveContainer>
        </div>
    </Stack>
}

export default TransactionStats;