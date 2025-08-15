import config from '../../config' 
import { DividendEntity, DividendEntityRequest, DividendEntityResponse } from '../../models/securities/DividendEntity';
import { convertToDateOnly } from '../../shared/utilities/dateUtils';
import { checkPromiseStatus, logPromiseError } from '../../shared/utilities/webApiUtilities';
import { createEntity, deleteEntity, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/Dividend`;

export const getDividends = async (securityId: string): Promise<DividendEntity[]> => {
    const dividends: DividendEntity[] | void = await fetch(`${basicUrl}/GetAll?securityId=${securityId}`, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then(dividends => dividends.map(prepareDividend))
        .catch(logPromiseError);
    
    return dividends ?? [];
};

export const getAvailableDividends = async (brokerAccountId: string): Promise<DividendEntity[]> => {
    const dividends: DividendEntity[] | void = await fetch(`${basicUrl}/GetAvailable?brokerAccountId=${brokerAccountId}`, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then(dividends => dividends.map(prepareDividend))
        .catch(logPromiseError);
    
    return dividends ?? [];
};

export const createDividend = async (dividend: DividendEntity): Promise<DividendEntity | void> => {
    return await createEntity<DividendEntityRequest, DividendEntityResponse>(basicUrl, prepareDividendRequest(dividend))
        .then((dividendResponse) =>  dividendResponse && prepareDividend(dividendResponse));
}

export const updateDividend = async (dividend: DividendEntity): Promise<boolean> => {
    return await updateEntity(basicUrl, prepareDividendRequest(dividend));
}

export const deleteDividend = async (dividendId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, dividendId);
}

const prepareDividendRequest = (dividend: DividendEntity): DividendEntityRequest => {
    return {
        id: dividend.id,
        declarationDate: convertToDateOnly(dividend.declarationDate),
        snapshotDate: convertToDateOnly(dividend.snapshotDate),
        securityId: dividend.security.id,
        amount: dividend.amount
    };
}

const prepareDividend = (dividend: DividendEntityResponse): DividendEntity => {
    return {
        id: dividend.id,
        amount: dividend.amount,
        security: dividend.security,
        declarationDate: new Date(dividend.declarationDate),
        snapshotDate: new Date(dividend.snapshotDate)
    };
}