export interface Dashboard {
    transactionStats: TransactionStats,
    brokerAccountStats: BrokerAccountStats,
    accountStats: AccountStats,
    total: number
}

export interface TransactionStats {
    spentsTotal: number,
    incomesTotal: number,
    spentsDistribution: DistributionModel[],
    incomesDistribution: DistributionModel[],
}

export interface BrokerAccountStats {
    total: number,
    distribution: DistributionModel[],
}

export interface AccountStats {
    total: number,
    totalCash: number,
    totalDeposit: number,
    totalBankAccount: number,
    cashDistribution: DistributionModel[],
    depositsDistribution: DistributionModel[],
    bankAccountsDistribution: DistributionModel[],
}

export interface DistributionModel
{
    name: string,
    currency: string,
    amount: number,
    convertedAmount: number
}