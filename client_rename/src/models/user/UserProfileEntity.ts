import { CurrencyEntity } from "../currencies/CurrencyEntity"

export interface CommonUserProfileEntity {
    id: string,
    languageCode: string
}

export interface UserProfileEntityRequest extends CommonUserProfileEntity {
    currencyId: string,
}

export interface UserProfileEntity extends CommonUserProfileEntity {
    currency: CurrencyEntity,
}

export interface UserProfileEntityResponse extends CommonUserProfileEntity {
    currency: CurrencyEntity,
}