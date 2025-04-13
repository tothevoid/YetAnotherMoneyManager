import { AccountEntity } from "../accounts/AccountEntity";

export type TransactionEntity = {
    id: string,
    name: string,
    date: Date,
    moneyQuantity: number;
    cashback: number,
    account: AccountEntity,
    transactionType: string,
    isSystem: boolean
}