import config from '../../config' 
import { BrokerAccountSummaryEntity } from '../../models/brokers/BrokerAccountSummaryEntity';
import { checkPromiseStatus } from '../../shared/utilities/webApiUtilities';
import { prepareBrokerAccountsSecurityStats } from './brokerAccountSummaryApiMapping';

const basicUrl = `${config.api.URL}/BrokerAccount`;

export const getBrokerAccountStats = async (brokerAccountId: string, from: Date, to: Date): Promise<BrokerAccountSummaryEntity> => {
    return await fetch(`${basicUrl}/GetSummary?brokerAccountId=${brokerAccountId}&from=${from.toISOString()}&to=${to.toISOString()}`, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then((data: BrokerAccountSummaryEntity) => prepareBrokerAccountsSecurityStats(data));
}