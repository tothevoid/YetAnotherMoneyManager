import { BankEntity } from "../banks/BankEntity"
import { CurrencyEntity } from "../currencies/CurrencyEntity"
import { AccountTypeEntity } from "./AccountTypeEntity"

interface CommonAccount {
    id: string,
    name: string,
    balance: number,
    active: boolean,
}

export interface AccountEntityRequest extends CommonAccount {
    createdOn: string,
    currencyId: string,
    accountTypeId: string
    bankId?: string
}

export interface AccountEntity extends CommonAccount {
    createdOn: Date
    currency: CurrencyEntity,
    accountType: AccountTypeEntity,
    bank?: BankEntity
}

export interface AccountEntityResponse extends CommonAccount {
    createdOn: string
    currency: CurrencyEntity,
    accountType: AccountTypeEntity,
    bank?: BankEntity
}