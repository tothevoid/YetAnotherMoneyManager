import { FundEntity } from '../../models/FundEntity'
import { TransactionEntity } from '../../models/TransactionEntity'
import { Button, Flex, Stack } from '@chakra-ui/react'
import { Text } from '@chakra-ui/react';
import { useState } from 'react';
import { Cell, Legend, Pie, PieChart, ResponsiveContainer, Tooltip } from 'recharts';
import { TransactionType } from '../../models/TransactionType';

type Props = {
    funds: FundEntity[]
    transactions: TransactionEntity[],
    transactionTypes: TransactionType[]
}

type PieChartData = {
    name: string;
    value: number;
}

const dataGrouping = {
    BySource: 0,
    ByType: 1
}


const getGraphData = (transactions: TransactionEntity[],
    keySelector: (transaction: TransactionEntity) => string,
    keyMapping: ((key: string) => string) | null = null) => {
    const data: Array<PieChartData> = []

    transactions.reduce(
        (accumulator: Map<string, number>, currentValue: TransactionEntity) => {
            const key = keySelector(currentValue);
            let currentQuantity = currentValue.moneyQuantity;
            if (accumulator.has(key)) {
                currentQuantity += accumulator.get(key);
            }

            accumulator.set(key, currentQuantity);
            return accumulator;
        }, new Map<string, number>(),
    ).forEach((value, key) => {
        const mappedName = keyMapping ? keyMapping(key): key;
        data.push({name: mappedName, value});
    })
    return data;
}

const getPossibleColors = () => {
    return ["#0088FE", "#00C49F", "#FFBB28", "#FF8042"]
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
    const fundsMapping = getNameMapping(props.funds, (fund) => fund.id, (fund) => fund.name );
    const transactionTypeMapping = getNameMapping(props.transactionTypes, 
        (transactionType) => transactionType.id, (transactionType) => transactionType.name );

    const groupingConfig = [
        {
            group: dataGrouping.BySource,
            caption: "By source",
            dataFunc: () => getGraphData(props.transactions, (transaction) => transaction.fundSource.id,
                (key) => fundsMapping.get(key) ?? "Incorrect source")
        },
        {
            group: dataGrouping.ByType,
            caption: "By type",
            dataFunc: () => getGraphData(props.transactions, (transaction) => transaction.transactionType.id,
                (key) => transactionTypeMapping.get(key) ?? "Incorrect type")
        }
    ]

    const [state, setState] = useState({selectedGrouping: dataGrouping.BySource, 
        data: groupingConfig[0].dataFunc()});

    const colors = getPossibleColors();
    return <Stack>
        <Text fontSize="2xl" fontWeight={600}>Stats</Text>
        <Flex gap={4} alignItems={'center'}>
            {
                groupingConfig.map(groupingConfig => {
                    return <Button colorScheme='purple'  disabled={groupingConfig.group == state.selectedGrouping} onClick={() => {
                            setState({data: groupingConfig.dataFunc(), selectedGrouping: groupingConfig.group})
                        }}>
                        {groupingConfig.caption}
                    </Button>
                })
            }
        </Flex>

        <div style={{ width: '100%', height: 300 }}>
            <ResponsiveContainer width="100%" height="100%">
                <PieChart>
                    <Pie data={state.data} cx="50%" cy="50%" outerRadius={100} dataKey="value">
                    {state.data.map((_, index) => (
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