import { BrokerAccountEntity, BrokerAccountEntityResponse } from "./BrokerAccountEntity"

interface CommonBrokerAccountTaxDeduction {
    id: string,
    amount: number,
    name: string
}

export interface BrokerAccountTaxDeductionEntityRequest extends CommonBrokerAccountTaxDeduction{
    brokerAccountId: string,
    appliedOn: Date
}

export interface BrokerAccountTaxDeductionEntity extends CommonBrokerAccountTaxDeduction {
    brokerAccount: BrokerAccountEntity,
    appliedOn: Date
}

export interface BrokerAccountTaxDeductionEntityResponse extends CommonBrokerAccountTaxDeduction{
    brokerAccount: BrokerAccountEntityResponse,
    appliedOn: string
}