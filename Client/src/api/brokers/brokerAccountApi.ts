import config from '../../config' 
import { BrokerAccountEntity, BrokerAccountEntityRequest, BrokerAccountEntityResponse } from '../../models/brokers/BrokerAccountEntity';
import { checkPromiseStatus, logPromiseError } from '../../shared/utilities/webApiUtilities';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/BrokerAccount`;

export const getBrokerAccounts = async (): Promise<BrokerAccountEntity[]> => {
    return await getAllEntities<BrokerAccountEntityResponse>(basicUrl)
        .then(brokerAccounts => brokerAccounts.map(prepareBrokerAccount));
};

export const getBrokerAccountById = async (id: string): Promise<BrokerAccountEntity | void> => {
    const brokerAccount: BrokerAccountEntity | void = await fetch(`${basicUrl}/GetById?id=${id}`, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then((brokerAccount: BrokerAccountEntityResponse) => prepareBrokerAccount(brokerAccount))
        .catch(logPromiseError);
    return brokerAccount;
}

export const createBrokerAccount = async (addedBroker: BrokerAccountEntity): Promise<BrokerAccountEntity | void> => {
    return await createEntity<BrokerAccountEntityRequest, BrokerAccountEntityResponse>(basicUrl, prepareBrokerAccountRequest(addedBroker));
}

export const updateBrokerAccount = async (modifiedBroker: BrokerAccountEntity): Promise<boolean> => {
    return await updateEntity(basicUrl, prepareBrokerAccountRequest(modifiedBroker));
}

export const deleteBrokerAccount = async (deleteBrokerAccountId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, deleteBrokerAccountId);
}

const prepareBrokerAccountRequest = (brokerAccount: BrokerAccountEntity): BrokerAccountEntityRequest => {
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

const prepareBrokerAccount = (brokerAccount: BrokerAccountEntityResponse): BrokerAccountEntity => {
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
