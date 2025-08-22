import { BrokerAccountEntity, BrokerAccountEntityResponse } from "../brokers/BrokerAccountEntity";
import { SecurityEntity, SecurityEntityResponse } from "./SecurityEntity";

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
    date: Date
}

export interface SecurityTransactionEntity extends CommonSecurityTransactionEntity {
    security: SecurityEntity,
    brokerAccount: BrokerAccountEntity,
    date: Date,
}

export interface SecurityTransactionEntityResponse extends CommonSecurityTransactionEntity {
    security: SecurityEntityResponse,
    brokerAccount: BrokerAccountEntityResponse,
    date: string
}
