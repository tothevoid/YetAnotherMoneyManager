import config from '../../config' 
import { SecurityTransactionEntity, ServerSecurityTransactionEntity } from '../../models/securities/SecurityTransactionEntity';
import { convertToDateOnly } from '../../utils/DateUtils';
import { checkPromiseStatus, logPromiseError } from '../../utils/PromiseUtils';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/SecurityTransaction`;

export const getSecurityTransactions = async (): Promise<SecurityTransactionEntity[]> => {
    const securityTransactions = await getAllEntities<SecurityTransactionEntity>(basicUrl);
    return securityTransactions.map((securityTransaction: SecurityTransactionEntity) => {
        const date = new Date(securityTransaction.date);
        return {...securityTransaction, date: date} as SecurityTransactionEntity;
    });
};

export const getSecurityTransactionsByBrokerAccount = async (brokerAccountId: string): Promise<SecurityTransactionEntity[]> => {
    const securityTransactions = await fetch(`${basicUrl}/GetByBrokerAccount?brokerAccountId=${brokerAccountId}`, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then((securityTransactions: SecurityTransactionEntity[]) => {
            return securityTransactions.map(securityTransaction => {
                const date = new Date(securityTransaction.date);
                return {...securityTransaction, date: date} as SecurityTransactionEntity;
            })
        })
        .catch(logPromiseError);
         
    return securityTransactions ?
        securityTransactions: 
        [] as SecurityTransactionEntity[];
   
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

const prepareServerSecurityTransaction = (securityTransaction: SecurityTransactionEntity): ServerSecurityTransactionEntity => {
    return {
        id: securityTransaction.id,
        commission: securityTransaction.commission,
        price: securityTransaction.price,
        quantity: securityTransaction.quantity,
        tax: securityTransaction.tax,
        date: convertToDateOnly(securityTransaction.date),
        brokerAccountId: securityTransaction.brokerAccount.id,
        securityId: securityTransaction.security.id,
    }
}
