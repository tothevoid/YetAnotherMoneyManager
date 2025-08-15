import config from '../../config' 
import { SecurityTransactionEntity, SecurityTransactionEntityRequest, SecurityTransactionEntityResponse } from '../../models/securities/SecurityTransactionEntity';
import { SecurityTransactionsHistory } from '../../models/securities/SecurityTransactionsHistory';
import { SecurityTransactionsPagination } from '../../models/securities/SecurityTransactionsPagination';
import { SecurityTransactionsRequest } from '../../models/securities/SecurityTransactionsRequest';
import { convertToDateOnly } from '../../shared/utilities/dateUtils';
import { checkPromiseStatus, logPromiseError } from '../../shared/utilities/webApiUtilities';
import { createEntity, deleteEntity, getAllEntitiesByConfig, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/SecurityTransaction`;

export const getSecurityTransactions = async (request: SecurityTransactionsRequest): Promise<SecurityTransactionEntity[]> => {
    return await getAllEntitiesByConfig<SecurityTransactionsRequest, SecurityTransactionEntityResponse> (`${basicUrl}/GetAll`, request)
        .then((securityTransactions: SecurityTransactionEntityResponse[]) => {
            return securityTransactions.map(prepareSecurityTransaction)
        })
};

export const gerSecurityTransactionsPagination = async (brokerAccountId: string): Promise<SecurityTransactionsPagination | void> => {
    return await fetch(`${basicUrl}/GetPagination?brokerAccountId=${brokerAccountId}`, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
};

export const getTransactionsBySecurity = async (securityId: string): Promise<SecurityTransactionsHistory[]> => {
    return await fetch(`${basicUrl}/GetTransactionsHistory?securityId=${securityId}`, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
};

export const createSecurityTransaction = async (addedSecurityTransaction: SecurityTransactionEntity): Promise<SecurityTransactionEntity | void> => {
    return await createEntity<SecurityTransactionEntityRequest, SecurityTransactionEntityResponse>(basicUrl, 
        prepareSecurityTransactionRequest(addedSecurityTransaction))
        .then(securityTransaction => securityTransaction && prepareSecurityTransaction(securityTransaction));
}

export const updateSecurityTransaction = async (modifiedSecurityTransaction: SecurityTransactionEntity): Promise<boolean> => {
    return await updateEntity<SecurityTransactionEntityRequest>(basicUrl, prepareSecurityTransactionRequest(modifiedSecurityTransaction));
}

export const deleteSecurityTransaction = async (securityTransactionId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, securityTransactionId);
}

const prepareSecurityTransactionRequest = (securityTransaction: SecurityTransactionEntity): SecurityTransactionEntityRequest => {
    return {
        id: securityTransaction.id,
        brokerCommission: securityTransaction.brokerCommission,
        stockExchangeCommission: securityTransaction.stockExchangeCommission,
        price: securityTransaction.price,
        quantity: securityTransaction.quantity,
        tax: securityTransaction.tax,
        date: convertToDateOnly(securityTransaction.date),
        brokerAccountId: securityTransaction.brokerAccount.id,
        securityId: securityTransaction.security.id,
    }
}

const prepareSecurityTransaction = (securityTransaction: SecurityTransactionEntityResponse): SecurityTransactionEntity => {
    return {
        id: securityTransaction.id,
        brokerCommission: securityTransaction.brokerCommission,
        stockExchangeCommission: securityTransaction.stockExchangeCommission,
        price: securityTransaction.price,
        quantity: securityTransaction.quantity,
        tax: securityTransaction.tax,
        date: new Date(securityTransaction.date),
        brokerAccount: securityTransaction.brokerAccount,
        security: securityTransaction.security,
    }
}
