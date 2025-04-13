import config from '../config' 
import { BrokerAccountEntity } from '../models/brokers/BrokerAccountEntity';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from './basicApi';

const basicUrl = `${config.api.URL}/BrokerAccount`;

export const getBrokerAccounts = async (): Promise<BrokerAccountEntity[]> => {
    return await getAllEntities<BrokerAccountEntity>(basicUrl);
};

export const createBrokerAccount = async (addedBroker: BrokerAccountEntity): Promise<BrokerAccountEntity | void> => {
    return await createEntity<BrokerAccountEntity>(basicUrl, addedBroker);
}

export const updateBrokerAccount = async (modifiedBroker: BrokerAccountEntity): Promise<boolean> => {
    return await updateEntity<BrokerAccountEntity>(basicUrl, modifiedBroker);
}

export const deleteBrokerAccount = async (deleteBrokerAccountId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, deleteBrokerAccountId);
}