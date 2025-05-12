import config from '../../config' 
import { SecurityEntity, ServerSecurityEntity } from '../../models/securities/SecurityEntity';
import { convertToDateOnly } from '../../utils/DateUtils';
import { checkPromiseStatus, logPromiseError } from '../../utils/PromiseUtils';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/Security`;

export const getSecurities = async (): Promise<SecurityEntity[]> => {
   return await getAllEntities<SecurityEntity>(basicUrl)
        .then(securityEntities => securityEntities.map(prepareClientSecurity));
};

export const getSecurityById = async (id: string): Promise<SecurityEntity | void> => {
    const brokerAccount: SecurityEntity | void = await fetch(`${basicUrl}/GetById?id=${id}`, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
    return brokerAccount;
}

export const createSecurity = async (addedSecurity: SecurityEntity): Promise<SecurityEntity | void> => {
    return await createEntity<ServerSecurityEntity>(basicUrl, prepareServerSecurity(addedSecurity));
}

export const updateSecurity = async (modifiedSecurity: SecurityEntity): Promise<boolean> => {
    return await updateEntity<ServerSecurityEntity>(basicUrl, prepareServerSecurity(modifiedSecurity));
}

export const deleteSecurity = async (securityId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, securityId);
}

const prepareClientSecurity = (security: SecurityEntity): SecurityEntity => {
    return {
        ...security,
        priceFetchedAt: new Date(security.priceFetchedAt)
    };
}

const prepareServerSecurity = (security: SecurityEntity): ServerSecurityEntity => {
    return {
        id: security.id,
        name: security.name,
        ticker: security.ticker,
        typeId: security.type.id,
        actualPrice: security.actualPrice,
        priceFetchedAt: convertToDateOnly(security.priceFetchedAt),
    };
}