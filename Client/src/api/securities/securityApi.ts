import config from '../../config' 
import { SecurityEntity, ServerSecurityEntity } from '../../models/securities/SecurityEntity';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/Security`;

export const getSecurities = async (): Promise<SecurityEntity[]> => {
   return await getAllEntities<SecurityEntity>(basicUrl);
};

export const createSecurity = async (addedSecurity: SecurityEntity): Promise<SecurityEntity | void> => {
    return await createEntity<ServerSecurityEntity>(basicUrl, prepareServerSecurity(addedSecurity));
}

export const updateSecurity = async (modifiedSecurity: SecurityEntity): Promise<boolean> => {
    return await updateEntity<ServerSecurityEntity>(basicUrl, prepareServerSecurity(modifiedSecurity));
}

export const deleteSecurity = async (securityId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, securityId);
}

const prepareServerSecurity = (security: SecurityEntity): ServerSecurityEntity => {
    return {
        id: security.id,
        name: security.name,
        ticker: security.ticker,
        typeId: security.type.id
    };
}