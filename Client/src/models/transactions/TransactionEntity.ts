import { AccountEntity } from "../accounts/AccountEntity";

interface CommonTransactionEntity {
    id: string,
    name: string,
    moneyQuantity: number;
    cashback: number,
    transactionType: string,
    isSystem: boolean
}

export interface ServerTransactionEntity extends CommonTransactionEntity {
    date: string,
    accountId: string
}

export interface TransactionEntity extends CommonTransactionEntity {
    date: Date,
    account: AccountEntity,
}