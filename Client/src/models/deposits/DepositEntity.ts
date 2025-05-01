import { AccountEntity } from "../accounts/AccountEntity"

interface CommonDepositEntity {
    id: string,
    name: string,
    initialAmount: number,
    estimatedEarn?: number | null,
    percentage: number,
}

export interface ServerDepositEntity extends CommonDepositEntity {
    from: string,
    to: string,
    accountId?: string | null
}

export interface DepositEntity extends CommonDepositEntity {
    from: Date,
    to: Date,
    account?: AccountEntity | null
}