import config from '../../config' 
import { BrokerAccountDailyStatsEntity } from '../../models/brokers/BrokerAccountDailyStatsEntity';
import { BrokerAccountDayTransferEntity } from '../../models/brokers/BrokerAccountDayTransferEntity';
import { BrokerAccountMonthTransferEntity } from '../../models/brokers/BrokerAccountMonthTransferEntity';
import { BrokerAccountSummaryEntity } from '../../models/brokers/BrokerAccountSummaryEntity';
import { checkPromiseStatus } from '../../shared/utilities/webApiUtilities';
import { prepareBrokerAccountsSecurityStats, prepareDailyStats } from './brokerAccountSummaryApiMapping';

const basicUrl = `${config.api.URL}/BrokerAccountSummary`;

export const getBrokerAccountStats = async (brokerAccountId: string, from: Date, to: Date): Promise<BrokerAccountSummaryEntity> => {
    return await fetch(`${basicUrl}/GetSummary?brokerAccountId=${brokerAccountId}&from=${from.toISOString()}&to=${to.toISOString()}`, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then((data: BrokerAccountSummaryEntity) => prepareBrokerAccountsSecurityStats(data));
}

export const getMonthTransfersHistory = async (brokerAccountId: string, month: number, year: number): Promise<BrokerAccountDayTransferEntity[]> => {
    return await fetch(`${basicUrl}/GetMonthTransfersHistory?brokerAccountId=${brokerAccountId}&month=${month}&year=${year}`, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json()) as BrokerAccountDayTransferEntity[];
}

export const getYearTransfersHistory = async (brokerAccountId: string, year: number): Promise<BrokerAccountMonthTransferEntity[]> => {
    return await fetch(`${basicUrl}/GetYearTransfersHistory?brokerAccountId=${brokerAccountId}&year=${year}`, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json()) as BrokerAccountMonthTransferEntity[];
}

export const getDailyStats = async (brokerAccountId: string): Promise<BrokerAccountDailyStatsEntity> => {
    return await fetch(`${basicUrl}/GetDailyStats?brokerAccountId=${brokerAccountId}`, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then((data: BrokerAccountDailyStatsEntity) => prepareDailyStats(data));
}