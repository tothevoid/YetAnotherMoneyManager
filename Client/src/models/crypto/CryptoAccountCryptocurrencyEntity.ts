import { ClientCryptoAccountEntity } from "./CryptoAccountEntity";
import { CryptocurrencyEntity } from "./CryptocurrencyEntity";

interface CommonCryptoAccountCryptocurrencyEntity {
    id: string,
    quantity: number
}

export interface ClientCryptoAccountCryptocurrencyEntity extends CommonCryptoAccountCryptocurrencyEntity {
    cryptocurrency: CryptocurrencyEntity,
    cryptoAccount: ClientCryptoAccountEntity,
}

export interface ServerCryptoAccountCryptocurrencyEntity extends CommonCryptoAccountCryptocurrencyEntity {
    cryptocurrencyId: string,
    cryptoAccountId: string,
}