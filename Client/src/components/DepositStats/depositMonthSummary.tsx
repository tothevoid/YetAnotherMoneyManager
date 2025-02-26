export interface DepositMonthSummary {
    deposits: string[],
    payments: PeriodPayment[]
}

export interface PeriodPayment
{
    period: string,
    payments: DepositPayment[]
}

export interface DepositPayment {
    name: string,
    value: number
}