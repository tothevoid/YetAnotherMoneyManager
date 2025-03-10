export type DepositEntity = {
    id: string,
    name: string,
    from: Date,
    to: Date,
    percentage: number,
    initialAmount: number,
    estimatedEarn?: number | null
}