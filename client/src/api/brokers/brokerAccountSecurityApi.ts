import config from '../../config' 
import { BrokerAccountSecurityEntity, BrokerAccountSecurityEntityResponse } from '../../models/brokers/BrokerAccountSecurityEntity';
import { Nullable } from '../../shared/utilities/nullable';
import { checkPromiseStatus, logPromiseError } from '../../shared/utilities/webApiUtilities';
import { deleteEntity, getAction, getAllEntities, updateEntity } from '../basicApi';
import { prepareBrokerAccountSecurity, prepareBrokerAccountSecurityRequest } from './brokerAccountSecurityApiMapping';

const basicUrl = `${config.api.URL}/BrokerAccountSecurity`;

export const getSecuritiesByBrokerAccount = async (brokerAccountId: string): Promise<BrokerAccountSecurityEntity[]> => {
    return await getAllEntities<BrokerAccountSecurityEntityResponse>(`${basicUrl}/GetByBrokerAccount?brokerAccountId=${brokerAccountId}`)
        .then((securities: BrokerAccountSecurityEntityResponse[]) => securities.map(prepareBrokerAccountSecurity))
};

export const pullBrokerAccountQuotations = async (brokerAccountId: string) => {
    await getAction(`${basicUrl}/PullQuotations?brokerAccountId=${brokerAccountId}`);
};

export const getLastPullDate = async (): Promise<Nullable<Date>> => {
    return await fetch(`${basicUrl}/GetLastPullDate`, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then((pullInfo: { lastPullDate: string }) => new Date(pullInfo.lastPullDate))
        .catch(logPromiseError) ?? null;
};

export const updateBrokerAccountSecurity = async (modifiedBrokerAccountSecurity: BrokerAccountSecurityEntity): Promise<boolean> => {
    return await updateEntity(basicUrl, prepareBrokerAccountSecurityRequest(modifiedBrokerAccountSecurity));
}

export const deleteBrokerAccountSecurity = async (brokerAccountSecurityId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, brokerAccountSecurityId);
}