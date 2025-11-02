export interface BrokerAccountSummaryEntity {
    transferStats: BrokerAccountTransfersStats;
    brokerAccountStats: BrokerAccountStats;
}

export interface BrokerAccountStats {
    investedValue: number;
    currentValue: number;
    totalDividendsValue: number;
}

export interface BrokerAccountTransfersStats {
    totalDeposited: number;
    totalWithdrawn: number;
}