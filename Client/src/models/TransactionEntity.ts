import { AccountEntity } from "./AccountEntity";

export type TransactionEntity = {
    id: string,
    name: string,
    date: Date,
    moneyQuantity: number;
    account: AccountEntity,
    transactionType: string
}