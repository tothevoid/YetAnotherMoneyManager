export interface DepositMonthSummary {
    totalEarnings: number;
    averageMonthly: number;
    peakMonthPeriod: string | null;
    peakMonthValue: number;
    monthsCount: number;
    depositTotals: DepositSummaryItem[];
    payments: PeriodPayment[];
}

export interface DepositSummaryItem {
    depositId: string;
    name: string;
    totalValue: number;
}

export interface PeriodPayment {
    period: string;
    totalValue: number;
    payments: DepositPayment[];
}

export interface DepositPayment {
    depositId: string;
    name: string;
    value: number;
}