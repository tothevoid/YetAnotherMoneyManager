import config from '../../config' 
import { BrokerAccountSecurityEntity, ServerBrokerAccountSecurityEntity } from '../../models/brokers/BrokerAccountSecurityEntity';
import { checkPromiseStatus, logPromiseError } from '../../shared/utilities/webApiUtilities';
import { deleteEntity, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/BrokerAccountSecurity`;

export const getSecuritiesByBrokerAccount = async (brokerAccountId: string): Promise<BrokerAccountSecurityEntity[]> => {
    const entities = await fetch(`${basicUrl}/GetByBrokerAccount?brokerAccountId=${brokerAccountId}`, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
     
    return entities ?
        entities: 
        [] as BrokerAccountSecurityEntity[];
};

export const pullBrokerAccountQuotations = async (brokerAccountId: string) => {
    await fetch(`${basicUrl}/PullQuotations?brokerAccountId=${brokerAccountId}`, {method: "GET"})
        .then(checkPromiseStatus)
        .catch(logPromiseError);
}

export const updateBrokerAccountSecurity = async (modifiedBrokerAccountSecurity: BrokerAccountSecurityEntity): Promise<boolean> => {
    return await updateEntity<ServerBrokerAccountSecurityEntity>(basicUrl, 
        prepareServerBrokerAccountSecurity(modifiedBrokerAccountSecurity));
}

export const deleteBrokerAccountSecurity = async (brokerAccountSecurityId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, brokerAccountSecurityId);
}

const prepareServerBrokerAccountSecurity = (brokerAccountSecurity: BrokerAccountSecurityEntity): ServerBrokerAccountSecurityEntity => {
    return {
        id: brokerAccountSecurity.id,
        brokerAccountId: brokerAccountSecurity.brokerAccount.id,
        price: brokerAccountSecurity.price,
        quantity: brokerAccountSecurity.quantity,
        securityId: brokerAccountSecurity.security.id
    };
}