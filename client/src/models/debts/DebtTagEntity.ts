export interface DebtTagEntity {
    id: string;
    name: string;
    colorHex: string;
    usageCount?: number;
}

export interface DebtTagStatsEntity {
    tagId: string;
    tagName: string;
    colorHex: string;
    totalAmount: number;
    totalPaid: number;
    remainingAmount: number;
    repaymentPercentage: number;
    currencyName?: string;
}
