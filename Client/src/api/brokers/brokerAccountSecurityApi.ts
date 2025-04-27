import config from '../../config' 
import { BrokerAccountSecurityEntity } from '../../models/brokers/BrokerAccountSecurityEntity';
import { checkPromiseStatus, logPromiseError } from '../../utils/PromiseUtils';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/BrokerAccountSecurity`;

export const getBrokerAccountSecurities = async (): Promise<BrokerAccountSecurityEntity[]> => {
    return await getAllEntities<BrokerAccountSecurityEntity>(basicUrl);
};

export const getSecuritiesByBrokerAccount = async (brokerAccountId: string): Promise<BrokerAccountSecurityEntity[]> => {
    const entities = await fetch(`${basicUrl}/GetByBrokerAccount?brokerAccountId=${brokerAccountId}`, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
     
    return entities ?
        entities: 
        [] as BrokerAccountSecurityEntity[];
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