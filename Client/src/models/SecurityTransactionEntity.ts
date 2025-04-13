import { BrokerAccountEntity } from "./BrokerAccountEntity";
import { SecurityEntity } from "./SecurityEntity";

export interface SecurityTransactionEntity {
    id: string,
    security: SecurityEntity,
    securityId: string,
    brokerAccount: BrokerAccountEntity,
    brokerAccountId: string,
    quantity: number,
    price: number,
    date: string,
    commission: number,
    tax: number
}
