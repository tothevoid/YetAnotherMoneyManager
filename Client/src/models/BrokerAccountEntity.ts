import { Currency } from "../formatters/moneyFormatter";
import { BrokerAccountTypeEntity } from "./BrokerAccountTypeEntity";
import { BrokerEntity } from "./BrokerEntity";

export interface BrokerAccountEntity {
    id: string,
    name: string,
    type: BrokerAccountTypeEntity,
    typeId: string,
    currency: Currency,
    currencyId: string, 
    broker: BrokerEntity,
    brokerId: string
}