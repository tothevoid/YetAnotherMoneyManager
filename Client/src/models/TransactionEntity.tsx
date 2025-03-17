import { AccountEntity } from "./AccountEntity";
// import { TransactionType } from "./TransactionType";

export type TransactionEntity = {
    id: string,
    name: string,
    date: Date,
    moneyQuantity: number;
    account: AccountEntity,
    transactionType: string
}