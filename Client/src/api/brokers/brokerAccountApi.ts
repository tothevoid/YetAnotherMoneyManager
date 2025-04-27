import config from '../../config' 
import { BrokerAccountEntity } from '../../models/brokers/BrokerAccountEntity';
import { checkPromiseStatus, logPromiseError } from '../../utils/PromiseUtils';
import { convertRecordToJson, createEntity, deleteEntity, getAllEntities, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/BrokerAccount`;

export const getBrokerAccounts = async (): Promise<BrokerAccountEntity[]> => {
    return (await getAllEntities<BrokerAccountEntity>(basicUrl)).map(brokerAccount => {
        const lastUpdateAt = new Date(brokerAccount.lastUpdateAt);
        return {...brokerAccount, lastUpdateAt} as BrokerAccountEntity;
    });
};

export const getBrokerAccountById = async (id: string): Promise<BrokerAccountEntity | void> => {
    const brokerAccount: BrokerAccountEntity | void = await fetch(`${basicUrl}/GetById?id=${id}`, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
    return brokerAccount;
}

export const createBrokerAccount = async (addedBroker: BrokerAccountEntity): Promise<BrokerAccountEntity | void> => {
    return await createEntity<BrokerAccountEntity>(basicUrl, addedBroker);
}

export const updateBrokerAccount = async (modifiedBroker: BrokerAccountEntity): Promise<boolean> => {
    return await updateEntity<BrokerAccountEntity>(basicUrl, modifiedBroker);
}

export const deleteBrokerAccount = async (deleteBrokerAccountId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, deleteBrokerAccountId);
}