import { BrokerAccountEntity } from "./BrokerAccountEntity";
import { SecurityEntity } from "../securities/SecurityEntity";

export interface CommonBrokerAccountSecurityEntity {
    id: string,
    quantity: number,
    price: number
}

export interface BrokerAccountSecurityEntityRequest extends CommonBrokerAccountSecurityEntity {
    brokerAccountId: string,
    securityId: string,
}

export interface BrokerAccountSecurityEntity extends CommonBrokerAccountSecurityEntity {
    brokerAccount: BrokerAccountEntity,
    security: SecurityEntity,
}

export interface BrokerAccountSecurityEntityResponse extends CommonBrokerAccountSecurityEntity {
    brokerAccount: BrokerAccountEntity,
    security: SecurityEntity,
}