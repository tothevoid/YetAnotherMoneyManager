import { BrokerAccountSecurityEntity, BrokerAccountSecurityEntityRequest, BrokerAccountSecurityEntityResponse } from "../../models/brokers/BrokerAccountSecurityEntity";
import { prepareSecurity } from "../securities/securityApiMapping";
import { prepareBrokerAccount } from "./brokerAccountApiMapping";

export const prepareBrokerAccountSecurityRequest = (brokerAccountSecurity: BrokerAccountSecurityEntity): BrokerAccountSecurityEntityRequest => {
    return {
        id: brokerAccountSecurity.id,
        brokerAccountId: brokerAccountSecurity.brokerAccount.id,
        price: brokerAccountSecurity.price,
        quantity: brokerAccountSecurity.quantity,
        securityId: brokerAccountSecurity.security.id
    };
}

export const prepareBrokerAccountSecurity = (brokerAccountSecurity: BrokerAccountSecurityEntityResponse): BrokerAccountSecurityEntity => {
    return {
        id: brokerAccountSecurity.id,
        brokerAccount: prepareBrokerAccount(brokerAccountSecurity.brokerAccount),
        price: brokerAccountSecurity.price,
        quantity: brokerAccountSecurity.quantity,
        security: prepareSecurity(brokerAccountSecurity.security)
    };
}