import config from '../../config' 
import { CryptoProviderEntity } from '../../models/crypto/CryptoProviderEntity';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/CryptoProvider`;

export const getCryptoProviders = async (): Promise<CryptoProviderEntity[]> => {
   return await getAllEntities<CryptoProviderEntity>(basicUrl);
};

export const createCryptoProvider = async (addedCryptoProvider: CryptoProviderEntity): Promise<CryptoProviderEntity | void> => {
    return await createEntity<CryptoProviderEntity, CryptoProviderEntity>(basicUrl, addedCryptoProvider);
}

export const updateCryptoProvider = async (modifiedCryptoProvider: CryptoProviderEntity): Promise<boolean> => {
    return await updateEntity<CryptoProviderEntity>(basicUrl, modifiedCryptoProvider);
}

export const deleteCryptoProvider = async (cryptoProviderId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, cryptoProviderId);
}