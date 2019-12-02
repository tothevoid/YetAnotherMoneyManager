import { FundEntity } from "./FundEntity";
import { TransactionType } from "./TransactionType";

export type TransactionEntity = {
    id: string,
    name: string,
    date: string,
    moneyQuantity: number;
    fundSource: FundEntity,
    transactionType: TransactionType
}