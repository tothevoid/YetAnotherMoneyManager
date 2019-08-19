import { FundEntity } from "./FundEntity";

export type TransactionEntity = {
    id: string,
    name: string,
    date: string,
    moneyQuantity: number;
    fundSource: FundEntity,
}