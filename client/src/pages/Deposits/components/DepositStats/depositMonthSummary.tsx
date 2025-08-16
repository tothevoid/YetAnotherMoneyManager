export interface DepositMonthSummary {
	payments: PeriodPayment[]
}

export interface PeriodPayment {
	period: string,
	payments: DepositPayment[]
}

export interface DepositPayment {
	depositId: string
	name: string,
	value: number
}