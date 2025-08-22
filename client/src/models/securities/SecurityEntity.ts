import { Nullable } from "../../shared/utilities/nullable";
import { CurrencyEntity } from "../currencies/CurrencyEntity";
import { SecurityTypeEntity } from "./SecurityTypeEntity";

export interface CommonSecurityEntity {
    id: string,
    name: string,
    ticker: string,
    actualPrice: number,
    iconKey: string
}

export interface SecurityEntityRequest extends CommonSecurityEntity {
    typeId: string,
    currencyId: string,
    priceFetchedAt: Nullable<Date>
}

export interface SecurityEntity extends CommonSecurityEntity {
    type: SecurityTypeEntity,
    currency: CurrencyEntity,
    priceFetchedAt: Nullable<Date>
}

export interface SecurityEntityResponse extends CommonSecurityEntity {
    type: SecurityTypeEntity,
    currency: CurrencyEntity,
    priceFetchedAt: Nullable<Date>
}