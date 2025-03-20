import { CurrencyEntity } from "./CurrencyEntity"

export type AccountEntity = {
    id: string,
    name: string,
    balance: number,
    currency: CurrencyEntity
}