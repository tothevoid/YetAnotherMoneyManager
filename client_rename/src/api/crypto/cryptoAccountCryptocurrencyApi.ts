import config from '../../config' 
import { CryptoAccountCryptocurrencyEntity, CryptoAccountCryptocurrencyEntityRequest, CryptoAccountCryptocurrencyEntityResponse } from '../../models/crypto/CryptoAccountCryptocurrencyEntity';
import { checkPromiseStatus, logPromiseError } from '../../shared/utilities/webApiUtilities';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from '../basicApi';
import { prepareCryptoAccountCryptocurrency, prepareCryptoAccountCryptocurrencyRequest } from './cryptoAccountCryptocurrencyApiMapping';

const basicUrl = `${config.api.URL}/CryptoAccountCryptocurrency`;

export const getCryptoAccountCryptocurrencies = async (): Promise<CryptoAccountCryptocurrencyEntity[]> => {
   return await getAllEntities<CryptoAccountCryptocurrencyEntityResponse>(basicUrl)
        .then(cryptoAccounts => cryptoAccounts.map(prepareCryptoAccountCryptocurrency));
};

export const getCryptocurrenciesByCryptoAccount = async (cryptoAccountId: string): Promise<CryptoAccountCryptocurrencyEntity[]> => {
    const cryptoAccountCryptocurrencies = await fetch(`${basicUrl}/GetByCryptoAccount?cryptoAccountId=${cryptoAccountId}`, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then((cryptoAccountCryptocurrencies: CryptoAccountCryptocurrencyEntityResponse[]) => cryptoAccountCryptocurrencies.map(prepareCryptoAccountCryptocurrency))
        .catch(logPromiseError);
     
    return cryptoAccountCryptocurrencies ?? [];
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