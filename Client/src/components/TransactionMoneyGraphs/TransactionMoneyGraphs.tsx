import { FundEntity } from '../../models/FundEntity'
import { TransactionEntity } from '../../models/TransactionEntity'
import { FrequencyDistributionModel } from '../FrequencyDistribution/FrequencyDistributionModel'
import FrequencyDistribution from '../FrequencyDistribution/FrequencyDistribution'
import {  Stack } from '@chakra-ui/react'
import { Text } from '@chakra-ui/react';

type Props = {
    funds: FundEntity[]
    transactions: TransactionEntity[]
}

// const calculateDistribution = (total: FrequencyDistributionModel[], transaction: TransactionEntity, funds: FundEntity[]) => {
//     if (!transaction || !transaction.fundSource || !transaction.fundSource.id) return total;
//     var freqIndex = total
//         .findIndex((element: FrequencyDistributionModel) => element.id === transaction.fundSource.id);
//     if (freqIndex !== -1){
//         total[freqIndex].value += transaction.moneyQuantity;
//     } else {
//         const fund = (funds as FundEntity[]).find((element: FundEntity) => element.id === transaction.fundSource.id) || {name: ""};
//         let freq: FrequencyDistributionModel = {
//             id: transaction.fundSource.id,
//             name: fund.name,
//             value: Math.abs(transaction.moneyQuantity)
//         };
//         total.push(freq);
//     }
//     return total;
// }

// const getElement = (collection: FrequencyDistributionModel[], title: string) => 
//     (collection && collection.length) ?
//     <Fragment>
//         <h2 className="sub-title">{title}</h2>
//         <FrequencyDistribution collection={collection}></FrequencyDistribution>
//     </Fragment>:
//     null;


const TransactionMoneyGraphs = (props: Props) => {
    // const {transactions, funds} = props;

    // if (!transactions || transactions.length === 0){
    //     return null;
    // }

    // const wrapByContext = (total: FrequencyDistributionModel[], transaction: TransactionEntity) =>
    //     calculateDistribution(total, transaction, funds)
    
    // let gainDistrubution: FrequencyDistributionModel[] = [];
    // let spentsDistrubution: FrequencyDistributionModel[] = [];
    // let moneyFlow: FrequencyDistributionModel[] = [];
    // if (transactions){
    //     spentsDistrubution = transactions.filter((element: TransactionEntity) => element.moneyQuantity < 0)
    //         .reduce(wrapByContext, []);
    //     gainDistrubution = transactions.filter((element: TransactionEntity) => element.moneyQuantity > 0)
    //         .reduce(wrapByContext, []);
    //     const spentValue = spentsDistrubution
    //         .reduce((currentSum: number, spent: FrequencyDistributionModel) => currentSum + Math.abs(spent.value), 0);
    //     const gainValue = gainDistrubution
    //         .reduce((currentSum: number, spent: FrequencyDistributionModel) => currentSum + spent.value, 0);
    //     moneyFlow = [
    //         {id: "1", name: "Got", value: gainValue},
    //         {id: "2", name: "Spent", value: spentValue}];
    // }

    // return <Fragment>
    //     {getElement(moneyFlow, "Month's money change")}
    //     {getElement(gainDistrubution, "Gain distribution")}
    //     {getElement(spentsDistrubution, "Spents distribution")}
    // </Fragment> 

    return <Stack>
        <Text fontSize="2xl" fontWeight={600}>Stats</Text>
    </Stack>
}

export default TransactionMoneyGraphs;