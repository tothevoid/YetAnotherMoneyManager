import config from '../config' 
import { SecurityTransactionEntity } from '../models/SecurityTransactionEntity';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from './basicApi';

const basicUrl = `${config.api.URL}/SecurityTransaction`;

export const getSecurityTransactions = async (): Promise<SecurityTransactionEntity[]> => {
   return await getAllEntities<SecurityTransactionEntity>(basicUrl);
};

export const createSecurityTransaction = async (addedSecurityTransaction: SecurityTransactionEntity): Promise<SecurityTransactionEntity | void> => {
    return await createEntity<SecurityTransactionEntity>(basicUrl, addedSecurityTransaction);
}

export const updateSecurityTransaction = async (modifiedSecurityTransaction: SecurityTransactionEntity): Promise<boolean> => {
    return await updateEntity<SecurityTransactionEntity>(basicUrl, modifiedSecurityTransaction);
}

export const deleteSecurityTransaction = async (securityTransactionId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, securityTransactionId);
}