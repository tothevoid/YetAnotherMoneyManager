import { i18n } from 'i18next';
import config from '../../config' 
import { formatDate } from '../../shared/utilities/formatters/dateFormatter';
import { SecurityHistoryValue } from '../../models/securities/SecurityHistoryValue';
import { checkPromiseStatus, logPromiseError } from '../../shared/utilities/webApiUtilities';
import { createEntityWithIcon, deleteEntity, getAllEntities, updateEntityWithIcon } from '../basicApi';
import { SecurityStats } from '../../models/securities/SecurityStats';
import { SecurityEntity, SecurityEntityRequest, SecurityEntityResponse } from '../../models/securities/SecurityEntity';
import { prepareSecurity, prepareSecurityEntityRequest } from './securityApiMapping';
import { Nullable } from '../../shared/utilities/nullable';

const basicUrl = `${config.api.URL}/Security`;
const FIELD_NAME = "securityJson"

export const getSecurities = async (): Promise<SecurityEntity[]> => {
    return await getAllEntities<SecurityEntityResponse>(basicUrl)
        .then(securityEntities => securityEntities.map(prepareSecurity));
};

export const getSecurityById = async (id: string): Promise<SecurityEntity | void> => {
    return await fetch(`${basicUrl}/GetById?id=${id}`, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then((response: SecurityEntityResponse) => response && prepareSecurity(response))
        .catch(logPromiseError);
}

export const getSecurityStats = async (securityId: string): Promise<SecurityStats | void> => {
    return await fetch(`${basicUrl}/GetStats?securityId=${securityId}`, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
}

export const getTickerHistory = async (ticker: string, format: i18n): Promise<SecurityHistoryValue[] | void> => {
   const tickerHistoryValues: SecurityHistoryValue[] | void = await fetch(`${basicUrl}/GetTickerHistory?ticker=${ticker}`, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then((securityHistories: SecurityHistoryValue[]) => {
            return securityHistories.map(securityHistory => {
                return {
                    value: securityHistory.value,
                    date: formatDate(new Date(securityHistory.date), format)
                }
            })
        })
        .catch(logPromiseError);
    return tickerHistoryValues;
}

export const createSecurity = async (addedSecurity: SecurityEntity, file: File | null): Promise<SecurityEntity | void> => {
    const createdEntity = await createEntityWithIcon<SecurityEntityRequest, SecurityEntityRequest>(basicUrl, prepareSecurityEntityRequest(addedSecurity), FIELD_NAME, file);

    if (!createdEntity) {
        return;
    }

    return {...addedSecurity, id: createdEntity.id}
}

export const updateSecurity = async (modifiedSecurity: SecurityEntity, file: File | null): Promise<boolean> => {
    return await updateEntityWithIcon(basicUrl, prepareSecurityEntityRequest(modifiedSecurity), FIELD_NAME, file);;
}

export const deleteSecurity = async (securityId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, securityId);
}

export const getIconUrl = (iconKey: Nullable<string>): string => {
    if (!iconKey) {
        return "";
    }

    return `${basicUrl}/icon?iconKey=${iconKey}`;
}