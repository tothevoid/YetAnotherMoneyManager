import config from "../../config";
import { DebtPaymentEntity, DebtPaymentEntityRequest, DebtPaymentEntityResponse } from "../../models/debts/DebtPaymentEntity";
import { DebtPaymentsQuery } from "../../pages/Debts/hooks/useDebtPayments";
import { PaginationConfig } from "../../shared/models/PaginationConfig";
import { checkPromiseStatus, logPromiseError } from "../../shared/utilities/webApiUtilities";
import { createEntity, deleteEntity, getAllEntitiesByConfig, updateEntity } from "../basicApi";
import { prepareDebtPayment, prepareDebtPaymentRequest } from "./debtPaymentApiMapping";

const basicUrl = `${config.api.URL}/DebtPayment`;

export const getDebtPayments = async (query: DebtPaymentsQuery): Promise<DebtPaymentEntity[]> =>  {
    return await getAllEntitiesByConfig<DebtPaymentsQuery, DebtPaymentEntityResponse>(`${basicUrl}/GetAll`, query)
        .then((debtPayment) => {
            return debtPayment.map(prepareDebtPayment)
        });
}

export const getDebtPaymentsPagination = async (): Promise<PaginationConfig | void> => {
    return await fetch(`${basicUrl}/GetPagination`, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
};

export const createDebtPayment = async (newDebtPayment: DebtPaymentEntity): Promise<boolean | void> => {
    const addedEntity = await createEntity<DebtPaymentEntityRequest, DebtPaymentEntityResponse>(basicUrl, prepareDebtPaymentRequest(newDebtPayment));
    return !!addedEntity;
}

export const updateDebtPayment = async (updatedDebtPayment: DebtPaymentEntity): Promise<boolean> => {
    return await updateEntity(basicUrl, prepareDebtPaymentRequest(updatedDebtPayment));
}

export const deleteDebtPayment = async (debtPaymentId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, debtPaymentId);
}