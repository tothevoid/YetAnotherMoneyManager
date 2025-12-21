 import { SecurityTypeEntity } from '../../models/securities/SecurityTypeEntity';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from '../basicApi';

const basicUrl = `SecurityType`;

export const getSecurityTypes = async (): Promise<SecurityTypeEntity[]> => {
   return await getAllEntities<SecurityTypeEntity>(basicUrl);
};

export const createSecurityType = async (addedSecurityType: SecurityTypeEntity): Promise<SecurityTypeEntity | void> => {
    return await createEntity<SecurityTypeEntity, SecurityTypeEntity>(basicUrl, addedSecurityType);
}

export const updateSecurityType = async (modifiedSecurityType: SecurityTypeEntity): Promise<boolean> => {
    return await updateEntity<SecurityTypeEntity>(basicUrl, modifiedSecurityType);
}

export const deleteSecurityType = async (securityTypeId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, securityTypeId);
}