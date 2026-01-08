import { BrokerAccountTaxDeductionEntity, BrokerAccountTaxDeductionEntityResponse } from "../../models/brokers/BrokerAccountTaxDeductionEntity";
import { TaxDecutionsQuery } from "../../pages/BrokerAccount/hooks/useBrokerAccountTaxDeductions";
import { createEntity, deleteEntity, getAllEntitiesByConfig, getEntity, updateEntity } from "../basicApi";
import { prepareBrokerAccountTaxDeductionRequest, prepareBrokerAccountTaxDeductionResponse } from "./BrokerAccountTaxDeductionApiMapping";

const basicUrl = `BrokerAccountTaxDeduction`;

export const getBrokerAccountTaxDeductions = async (query: TaxDecutionsQuery): Promise<BrokerAccountTaxDeductionEntity[]> => {
    return await getAllEntitiesByConfig<TaxDecutionsQuery, BrokerAccountTaxDeductionEntityResponse>(`${basicUrl}/GetAll`, query)
        .then((data) => data.map(prepareBrokerAccountTaxDeductionResponse));
};

export const createBrokerAccountTaxDeduction = async (addedTaxDeduction: BrokerAccountTaxDeductionEntity): Promise<BrokerAccountTaxDeductionEntity | void> => {
    return await createEntity<any, any>(basicUrl, prepareBrokerAccountTaxDeductionRequest(addedTaxDeduction))
        .then(prepareBrokerAccountTaxDeductionResponse);
};

export const getAmountByBrokerAccount = async (brokerAccountId: string): Promise<number> => {
    return await getEntity<number>(`${basicUrl}/GetAmountByBrokerAccount?brokerAccountId=${brokerAccountId}`) ?? 0;   
};

export const updateBrokerAccountTaxDeduction = async (updatedTaxDeduction: BrokerAccountTaxDeductionEntity): Promise<boolean> => {
    return await updateEntity(basicUrl, prepareBrokerAccountTaxDeductionRequest(updatedTaxDeduction));
};

export const deleteBrokerAccountTaxDeduction = async (taxDeductionId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, taxDeductionId);
};