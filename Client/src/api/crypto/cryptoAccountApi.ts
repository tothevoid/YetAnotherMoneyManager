import config from '../../config' 
import { CryptoAccountEntity, CryptoAccountEntityRequest, CryptoAccountEntityResponse } from '../../models/crypto/CryptoAccountEntity';
import { createEntity, deleteEntity, getAllEntities, getEntityById, updateEntity } from '../basicApi';
import { prepareCryptoAccountEntity, prepareCryptoAccountEntityRequest } from './cryptoAccountApiMapping';

const basicUrl = `${config.api.URL}/CryptoAccount`;

export const getCryptoAccounts = async (): Promise<CryptoAccountEntity[]> => {
    return await getAllEntities<CryptoAccountEntityResponse>(basicUrl)
        .then(cryptoAccounts => cryptoAccounts.map(prepareCryptoAccountEntity));
};

export const getCryptoAccountById = async (id: string): Promise<CryptoAccountEntity | void> => {
    return await getEntityById<CryptoAccountEntityResponse>(basicUrl, id)
        .then((cryptoAccountEntity) => cryptoAccountEntity && prepareCryptoAccountEntity(cryptoAccountEntity));
}

export const createCryptoAccount = async (addedCryptoAccount: CryptoAccountEntity): Promise<CryptoAccountEntity | void> => {
    return await createEntity<CryptoAccountEntityRequest, CryptoAccountEntityResponse>(basicUrl, prepareCryptoAccountEntityRequest(addedCryptoAccount))
        .then((cryptoAccountEntity) => cryptoAccountEntity && prepareCryptoAccountEntity(cryptoAccountEntity));
}

export const updateCryptoAccount = async (modifiedCryptoAccount: CryptoAccountEntity): Promise<boolean> => {
    return await updateEntity<CryptoAccountEntityRequest>(basicUrl, prepareCryptoAccountEntityRequest(modifiedCryptoAccount));
}

export const deleteCryptoAccount = async (cryptoAccountId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, cryptoAccountId);
}

