export interface Dashboard {
	transactionStats: TransactionStats,
	brokerAccountStats: BrokerAccountStats,
	accountStats: AccountStats,
	debtStats: DebtStats,
	depositStats: DepositStats
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
	totalBankAccount: number,
	cashDistribution: DistributionModel[],
	bankAccountsDistribution: DistributionModel[],
}

export interface DistributionModel
{
	name: string,
	currency: string,
	amount: number,
	convertedAmount: number
}

export interface DebtStats {
	total: number,
	distribution: DistributionModel[],
}

export interface DepositStats {
	totalStartedAmount: number,
	totalEarned: number,
	startedAmountDistribution: DistributionModel[],
	earningsDistribution: DistributionModel[]
}