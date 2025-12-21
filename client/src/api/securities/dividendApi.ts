import { DividendEntity, DividendEntityRequest, DividendEntityResponse } from '../../models/securities/DividendEntity';
import { DividendsQuery } from '../../pages/SecurityPage/hooks/useDividends';
import { PaginationConfig } from '../../shared/models/PaginationConfig';
import { createEntity, deleteEntity, getAllEntities, getAllEntitiesByConfig, getPagination, updateEntity } from '../basicApi';
import { prepareDividend, prepareDividendRequest } from './dividendApiMapping';

const basicUrl = `/Dividend`;

export const getDividends = async (query: DividendsQuery): Promise<DividendEntity[]> => {
    return await getAllEntitiesByConfig<DividendsQuery, DividendEntityResponse>(`${basicUrl}/GetAll`, query)
        .then((dividend) => {
            return dividend.map(prepareDividend)
        });
};

export const getDividendsPagination = async (securityId: string): Promise<PaginationConfig | void> => {
    return await getPagination(`${basicUrl}/GetPagination?securityId=${securityId}`);
};

export const getAvailableDividends = async (brokerAccountId: string): Promise<DividendEntity[]> => {
    return await getAllEntities<DividendEntityResponse>(`${basicUrl}/GetAvailable?brokerAccountId=${brokerAccountId}`)
        .then(dividends => dividends.map(prepareDividend));
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