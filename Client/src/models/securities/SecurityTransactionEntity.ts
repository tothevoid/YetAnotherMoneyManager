import { BrokerAccountEntity } from "../brokers/BrokerAccountEntity";
import { SecurityEntity } from "./SecurityEntity";

interface CommonSecurityTransactionEntity {
    id: string,
    quantity: number,
    price: number,
    commission: number,
    tax: number
}

export interface ServerSecurityTransactionEntity extends CommonSecurityTransactionEntity {
    date: string,
    securityId: string,
    brokerAccountId: string,
}

export interface SecurityTransactionEntity extends CommonSecurityTransactionEntity {
    date: Date,
    security: SecurityEntity,
    brokerAccount: BrokerAccountEntity,
}
