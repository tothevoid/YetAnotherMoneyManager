import { CryptoAccountEntity, CryptoAccountEntityResponse } from "./CryptoAccountEntity";
import { CryptocurrencyEntity } from "./CryptocurrencyEntity";

interface CommonCryptoAccountCryptocurrencyEntity {
    id: string,
    quantity: number
}

export interface CryptoAccountCryptocurrencyEntityRequest extends CommonCryptoAccountCryptocurrencyEntity {
    cryptocurrencyId: string,
    cryptoAccountId: string,
}

export interface CryptoAccountCryptocurrencyEntity extends CommonCryptoAccountCryptocurrencyEntity {
    cryptocurrency: CryptocurrencyEntity,
    cryptoAccount: CryptoAccountEntity
}

export interface CryptoAccountCryptocurrencyEntityResponse extends CommonCryptoAccountCryptocurrencyEntity {
    cryptocurrency: CryptocurrencyEntity,
    cryptoAccount: CryptoAccountEntityResponse
}