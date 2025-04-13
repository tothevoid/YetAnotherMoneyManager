import { CurrencyEntity } from "./CurrencyEntity";
import { SecurityTypeEntity } from "./SecurityTypeEntity";

export interface SecurityEntity {
    id: string,
    name: string,
    ticker: string,
    type: SecurityTypeEntity,
    typeId: string,
    currency: CurrencyEntity,
    currencyId: string
}