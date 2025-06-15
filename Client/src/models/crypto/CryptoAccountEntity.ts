import { CryptoProviderEntity } from "./CryptoProviderEntity";

interface CommonCryptoAccountEntity {
    id: string,
    name: string,
}

export interface ClientCryptoAccountEntity extends CommonCryptoAccountEntity {
    cryptoProvider: CryptoProviderEntity,
}

export interface ServerCryptoAccountEntity extends CommonCryptoAccountEntity {
    cryptoProviderId: string
}