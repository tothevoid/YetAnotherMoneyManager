import { DistributionModel } from "../dashboard/DashboardEntity";

export interface CryptoAccountStatsEntity {
    cryptoDistribution: DistributionModel[];
    accountsDistribution: DistributionModel[];
}
