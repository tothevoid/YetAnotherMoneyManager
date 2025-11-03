export interface GlobalDashboard {
	transactionsGlobalDashboard: TransactionsGlobalDashboard,
	brokerAccountsGlobalDashboard: BrokerAccountsGlobalDashboard,
	accountsGlobalDashboard: AccountsGlobalDashboard,
	debtsGlobalDashboard: DebtsGlobalDashboard,
	depositsGlobalDashboard: DepositsGlobalDashboard,
	cryptoAccountsGlobalDashboard: CryptoAccountsGlobalDashboard,
	banksGlobalDashboard: BanksGlobalDashboard,
	total: number
}

export interface TransactionsGlobalDashboard {
	spentsTotal: number,
	incomesTotal: number,
	spentsDistribution: DistributionModel[],
	incomesDistribution: DistributionModel[],
}

export interface BrokerAccountsGlobalDashboard {
	total: number,
	distribution: DistributionModel[],
}

export interface CryptoAccountsGlobalDashboard {
	total: number,
	distribution: DistributionModel[],
}

export interface AccountsGlobalDashboard {
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

export interface DebtsGlobalDashboard {
	total: number,
	distribution: DistributionModel[],
}

export interface DepositsGlobalDashboard {
	totalStartedAmount: number,
	totalEarned: number,
	startedAmountDistribution: DistributionModel[],
	earningsDistribution: DistributionModel[]
}

export interface BanksGlobalDashboard { 
	distribution: DistributionModel[],
}