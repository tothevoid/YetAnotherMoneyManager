import { BrokerAccountSecurityEntity, BrokerAccountSecurityEntityRequest, BrokerAccountSecurityEntityResponse } from "../../models/brokers/BrokerAccountSecurityEntity";

export const prepareBrokerAccountSecurityResponse = (brokerAccountSecurity: BrokerAccountSecurityEntity): BrokerAccountSecurityEntityRequest => {
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
        brokerAccount: brokerAccountSecurity.brokerAccount,
        price: brokerAccountSecurity.price,
        quantity: brokerAccountSecurity.quantity,
        security: brokerAccountSecurity.security
    };
}