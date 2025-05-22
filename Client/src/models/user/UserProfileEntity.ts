import { CurrencyEntity } from "../currencies/CurrencyEntity"

export interface UserProfileEntity{
    id: string,
    currencyId: string,
    currency: CurrencyEntity,
    languageCode: string
}