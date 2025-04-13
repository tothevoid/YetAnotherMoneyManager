import { BrokerAccountEntity } from "./BrokerAccountEntity";
import { SecurityEntity } from "../securities/SecurityEntity";

export interface BrokerAccountSecurityEntity {
    id: string,
    brokerAccount: BrokerAccountEntity,
    brokerAccountId: string,
    security: SecurityEntity,
    securityId: string,
    quantity: number,
    initialPrice: number,
    currentPrice: number
}
