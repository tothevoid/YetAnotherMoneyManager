import { CurrencyEntity } from "../currencies/CurrencyEntity"

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
    currencyId: string
}

export interface DepositEntity extends CommonDepositEntity {
    from: Date,
    to: Date,
    currency: CurrencyEntity
}