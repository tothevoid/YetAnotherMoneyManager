import config from '../../config' 
import { BrokerAccountEntity, ServerBrokerAccountEntity } from '../../models/brokers/BrokerAccountEntity';
import { checkPromiseStatus, logPromiseError } from '../../shared/utilities/webApiUtilities';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/BrokerAccount`;

export const getBrokerAccounts = async (): Promise<BrokerAccountEntity[]> => {
    return await getAllEntities<BrokerAccountEntity>(basicUrl);
};

export const getBrokerAccountById = async (id: string): Promise<BrokerAccountEntity | void> => {
    const brokerAccount: BrokerAccountEntity | void = await fetch(`${basicUrl}/GetById?id=${id}`, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
    return brokerAccount;
}

export const createBrokerAccount = async (addedBroker: BrokerAccountEntity): Promise<BrokerAccountEntity | void> => {
    return await createEntity<ServerBrokerAccountEntity>(basicUrl, prepareServerBrokerAccount(addedBroker));
}

export const updateBrokerAccount = async (modifiedBroker: BrokerAccountEntity): Promise<boolean> => {
    return await updateEntity<ServerBrokerAccountEntity>(basicUrl, prepareServerBrokerAccount(modifiedBroker));
}

export const deleteBrokerAccount = async (deleteBrokerAccountId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, deleteBrokerAccountId);
}

const prepareServerBrokerAccount = (brokerAccount: BrokerAccountEntity): ServerBrokerAccountEntity => {
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
