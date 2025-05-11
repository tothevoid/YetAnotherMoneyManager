import { BrokerAccountEntity } from "../brokers/BrokerAccountEntity";
import { SecurityEntity } from "./SecurityEntity";

interface CommonSecurityTransactionEntity {
    id: string,
    quantity: number,
    price: number,
    brokerCommission: number,
    stockExchangeCommission: number,
    tax: number,
    date: Date
}

export interface ServerSecurityTransactionEntity extends CommonSecurityTransactionEntity {
    securityId: string,
    brokerAccountId: string,
}

export interface SecurityTransactionEntity extends CommonSecurityTransactionEntity {
    security: SecurityEntity,
    brokerAccount: BrokerAccountEntity,
}
