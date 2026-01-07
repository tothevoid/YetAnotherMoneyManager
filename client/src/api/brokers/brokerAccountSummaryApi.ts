import { BrokerAccountDailyStatsEntity } from '../../models/brokers/BrokerAccountDailyStatsEntity';
import { BrokerAccountDayTransferEntity } from '../../models/brokers/BrokerAccountDayTransferEntity';
import { BrokerAccountMonthTransferEntity } from '../../models/brokers/BrokerAccountMonthTransferEntity';
import { BrokerAccountSummaryEntity } from '../../models/brokers/BrokerAccountSummaryEntity';
import { Nullable } from '../../shared/utilities/nullable';
import { getAllEntities, getEntity } from '../basicApi';
import { prepareBrokerAccountsSecurityStats, prepareDailyStats } from './brokerAccountSummaryApiMapping';

const basicUrl = `BrokerAccountSummary`;

export const getBrokerAccountStats = async (brokerAccountId: Nullable<string>): Promise<BrokerAccountSummaryEntity | void> => {
    const url = brokerAccountId ?
        `${basicUrl}/GetSummaryByBrokerAccount?brokerAccountId=${brokerAccountId}`:
        `${basicUrl}/GetSummary`;

    return getEntity<BrokerAccountSummaryEntity>(url)
        .then((data: BrokerAccountSummaryEntity | void) => data && prepareBrokerAccountsSecurityStats(data));
}

export const getMonthTransfersHistory = async (brokerAccountId: Nullable<string>, month: number, year: number): Promise<BrokerAccountDayTransferEntity[]> => {
    const url = brokerAccountId ? 
        `${basicUrl}/GetMonthTransfersHistoryByBrokerAccount?brokerAccountId=${brokerAccountId}&month=${month}&year=${year}` :
        `${basicUrl}/GetMonthTransfersHistory?month=${month}&year=${year}`;
    return await getAllEntities<BrokerAccountDayTransferEntity>(url)
}

export const getYearTransfersHistory = async (brokerAccountId: Nullable<string>, year: number): Promise<BrokerAccountMonthTransferEntity[]> => {
    const url = brokerAccountId ? 
        `${basicUrl}/GetYearTransfersHistoryByBrokerAccount?brokerAccountId=${brokerAccountId}&year=${year}` :
        `${basicUrl}/GetYearTransfersHistory?year=${year}`;
    return await getAllEntities<BrokerAccountMonthTransferEntity>(url);
}

export const getDailyStats = async (brokerAccountId: Nullable<string>): Promise<BrokerAccountDailyStatsEntity | void> => {
    const url = brokerAccountId ?
        `${basicUrl}/GetDailyStatsByBrokerAccount?brokerAccountId=${brokerAccountId}`:
        `${basicUrl}/GetDailyStats`;

    return await getEntity<BrokerAccountDailyStatsEntity>(url)
        .then((data: BrokerAccountDailyStatsEntity | void) => data && prepareDailyStats(data));
}