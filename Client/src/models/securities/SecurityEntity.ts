import { SecurityTypeEntity } from "./SecurityTypeEntity";

export interface ServerSecurityEntity {
    id: string,
    name: string,
    ticker: string,
    typeId: string
}

export interface SecurityEntity extends ServerSecurityEntity {
    type: SecurityTypeEntity
}