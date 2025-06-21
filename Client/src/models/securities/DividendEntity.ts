import { SecurityEntity } from "./SecurityEntity";

export interface CommonDividendEntity {
    id: string,
    amount: number
}

export interface ServerDividendEntity extends CommonDividendEntity {
    declarationDate: string,
    snapshotDate: string,
    securityId: string,
}

export interface DividendEntity extends CommonDividendEntity {
    declarationDate: Date,
    snapshotDate: Date,
    security: SecurityEntity
}

