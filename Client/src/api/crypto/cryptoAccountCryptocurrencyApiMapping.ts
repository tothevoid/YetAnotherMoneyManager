import { CryptoAccountCryptocurrencyEntityResponse, CryptoAccountCryptocurrencyEntity, CryptoAccountCryptocurrencyEntityRequest } from "../../models/crypto/CryptoAccountCryptocurrencyEntity";

export const prepareCryptoAccountCryptocurrency = (cryptoAccountCryptocurrency: CryptoAccountCryptocurrencyEntityResponse): CryptoAccountCryptocurrencyEntity => {
    return {
        id: cryptoAccountCryptocurrency.id,
        quantity: cryptoAccountCryptocurrency.quantity,
        cryptoAccount: cryptoAccountCryptocurrency.cryptoAccount,
        cryptocurrency: cryptoAccountCryptocurrency.cryptocurrency 
    }
}

export const prepareCryptoAccountCryptocurrencyRequest = (cryptoAccountCryptocurrency: CryptoAccountCryptocurrencyEntity): CryptoAccountCryptocurrencyEntityRequest => {
    return {
        id: cryptoAccountCryptocurrency.id,
        cryptoAccountId: cryptoAccountCryptocurrency.cryptoAccount.id,
        cryptocurrencyId: cryptoAccountCryptocurrency.cryptocurrency.id,
        quantity: cryptoAccountCryptocurrency.quantity
    };
}