import { SecurityEntity } from "../securities/SecurityEntity"

export interface BrokerAccountSummaryEntity {
    transferStats: BrokerAccountTransfersStats;
    brokerAccountStats: BrokerAccountStats;
}

export interface BrokerAccountStats {
    securityStats: BrokerAccountSecurityStats[];
    investedValue: number;
    currentValue: number;
    totalDividendsValue: number;
}

export interface BrokerAccountSecurityStats {
    security: SecurityEntity;
    firstPrice: number;
    lastPrice: number;
    minPrice: number;
    maxPrice: number;
}

export interface BrokerAccountTransfersStats {
    totalDeposited: number;
    totalWithdrawn: number;
}