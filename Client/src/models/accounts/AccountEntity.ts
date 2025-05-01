import { CurrencyEntity } from "../currencies/CurrencyEntity"
import { AccountTypeEntity } from "./AccountTypeEntity"

interface AccountCommon {
    id: string,
    name: string,
    balance: number,
    active: boolean,
}

export interface AccountEntity extends AccountCommon {
    createdOn: Date
    currency: CurrencyEntity,
    accountType: AccountTypeEntity
}

export interface ServerAccountEntity extends AccountCommon {
    createdOn: string,
    currencyId: string,
    accountTypeId: string
}