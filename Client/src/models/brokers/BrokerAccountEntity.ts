import { CurrencyEntity } from "../currencies/CurrencyEntity";
import { BrokerAccountTypeEntity } from "./BrokerAccountTypeEntity";
import { BrokerEntity } from "./BrokerEntity";

export interface BrokerAccountEntity {
    id: string,
    name: string,
    type: BrokerAccountTypeEntity,
    typeId: string,
    currency: CurrencyEntity,
    currencyId: string, 
    broker: BrokerEntity,
    brokerId: string,
    lastUpdateAt: Date,
    assetValue: number
}