import { BrokerAccountEntity, BrokerAccountEntityResponse } from "./BrokerAccountEntity"

interface CommonBrokerAccountTaxDeduction {
    id: string,
    amount: number,
    name: string
}

export interface BrokerAccountTaxDeductionEntityRequest extends CommonBrokerAccountTaxDeduction{
    brokerAccountId: string,
    dateApplied: Date
}

export interface BrokerAccountTaxDeductionEntity extends CommonBrokerAccountTaxDeduction {
    brokerAccount: BrokerAccountEntity,
    dateApplied: Date
}

export interface BrokerAccountTaxDeductionEntityResponse extends CommonBrokerAccountTaxDeduction{
    brokerAccount: BrokerAccountEntityResponse,
    dateApplied: string
}