import { SecurityEntity } from "../securities/SecurityEntity"

export interface SecurityStats {
    totalIncome: number
    totalDeposit: number
    totalWithdraw: number
    dailyStats: DailyStat[]
}

export interface DailyStat {
    security: SecurityEntity
    firstPrice: number
    lastPrice: number
    minPrice: number
    maxPrice: number
}