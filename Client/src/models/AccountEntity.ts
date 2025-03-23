import { AccountTypeEntity } from "./AccountTypeEntity"
import { CurrencyEntity } from "./CurrencyEntity"

interface AccountCommon {
    id: string,
    name: string,
    balance: number,
    currency: CurrencyEntity,
    accountType: AccountTypeEntity,
    active: boolean,
}

export interface AccountEntity extends AccountCommon {
    createdOn: Date
}

export interface ServerAccountEntity extends AccountCommon {
    createdOn: string
}