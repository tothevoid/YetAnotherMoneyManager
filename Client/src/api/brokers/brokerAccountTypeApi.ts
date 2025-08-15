import config from '../../config' 
import { BrokerAccountTypeEntity } from '../../models/brokers/BrokerAccountTypeEntity';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/BrokerAccountType`;

export const getBrokerAccountTypes = async (): Promise<BrokerAccountTypeEntity[]> => {
   return await getAllEntities<BrokerAccountTypeEntity>(basicUrl);
};

export const createBrokerAccountType = async (addedBrokerAccountType: BrokerAccountTypeEntity): Promise<BrokerAccountTypeEntity | void> => {
    return await createEntity<BrokerAccountTypeEntity, BrokerAccountTypeEntity>(basicUrl, addedBrokerAccountType);
}

export const updateBrokerAccountType = async (modifiedBrokerAccountType: BrokerAccountTypeEntity): Promise<boolean> => {
    return await updateEntity<BrokerAccountTypeEntity>(basicUrl, modifiedBrokerAccountType);
}

export const deleteBrokerAccountType = async (brokerAccountId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, brokerAccountId);
}