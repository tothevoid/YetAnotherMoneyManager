import config from '../../config' 
import { ClientCryptoAccountCryptocurrencyEntity, ServerCryptoAccountCryptocurrencyEntity } from '../../models/crypto/CryptoAccountCryptocurrencyEntity';
import { checkPromiseStatus, logPromiseError } from '../../shared/utilities/webApiUtilities';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/CryptoAccountCryptocurrency`;

export const getCryptoAccountCryptocurrencies = async (): Promise<ClientCryptoAccountCryptocurrencyEntity[]> => {
   return await getAllEntities<ServerCryptoAccountCryptocurrencyEntity>(basicUrl)
        .then(cryptoAccounts => cryptoAccounts.map(prepareClientCryptoAccountCryptocurrency));
};

export const getCryptocurrenciesByCryptoAccount = async (cryptoAccountId: string): Promise<ClientCryptoAccountCryptocurrencyEntity[]> => {
    const entities = await fetch(`${basicUrl}/GetByCryptoAccount?cryptoAccountId=${cryptoAccountId}`, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
     
    return entities ?
        entities: 
        [] as ClientCryptoAccountCryptocurrencyEntity[];
};

export const createCryptoAccountCryptocurrency = async (addedCryptoAccountCryptocurrency: ClientCryptoAccountCryptocurrencyEntity): Promise<ClientCryptoAccountCryptocurrencyEntity | void> => {
    return await createEntity<ServerCryptoAccountCryptocurrencyEntity>(basicUrl, prepareServerCryptoAccountCryptocurrency(addedCryptoAccountCryptocurrency))
        .then(prepareClientCryptoAccountCryptocurrency);
}

export const updateCryptoAccountCryptocurrency = async (modifiedCryptoAccountCryptocurrency: ClientCryptoAccountCryptocurrencyEntity): Promise<boolean> => {
    return await updateEntity<ServerCryptoAccountCryptocurrencyEntity>(basicUrl, prepareServerCryptoAccountCryptocurrency(modifiedCryptoAccountCryptocurrency));
}

export const deleteCryptoAccountCryptocurrency = async (cryptoAccountCryptocurrencyId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, cryptoAccountCryptocurrencyId);
}

const prepareClientCryptoAccountCryptocurrency = (cryptoAccountCryptocurrency: ServerCryptoAccountCryptocurrencyEntity): ClientCryptoAccountCryptocurrencyEntity => {
    return {
        ...cryptoAccountCryptocurrency
    };
}

const prepareServerCryptoAccountCryptocurrency = (cryptoAccountCryptocurrency: ClientCryptoAccountCryptocurrencyEntity): ServerCryptoAccountCryptocurrencyEntity => {
    const convertedSecurity: ServerCryptoAccountCryptocurrencyEntity = {
        id: cryptoAccountCryptocurrency.id,
        name: cryptoAccountCryptocurrency.name,
        cryptoAccountId: cryptoAccountCryptocurrency.cryptoAccount.id,
        cryptocurrencyId: cryptoAccountCryptocurrency.cryptocurrency.id,
        quantity: cryptoAccountCryptocurrency.quantity
    };
    return convertedSecurity;
}