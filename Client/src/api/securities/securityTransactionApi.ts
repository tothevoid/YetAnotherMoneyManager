import config from '../../config' 
import { SecurityTransactionEntity, ServerSecurityTransactionEntity } from '../../models/securities/SecurityTransactionEntity';
import { SecurityTransactionsPagination } from '../../models/securities/SecurityTransactionsPagination';
import { SecurityTransactionsRequest } from '../../models/securities/SecurityTransactionsRequest';
import { checkPromiseStatus, logPromiseError } from '../../utils/PromiseUtils';
import { createEntity, deleteEntity, getAllEntitiesByConfig, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/SecurityTransaction`;

export const getSecurityTransactions = async (request: SecurityTransactionsRequest): Promise<SecurityTransactionEntity[]> => {
    return await getAllEntitiesByConfig<SecurityTransactionsRequest, SecurityTransactionEntity> (`${basicUrl}/GetAll`, request)
        .then((securityTransactions: SecurityTransactionEntity[]) => {
            return securityTransactions.map(prepareClientSecurityTransaction)
        })   
};

export const gerSecurityTransactionsPagination = async (brokerAccountId: string): Promise<SecurityTransactionsPagination | void> => {
    return await fetch(`${basicUrl}/GetPagination?brokerAccountId=${brokerAccountId}`, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
};

export const createSecurityTransaction = async (addedSecurityTransaction: SecurityTransactionEntity): Promise<SecurityTransactionEntity | void> => {
    return await createEntity(basicUrl, prepareServerSecurityTransaction(addedSecurityTransaction));
}

export const updateSecurityTransaction = async (modifiedSecurityTransaction: SecurityTransactionEntity): Promise<boolean> => {
    return await updateEntity(basicUrl, prepareServerSecurityTransaction(modifiedSecurityTransaction));
}

export const deleteSecurityTransaction = async (securityTransactionId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, securityTransactionId);
}

const prepareClientSecurityTransaction = (securityTransaction: SecurityTransactionEntity): SecurityTransactionEntity => {
    const date = new Date(securityTransaction.date);
    return {...securityTransaction, date: date} as SecurityTransactionEntity;
}

const prepareServerSecurityTransaction = (securityTransaction: SecurityTransactionEntity): ServerSecurityTransactionEntity => {
    return {
        id: securityTransaction.id,
        commission: securityTransaction.commission,
        price: securityTransaction.price,
        quantity: securityTransaction.quantity,
        tax: securityTransaction.tax,
        date: securityTransaction.date,
        brokerAccountId: securityTransaction.brokerAccount.id,
        securityId: securityTransaction.security.id,
    }
}
