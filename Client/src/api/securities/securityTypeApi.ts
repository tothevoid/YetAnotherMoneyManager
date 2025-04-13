import config from '../../config' 
import { SecurityTransactionEntity } from '../../models/securities/SecurityTransactionEntity';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/SecurityType`;

export const getSecurityTypes = async (): Promise<SecurityTransactionEntity[]> => {
   return await getAllEntities<SecurityTransactionEntity>(basicUrl);
};

export const createSecurityType = async (addedSecurityType: SecurityTransactionEntity): Promise<SecurityTransactionEntity | void> => {
    return await createEntity<SecurityTransactionEntity>(basicUrl, addedSecurityType);
}

export const updateSecurityType = async (modifiedSecurityType: SecurityTransactionEntity): Promise<boolean> => {
    return await updateEntity<SecurityTransactionEntity>(basicUrl, modifiedSecurityType);
}

export const deleteSecurityType = async (securityTypeId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, securityTypeId);
}