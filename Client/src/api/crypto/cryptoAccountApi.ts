import config from '../../config' 
import { ClientCryptoAccountEntity, ServerCryptoAccountEntity } from '../../models/crypto/CryptoAccountEntity';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/CryptoAccount`;

export const getCryptoAccounts = async (): Promise<ClientCryptoAccountEntity[]> => {
   return await getAllEntities<ServerCryptoAccountEntity>(basicUrl)
        .then(cryptoAccounts => cryptoAccounts.map(prepareClientCryptoAccount));
};

export const createCryptoAccount = async (addedCryptoAccount: ClientCryptoAccountEntity): Promise<ClientCryptoAccountEntity | void> => {
    return await createEntity<ServerCryptoAccountEntity>(basicUrl, prepareServerCryptoAccount(addedCryptoAccount))
        .then(prepareClientCryptoAccount);
}

export const updateCryptoAccount = async (modifiedCryptoAccount: ClientCryptoAccountEntity): Promise<boolean> => {
    return await updateEntity<ServerCryptoAccountEntity>(basicUrl, prepareServerCryptoAccount(modifiedCryptoAccount));
}

export const deleteCryptoAccount = async (cryptoAccountId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, cryptoAccountId);
}

const prepareClientCryptoAccount = (cryptoAccount: ServerCryptoAccountEntity): ClientCryptoAccountEntity => {
    return {
        ...cryptoAccount
    };
}

const prepareServerCryptoAccount = (cryptoAccount: ClientCryptoAccountEntity): ServerCryptoAccountEntity => {
    const convertedSecurity: ServerCryptoAccountEntity = {
        id: cryptoAccount.id,
        name: cryptoAccount.name,
        cryptoProviderId: cryptoAccount.cryptoProvider.id
    };
    return convertedSecurity;
}