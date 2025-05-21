import { i18n } from 'i18next';
import config from '../../config' 
import { formatDate } from '../../formatters/dateFormatter';
import { SecurityEntity, ServerSecurityEntity } from '../../models/securities/SecurityEntity';
import { SecurityHistory } from '../../models/securities/SecurityHistory';
import { convertToDateOnly } from '../../utils/DateUtils';
import { checkPromiseStatus, logPromiseError } from '../../utils/PromiseUtils';
import { deleteEntity, getAllEntities } from '../basicApi';

const basicUrl = `${config.api.URL}/Security`;

export const getSecurities = async (): Promise<SecurityEntity[]> => {
   return await getAllEntities<SecurityEntity>(basicUrl)
        .then(securityEntities => securityEntities.map(prepareClientSecurity));
};

export const getSecurityById = async (id: string): Promise<SecurityEntity | void> => {
    const brokerAccount: SecurityEntity | void = await fetch(`${basicUrl}/GetById?id=${id}`, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
    return brokerAccount;
}

export const getTickerHistory = async (ticker: string, format: i18n): Promise<SecurityHistory[] | void> => {
   const tickerHistoryValues: SecurityHistory[] | void = await fetch(`${basicUrl}/GetTickerHistory?ticker=${ticker}`, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then((securityHistories: SecurityHistory[]) => {
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
    return await fetch(basicUrl, { method: "PUT", body: generateForm(prepareServerSecurity(addedSecurity), file)})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then(id => {
            return {...addedSecurity, id} as T;
        })
        .catch(logPromiseError);
}

export const updateSecurity = async (modifiedSecurity: SecurityEntity, file: File | null): Promise<boolean> => {
    const securityResponse = await fetch(basicUrl, { method: "PATCH", body: generateForm(prepareServerSecurity(modifiedSecurity), file)})
        .then(checkPromiseStatus)
        .catch(logPromiseError)

    return securityResponse?.ok ?? false;
}

export const deleteSecurity = async (securityId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, securityId);
}

export const getIconUrl = (iconKey: string | null): string => {
    if (!iconKey) {
        return "";
    }

    return `${basicUrl}/icon?iconKey=${iconKey}`;
}

const prepareClientSecurity = (security: SecurityEntity): SecurityEntity => {
    return {
        ...security,
        priceFetchedAt: new Date(security.priceFetchedAt)
    };
}

const prepareServerSecurity = (security: SecurityEntity): ServerSecurityEntity => {
    const convertedSecurity: ServerSecurityEntity = {
        id: security.id,
        name: security.name,
        ticker: security.ticker,
        typeId: security.type.id,
        actualPrice: security.actualPrice,
        iconKey: security.iconKey,
        currencyId: security.currency.id
    };

    if (security.priceFetchedAt) {
        convertedSecurity.priceFetchedAt = convertToDateOnly(security.priceFetchedAt);
    }

    return convertedSecurity;
}

const generateForm = (security: ServerSecurityEntity, file: File | null) => {
    const formData = new FormData();
    formData.append("securityJson", JSON.stringify(security));
    if (file) {
        formData.append("securityIcon", file);
    }
    return formData;
}