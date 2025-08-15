import config from '../../config' 
import { BrokerAccountSecurityEntity, BrokerAccountSecurityEntityRequest, BrokerAccountSecurityEntityResponse } from '../../models/brokers/BrokerAccountSecurityEntity';
import { checkPromiseStatus, logPromiseError } from '../../shared/utilities/webApiUtilities';
import { deleteEntity, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/BrokerAccountSecurity`;

export const getSecuritiesByBrokerAccount = async (brokerAccountId: string): Promise<BrokerAccountSecurityEntity[]> => {
    const securitiesByBrokerAccount = await fetch(`${basicUrl}/GetByBrokerAccount?brokerAccountId=${brokerAccountId}`, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then((securities: BrokerAccountSecurityEntityResponse[]) => securities.map(prepareBrokerAccountSecurity))
        .catch(logPromiseError);
     
    return securitiesByBrokerAccount ?? [];
};

export const pullBrokerAccountQuotations = async (brokerAccountId: string) => {
    await fetch(`${basicUrl}/PullQuotations?brokerAccountId=${brokerAccountId}`, {method: "GET"})
        .then(checkPromiseStatus)
        .catch(logPromiseError);
}

export const updateBrokerAccountSecurity = async (modifiedBrokerAccountSecurity: BrokerAccountSecurityEntity): Promise<boolean> => {
    return await updateEntity(basicUrl, prepareBrokerAccountSecurityResponse(modifiedBrokerAccountSecurity));
}

export const deleteBrokerAccountSecurity = async (brokerAccountSecurityId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, brokerAccountSecurityId);
}

const prepareBrokerAccountSecurityResponse = (brokerAccountSecurity: BrokerAccountSecurityEntity): BrokerAccountSecurityEntityRequest => {
    return {
        id: brokerAccountSecurity.id,
        brokerAccountId: brokerAccountSecurity.brokerAccount.id,
        price: brokerAccountSecurity.price,
        quantity: brokerAccountSecurity.quantity,
        securityId: brokerAccountSecurity.security.id
    };
}

const prepareBrokerAccountSecurity = (brokerAccountSecurity: BrokerAccountSecurityEntityResponse): BrokerAccountSecurityEntity => {
    return {
        id: brokerAccountSecurity.id,
        brokerAccount: brokerAccountSecurity.brokerAccount,
        price: brokerAccountSecurity.price,
        quantity: brokerAccountSecurity.quantity,
        security: brokerAccountSecurity.security
    };
}