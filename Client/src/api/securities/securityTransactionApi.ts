import config from '../../config' 
import { SecurityTransactionEntity } from '../../models/securities/SecurityTransactionEntity';
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
    return await createEntity<SecurityTransactionEntity>(basicUrl, prepareSecurityTransactionEntity(addedSecurityTransaction));
}

export const updateSecurityTransaction = async (modifiedSecurityTransaction: SecurityTransactionEntity): Promise<boolean> => {
    return await updateEntity<SecurityTransactionEntity>(basicUrl, prepareSecurityTransactionEntity(modifiedSecurityTransaction));
}

export const deleteSecurityTransaction = async (securityTransactionId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, securityTransactionId);
}

const prepareSecurityTransactionEntity = (securityTransaction: SecurityTransactionEntity): SecurityTransactionEntity => {
    const serverTransaction = {...securityTransaction};

    // .NET DateOnly cast
    serverTransaction.date = convertToDateOnly(serverTransaction.date );

    return serverTransaction;
}