import config from '../../config' 
import { CryptocurrencyEntity } from '../../models/crypto/CryptocurrencyEntity';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/Cryptocurrencies`;

export const getCryptocurrencies = async (): Promise<CryptocurrencyEntity[]> => {
   return await getAllEntities<CryptocurrencyEntity>(basicUrl);
};

export const createCryptocurrency = async (addedCryptocurrency: CryptocurrencyEntity): Promise<CryptocurrencyEntity | void> => {
    return await createEntity<CryptocurrencyEntity>(basicUrl, addedCryptocurrency);
}

export const updateCryptocurrency = async (modifiedCryptocurrency: CryptocurrencyEntity): Promise<boolean> => {
    return await updateEntity<CryptocurrencyEntity>(basicUrl, modifiedCryptocurrency);
}

export const deleteCryptocurrency = async (cryptoCurrencyId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, cryptoCurrencyId);
}