import { SecurityTransactionEntity, SecurityTransactionEntityRequest, SecurityTransactionEntityResponse } from '../../models/securities/SecurityTransactionEntity';
import { SecurityTransactionsHistory } from '../../models/securities/SecurityTransactionsHistory';
import { SecurityTransactionsRequest } from '../../models/securities/SecurityTransactionsRequest';
import { PaginationConfig } from '../../shared/models/PaginationConfig';
import { createEntity, deleteEntity, getAllEntities, getAllEntitiesByConfig, getPagination, updateEntity } from '../basicApi';
import { prepareSecurityTransaction, prepareSecurityTransactionRequest } from './securityTransactionApiMapping';

const basicUrl = `SecurityTransaction`;

export const getSecurityTransactions = async (request: SecurityTransactionsRequest): Promise<SecurityTransactionEntity[]> => {
    return await getAllEntitiesByConfig<SecurityTransactionsRequest, SecurityTransactionEntityResponse> (`${basicUrl}/GetAll`, request)
        .then((securityTransactions: SecurityTransactionEntityResponse[]) => {
            return securityTransactions.map(prepareSecurityTransaction)
        })
};

export const getSecurityTransactionsPagination = async (brokerAccountId: string): Promise<PaginationConfig | void> => {
    return getPagination(`${basicUrl}/GetPagination?brokerAccountId=${brokerAccountId}`);
};

export const getTransactionsBySecurity = async (securityId: string): Promise<SecurityTransactionsHistory[]> => {
    return getAllEntities<SecurityTransactionsHistory>(`${basicUrl}/GetTransactionsHistory?securityId=${securityId}`);
};

export const createSecurityTransaction = async (addedSecurityTransaction: SecurityTransactionEntity): Promise<boolean | void> => {
    const createdSecurityTransaction = await createEntity<SecurityTransactionEntityRequest, SecurityTransactionEntityResponse>(basicUrl, 
        prepareSecurityTransactionRequest(addedSecurityTransaction));

    return !!createdSecurityTransaction;
}

export const updateSecurityTransaction = async (modifiedSecurityTransaction: SecurityTransactionEntity): Promise<boolean> => {
    return await updateEntity<SecurityTransactionEntityRequest>(basicUrl, prepareSecurityTransactionRequest(modifiedSecurityTransaction));
}

export const deleteSecurityTransaction = async (securityTransactionId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, securityTransactionId);
}