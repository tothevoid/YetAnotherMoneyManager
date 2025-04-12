import { AccountEntity } from "./AccountEntity"

export type DepositEntity = {
    id: string,
    name: string,
    from: Date,
    to: Date,
    percentage: number,
    initialAmount: number,
    estimatedEarn?: number | null
    account?: AccountEntity | null
}