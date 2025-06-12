import config from '../../config' 
import { DividendEntity, ServerDividendEntity } from '../../models/securities/DividendEntity';
import { convertToDateOnly } from '../../shared/utilities/dateUtils';
import { checkPromiseStatus, logPromiseError } from '../../shared/utilities/webApiUtilities';
import { createEntity, deleteEntity, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/Dividend`;

export const getDividends = async (securityId: string): Promise<DividendEntity[]> => {
    const dividends: DividendEntity[] | void = await fetch(`${basicUrl}/GetAll?securityId=${securityId}`, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then(dividends => dividends.map(prepareClientDividend))
        .catch(logPromiseError);
    
    return dividends ?
        dividends: 
        [];
};

export const getAvailableDividends = async (brokerAccountId: string): Promise<DividendEntity[]> => {
    const dividends: DividendEntity[] | void = await fetch(`${basicUrl}/GetAvailable?brokerAccountId=${brokerAccountId}`, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then(dividends => dividends.map(prepareClientDividend))
        .catch(logPromiseError);
    
    return dividends ?
        dividends: 
        [];
};


export const createDividend = async (dividend: DividendEntity): Promise<DividendEntity | void> => {
    return await createEntity<ServerDividendEntity>(basicUrl, prepareServerDividend(dividend));
}

export const updateDividend = async (dividend: DividendEntity): Promise<boolean> => {
    return await updateEntity<ServerDividendEntity>(basicUrl, prepareServerDividend(dividend));
}

export const deleteDividend = async (dividendId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, dividendId);
}

export const prepareClientDividend = (dividend: DividendEntity): DividendEntity => {
    return {
        ...dividend,
        declarationDate: new Date(dividend.declarationDate),
        paymentDate: new Date(dividend.paymentDate),
        snapshotDate: new Date(dividend.snapshotDate)
    };
}

const prepareServerDividend = (dividend: DividendEntity): ServerDividendEntity => {
    return {
        id: dividend.id,
        declarationDate: convertToDateOnly(dividend.declarationDate),
        paymentDate: convertToDateOnly(dividend.paymentDate),
        snapshotDate: convertToDateOnly(dividend.snapshotDate),
        securityId: dividend.security.id,
        amount: dividend.amount
    };
}