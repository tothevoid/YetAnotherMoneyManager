import { BrokerAccountEntity, BrokerAccountEntityRequest, BrokerAccountEntityResponse } from "../../models/brokers/BrokerAccountEntity";

export const prepareBrokerAccountRequest = (brokerAccount: BrokerAccountEntity): BrokerAccountEntityRequest => {
    return {
        id: brokerAccount.id,
        name: brokerAccount.name,
        typeId: brokerAccount.type.id,
        currencyId: brokerAccount.currency.id,
        brokerId: brokerAccount.broker.id,
        initialValue: brokerAccount.initialValue,
        currentValue: brokerAccount.currentValue,
        mainCurrencyAmount: brokerAccount.mainCurrencyAmount
    };
}

export const prepareBrokerAccount = (brokerAccount: BrokerAccountEntityResponse): BrokerAccountEntity => {
    return {
        id: brokerAccount.id,
        name: brokerAccount.name,
        type: brokerAccount.type,
        currency: brokerAccount.currency,
        broker: brokerAccount.broker,
        initialValue: brokerAccount.initialValue,
        currentValue: brokerAccount.currentValue,
        mainCurrencyAmount: brokerAccount.mainCurrencyAmount
    };
}
