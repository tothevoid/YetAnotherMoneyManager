import { SecurityTypeEntity } from "./SecurityTypeEntity";

export interface CommonSecurityEntity {
    id: string,
    name: string,
    ticker: string,
    actualPrice: number,
}

export interface ServerSecurityEntity extends CommonSecurityEntity {
    typeId: string,
    priceFetchedAt: string
}

export interface SecurityEntity extends CommonSecurityEntity {
    type: SecurityTypeEntity,
    priceFetchedAt: Date
}