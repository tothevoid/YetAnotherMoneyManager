import { AccountEntity } from "../accounts/AccountEntity";
import { ClientDebtEntity } from "./DebtEntity";

export interface CommonDebtPaymentEntity {
    id: string,
    amount: number
}

export interface ClientDebtPaymentEntity extends CommonDebtPaymentEntity {
    targetAccount: AccountEntity,
    debt: ClientDebtEntity,
    date: Date
}

export interface ServerDebtPaymentEntity extends CommonDebtPaymentEntity {
    targetAccountId: string,
    debtId: string,
    date: string
}