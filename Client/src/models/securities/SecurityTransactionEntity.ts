import { BrokerAccountEntity } from "../brokers/BrokerAccountEntity";
import { SecurityEntity } from "./SecurityEntity";

export interface SecurityTransactionEntity {
    id: string,
    security: SecurityEntity,
    securityId: string,
    brokerAccount: BrokerAccountEntity,
    brokerAccountId: string,
    quantity: number,
    price: number,
    date: Date,
    commission: number,
    tax: number
}
