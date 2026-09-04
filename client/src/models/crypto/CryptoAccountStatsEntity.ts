import { DistributionModel } from "../dashboard/DashboardEntity";

export interface CryptoAccountStatsEntity {
    totalUsd: number;
    totalConverted: number;
    mainCurrency: string;
    cryptoDistribution: DistributionModel[];
    accountsDistribution: DistributionModel[];
}
