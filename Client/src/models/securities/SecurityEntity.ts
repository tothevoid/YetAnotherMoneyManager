import { SecurityTypeEntity } from "./SecurityTypeEntity";

export interface SecurityEntity {
    id: string,
    name: string,
    ticker: string,
    type: SecurityTypeEntity
}