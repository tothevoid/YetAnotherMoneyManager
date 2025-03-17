import { AccountEntity } from "./AccountEntity";
// import { TransactionType } from "./TransactionType";

export type TransactionEntity = {
    id: string,
    name: string,
    date: Date,
    moneyQuantity: number;
    fundSource: AccountEntity,
    transactionType: string
}