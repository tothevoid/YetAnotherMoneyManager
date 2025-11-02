import { SecurityEntity } from "../securities/SecurityEntity";

export interface BrokerAccountDailyStatsEntity {    
    fetchDate: Date;
    startPortfolioValue: number;
    currentPortfolioValue: number;
    brokerAccountDailySecurityStats: BrokerAccountDailySecurityStatsEntity[];
}

export interface BrokerAccountDailySecurityStatsEntity {
    security: SecurityEntity;
    startPrice: number;
    currentPrice: number;
}