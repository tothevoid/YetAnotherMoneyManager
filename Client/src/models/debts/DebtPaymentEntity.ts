import { AccountEntity } from "../accounts/AccountEntity";
import { TransactionEntity } from "../transactions/TransactionEntity";
import { ClientDebtEntity } from "./DebtEntity";

export interface CommonDebtPaymentEntity {
    id: string,
    amount: number
}

export interface ClientDebtPaymentEntity extends CommonDebtPaymentEntity {
    targetAccount: AccountEntity,
    transaction: TransactionEntity,
    debt: ClientDebtEntity,
    date: Date
}

export interface ServerDebtPaymentEntity extends CommonDebtPaymentEntity {
    transactionId: string,
    targetAccountId: string,
    debtId: string,
    date: string
}