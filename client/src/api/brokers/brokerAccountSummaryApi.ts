import { BrokerAccountDailyStatsEntity } from '../../models/brokers/BrokerAccountDailyStatsEntity';
import { BrokerAccountDayTransferEntity } from '../../models/brokers/BrokerAccountDayTransferEntity';
import { BrokerAccountMonthTransferEntity } from '../../models/brokers/BrokerAccountMonthTransferEntity';
import { BrokerAccountSummaryEntity } from '../../models/brokers/BrokerAccountSummaryEntity';
import { getAllEntities, getEntity } from '../basicApi';
import { prepareBrokerAccountsSecurityStats, prepareDailyStats } from './brokerAccountSummaryApiMapping';

const basicUrl = `/BrokerAccountSummary`;

export const getBrokerAccountStats = async (brokerAccountId: string, from: Date, to: Date): Promise<BrokerAccountSummaryEntity | void> => {
    const url = `${basicUrl}/GetSummary?brokerAccountId=${brokerAccountId}&from=${from.toISOString()}&to=${to.toISOString()}`;
    return getEntity<BrokerAccountSummaryEntity>(url)
        .then((data: BrokerAccountSummaryEntity | void) => data && prepareBrokerAccountsSecurityStats(data));
}

export const getMonthTransfersHistory = async (brokerAccountId: string, month: number, year: number): Promise<BrokerAccountDayTransferEntity[]> => {
    const url = `${basicUrl}/GetMonthTransfersHistory?brokerAccountId=${brokerAccountId}&month=${month}&year=${year}`;
    return await getAllEntities<BrokerAccountDayTransferEntity>(url)
}

export const getYearTransfersHistory = async (brokerAccountId: string, year: number): Promise<BrokerAccountMonthTransferEntity[]> => {
    const url = `${basicUrl}/GetYearTransfersHistory?brokerAccountId=${brokerAccountId}&year=${year}`;
    return await getAllEntities<BrokerAccountMonthTransferEntity>(url);
}

export const getDailyStats = async (brokerAccountId: string): Promise<BrokerAccountDailyStatsEntity | void> => {
    const url = `${basicUrl}/GetDailyStats?brokerAccountId=${brokerAccountId}`;
    return await getEntity<BrokerAccountDailyStatsEntity>(url)
        .then((data: BrokerAccountDailyStatsEntity | void) => data && prepareDailyStats(data));
}