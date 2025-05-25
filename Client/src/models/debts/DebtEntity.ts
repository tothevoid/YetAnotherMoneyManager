import { CurrencyEntity } from "../currencies/CurrencyEntity";

interface CommonDebtEntity {
    id: string,
    name: string,
    amount: number
}

export interface ClientDebtEntity extends CommonDebtEntity {
    currency: CurrencyEntity,
    date: Date,
    paidOn: Date
}

export interface ServerDebtEntity extends CommonDebtEntity {
    currencyId: string
    date: string,
    paidOn: string
}