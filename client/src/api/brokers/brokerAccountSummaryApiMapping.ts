import { BrokerAccountSummaryEntity } from "../../models/brokers/BrokerAccountSummaryEntity";

export const prepareBrokerAccountsSecurityStats = (brokerAccountSecurity: BrokerAccountSummaryEntity): BrokerAccountSummaryEntity => {
    return {
        ...brokerAccountSecurity
    };
}