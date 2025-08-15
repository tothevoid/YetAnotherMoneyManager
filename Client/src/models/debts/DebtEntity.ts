import { CurrencyEntity } from "../currencies/CurrencyEntity";

interface CommonDebtEntity {
    id: string,
    name: string,
    amount: number
}

export interface DebtEntityRequest extends CommonDebtEntity {
    currencyId: string
    date: string,
    paidOn: string
}

export interface DebtEntity extends CommonDebtEntity {
    currency: CurrencyEntity,
    date: Date,
    paidOn: Date
}

export interface DebtEntityResponse extends CommonDebtEntity {
    currency: CurrencyEntity,
    date: string,
    paidOn: string
}