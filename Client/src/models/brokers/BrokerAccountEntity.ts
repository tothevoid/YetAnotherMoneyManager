import { CurrencyEntity } from "../currencies/CurrencyEntity";
import { BrokerAccountTypeEntity } from "./BrokerAccountTypeEntity";
import { BrokerEntity } from "./BrokerEntity";

export interface ServerBrokerAccountEntity {
    id: string,
    name: string,
    lastUpdateAt: Date,
    assetsValue: number,

    typeId: string,
    currencyId: string, 
    brokerId: string,
}

export interface BrokerAccountEntity extends ServerBrokerAccountEntity {
    type: BrokerAccountTypeEntity,
    currency: CurrencyEntity,
    broker: BrokerEntity,
}
