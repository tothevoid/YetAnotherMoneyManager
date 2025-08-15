import { BrokerAccountEntity } from "../brokers/BrokerAccountEntity";
import { SecurityEntity } from "./SecurityEntity";

interface CommonSecurityTransactionEntity {
    id: string,
    quantity: number,
    price: number,
    brokerCommission: number,
    stockExchangeCommission: number,
    tax: number
}

export interface SecurityTransactionEntityRequest extends CommonSecurityTransactionEntity {
    securityId: string,
    brokerAccountId: string,
    date: string
}

export interface SecurityTransactionEntity extends CommonSecurityTransactionEntity {
    security: SecurityEntity,
    brokerAccount: BrokerAccountEntity,
    date: Date,
}

export interface SecurityTransactionEntityResponse extends CommonSecurityTransactionEntity {
    security: SecurityEntity,
    brokerAccount: BrokerAccountEntity,
    date: string
}
