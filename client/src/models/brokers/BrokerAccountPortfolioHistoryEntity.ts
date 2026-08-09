export interface BrokerAccountPortfolioHistoryEntity {
    date: string;
    mainCurrencyAmount: number;
    portfolioValue: number;
    totalDividends: number;
    totalTaxDeduction: number;
    totalDeposited: number;
    totalWithdraw: number;
    profitAndLoss: number;
}
