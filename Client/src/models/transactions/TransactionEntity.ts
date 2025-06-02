import { AccountEntity } from "../accounts/AccountEntity";
import { TransactionTypeEntity } from "./TransactionTypeEntity";

interface CommonTransactionEntity {
    id: string,
    name: string,
    amount: number;
    cashback: number,
    isSystem: boolean
}

export interface ServerTransactionEntity extends CommonTransactionEntity {
    date: string,
    accountId: string,
    transactionTypeId: string
}

export interface TransactionEntity extends CommonTransactionEntity {
    date: Date,
    account: AccountEntity,
    transactionType: TransactionTypeEntity,
}