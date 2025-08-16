import { CryptoProviderEntity } from "./CryptoProviderEntity";

interface CommonCryptoAccountEntity {
    id: string,
    name: string,
}

export interface CryptoAccountEntityRequest extends CommonCryptoAccountEntity {
    cryptoProviderId: string
}

export interface CryptoAccountEntity extends CommonCryptoAccountEntity {
    cryptoProvider: CryptoProviderEntity
}

export interface CryptoAccountEntityResponse extends CommonCryptoAccountEntity {
    cryptoProvider: CryptoProviderEntity
}