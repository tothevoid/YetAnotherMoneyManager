import { BrokerAccountTaxDeductionEntity, BrokerAccountTaxDeductionEntityRequest, BrokerAccountTaxDeductionEntityResponse, } from "../../models/brokers/BrokerAccountTaxDeductionEntity";
import { prepareBrokerAccount } from "./brokerAccountApiMapping";

export const prepareBrokerAccountTaxDeductionRequest = (entity: BrokerAccountTaxDeductionEntity): BrokerAccountTaxDeductionEntityRequest => {
    return {
        id: entity.id,
        name: entity.name,
        amount: entity.amount,
        appliedOn: entity.appliedOn,
        brokerAccountId: entity.brokerAccount.id,
    };
};

export const prepareBrokerAccountTaxDeductionResponse = (response: BrokerAccountTaxDeductionEntityResponse): BrokerAccountTaxDeductionEntity => {
    return {
        id: response.id,
        name: response.name,
        amount: response.amount,
        appliedOn: new Date(response.appliedOn),
        brokerAccount: prepareBrokerAccount(response.brokerAccount),
    };
};