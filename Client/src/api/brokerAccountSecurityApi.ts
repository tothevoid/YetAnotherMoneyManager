import config from '../config' 
import { BrokerAccountSecurityEntity } from '../models/BrokerAccountSecurityEntity';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from './basicApi';

const basicUrl = `${config.api.URL}/BrokerAccountSecurity`;

export const getBrokerAccountSecurities = async (): Promise<BrokerAccountSecurityEntity[]> => {
   return await getAllEntities<BrokerAccountSecurityEntity>(basicUrl);
};

export const createBrokerAccountSecurity = async (addedBrokerAccountSecurity: BrokerAccountSecurityEntity): Promise<BrokerAccountSecurityEntity | void> => {
    return await createEntity<BrokerAccountSecurityEntity>(basicUrl, addedBrokerAccountSecurity);
}

export const updateBrokerAccountSecurity = async (modifiedBrokerAccountSecurity: BrokerAccountSecurityEntity): Promise<boolean> => {
    return await updateEntity<BrokerAccountSecurityEntity>(basicUrl, modifiedBrokerAccountSecurity);
}

export const deleteBrokerAccountSecurity = async (brokerAccountSecurityId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, brokerAccountSecurityId);
}