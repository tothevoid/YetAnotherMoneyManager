import { FundEntity } from "./FundEntity";
import { TransactionType } from "./TransactionType";

export type TransactionEntity = {
    id: string,
    name: string,
    date: Date,
    moneyQuantity: number;
    fundSource: FundEntity,
    transactionType: TransactionType
}