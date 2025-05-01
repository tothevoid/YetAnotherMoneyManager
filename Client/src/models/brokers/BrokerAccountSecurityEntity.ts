import { BrokerAccountEntity } from "./BrokerAccountEntity";
import { SecurityEntity } from "../securities/SecurityEntity";

export interface ServerBrokerAccountSecurityEntity {
    id: string,
    brokerAccountId: string,
    securityId: string,
    quantity: number,
    initialPrice: number,
    currentPrice: number
}

export interface BrokerAccountSecurityEntity extends ServerBrokerAccountSecurityEntity {
    brokerAccount: BrokerAccountEntity,
    security: SecurityEntity,
}
