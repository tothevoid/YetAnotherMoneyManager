import config from '../../config' 
import { DividendEntity, DividendEntityRequest, DividendEntityResponse } from '../../models/securities/DividendEntity';
import { PaginationConfig } from '../../shared/models/PaginationConfig';
import { checkPromiseStatus, logPromiseError } from '../../shared/utilities/webApiUtilities';
import { createEntity, deleteEntity, updateEntity } from '../basicApi';
import { prepareDividend, prepareDividendRequest } from './dividendApiMapping';

const basicUrl = `${config.api.URL}/Dividend`;

export const getDividends = async (securityId: string): Promise<DividendEntity[]> => {
    const dividends: DividendEntity[] | void = await fetch(`${basicUrl}/GetAll?securityId=${securityId}`, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then(dividends => dividends.map(prepareDividend))
        .catch(logPromiseError);
    
    return dividends ?? [];
};

export const getBrokerAccountFundsTransferPagination = async (securityId: string): Promise<PaginationConfig | void> => {
    return await fetch(`${basicUrl}/GetPagination?securityId=${securityId}`, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
};

export const getAvailableDividends = async (brokerAccountId: string): Promise<DividendEntity[]> => {
    const dividends: DividendEntity[] | void = await fetch(`${basicUrl}/GetAvailable?brokerAccountId=${brokerAccountId}`, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then(dividends => dividends.map(prepareDividend))
        .catch(logPromiseError);
    
    return dividends ?? [];
};

export const createDividend = async (dividend: DividendEntity): Promise<boolean> => {
    const result = await createEntity<DividendEntityRequest, DividendEntityResponse>(basicUrl, prepareDividendRequest(dividend));
    return !!result;
}

export const updateDividend = async (dividend: DividendEntity): Promise<boolean> => {
    return await updateEntity(basicUrl, prepareDividendRequest(dividend));
}

export const deleteDividend = async (dividendId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, dividendId);
}