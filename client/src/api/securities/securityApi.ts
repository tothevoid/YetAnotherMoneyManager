import { i18n } from 'i18next';
import { formatDate } from '../../shared/utilities/formatters/dateFormatter';
import { SecurityHistoryValue } from '../../models/securities/SecurityHistoryValue';
import { createEntityWithIcon, deleteEntity, getAllEntities, getEntity, getEntityById, updateEntityWithIcon } from '../basicApi';
import { SecurityStats } from '../../models/securities/SecurityStats';
import { SecurityEntity, SecurityEntityRequest, SecurityEntityResponse } from '../../models/securities/SecurityEntity';
import { prepareSecurity, prepareSecurityEntityRequest } from './securityApiMapping';
import { Nullable } from '../../shared/utilities/nullable';
import { getStoredIconUrl } from '../iconApi';

const basicUrl = `Security`;
const ENTITY_NAME = "securityJson"
const ICON_NAME = "securityIcon"

export const getSecurities = async (): Promise<SecurityEntity[]> => {
    return await getAllEntities<SecurityEntityResponse>(basicUrl)
        .then(securityEntities => securityEntities.map(prepareSecurity));
};

export const getSecurityById = async (id: string): Promise<SecurityEntity | void> => {
    return await getEntityById<SecurityEntityResponse>(basicUrl, id)
        .then((response: SecurityEntityResponse | void) => response && prepareSecurity(response));
}

export const getSecurityStats = async (securityId: string): Promise<SecurityStats | void> => {
    return getEntity<SecurityStats>(`${basicUrl}/GetStats?securityId=${securityId}`);
}

export const getTickerHistory = async (ticker: string, format: i18n): Promise<SecurityHistoryValue[] | void> => {
    return getAllEntities<SecurityHistoryValue>(`${basicUrl}/GetTickerHistory?ticker=${ticker}`)
        .then((securityHistories: SecurityHistoryValue[]) => {
            return securityHistories.map(securityHistory => {
                return {
                    value: securityHistory.value,
                    date: formatDate(new Date(securityHistory.date), format)
                }
            })
        });
}

export const createSecurity = async (addedSecurity: SecurityEntity, file: File | null): Promise<SecurityEntity | void> => {
    return await createEntityWithIcon<SecurityEntityRequest, SecurityEntityResponse>(basicUrl, 
        prepareSecurityEntityRequest(addedSecurity), ENTITY_NAME, ICON_NAME, file)
        .then((securityResponse: SecurityEntityResponse | void) => securityResponse && prepareSecurity(securityResponse));
}

export const updateSecurity = async (modifiedSecurity: SecurityEntity, file: File | null): Promise<SecurityEntity | void> => {
    return await updateEntityWithIcon<SecurityEntityRequest, SecurityEntityResponse> (basicUrl, prepareSecurityEntityRequest(modifiedSecurity), ENTITY_NAME, ICON_NAME, file)
        .then((securityResponse: SecurityEntityResponse | void) => securityResponse && prepareSecurity(securityResponse));
}

export const deleteSecurity = async (securityId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, securityId);
}

export const getIconUrl = (iconKey: Nullable<string>, date: Nullable<Date> = null): string => {
    if (!iconKey) {
        return "";
    }

    const basicIconUrl = getStoredIconUrl(basicUrl, iconKey);
    return date ?
        `${basicIconUrl}&date=${date}`:
        basicIconUrl;
}