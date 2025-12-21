import { CryptoAccountCryptocurrencyEntity, CryptoAccountCryptocurrencyEntityRequest, CryptoAccountCryptocurrencyEntityResponse } from '../../models/crypto/CryptoAccountCryptocurrencyEntity';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from '../basicApi';
import { prepareCryptoAccountCryptocurrency, prepareCryptoAccountCryptocurrencyRequest } from './cryptoAccountCryptocurrencyApiMapping';

const basicUrl = `CryptoAccountCryptocurrency`;

export const getCryptoAccountCryptocurrencies = async (): Promise<CryptoAccountCryptocurrencyEntity[]> => {
   return await getAllEntities<CryptoAccountCryptocurrencyEntityResponse>(basicUrl)
        .then(cryptoAccounts => cryptoAccounts.map(prepareCryptoAccountCryptocurrency));
};

export const getCryptocurrenciesByCryptoAccount = async (cryptoAccountId: string): Promise<CryptoAccountCryptocurrencyEntity[]> => {
    return await getAllEntities<CryptoAccountCryptocurrencyEntityResponse>(`${basicUrl}/GetByCryptoAccount?cryptoAccountId=${cryptoAccountId}`)
        .then((cryptoAccountCryptocurrencies: CryptoAccountCryptocurrencyEntityResponse[]) => cryptoAccountCryptocurrencies.map(prepareCryptoAccountCryptocurrency));
};

export const createCryptoAccountCryptocurrency = async (addedCryptoAccountCryptocurrency: CryptoAccountCryptocurrencyEntity): Promise<CryptoAccountCryptocurrencyEntity | void> => {
    return await createEntity<CryptoAccountCryptocurrencyEntityRequest, CryptoAccountCryptocurrencyEntityResponse>(basicUrl, prepareCryptoAccountCryptocurrencyRequest(addedCryptoAccountCryptocurrency))
        .then((cryptoAccountCryptocurrency) => cryptoAccountCryptocurrency && prepareCryptoAccountCryptocurrency(cryptoAccountCryptocurrency));
}

export const updateCryptoAccountCryptocurrency = async (modifiedCryptoAccountCryptocurrency: CryptoAccountCryptocurrencyEntity): Promise<boolean> => {
    return await updateEntity<CryptoAccountCryptocurrencyEntityRequest>(basicUrl, prepareCryptoAccountCryptocurrencyRequest(modifiedCryptoAccountCryptocurrency));
}

export const deleteCryptoAccountCryptocurrency = async (cryptoAccountCryptocurrencyId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, cryptoAccountCryptocurrencyId);
}