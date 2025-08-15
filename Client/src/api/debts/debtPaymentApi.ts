import config from "../../config";
import { DebtPaymentEntity, DebtPaymentEntityRequest, DebtPaymentEntityResponse } from "../../models/debts/DebtPaymentEntity";
import { createEntity, deleteEntity, getAllEntities, updateEntity } from "../basicApi";
import { prepareDebtPayment, prepareDebtPaymentRequest } from "./debtPaymentApiMapping";

const basicUrl = `${config.api.URL}/DebtPayment`;

export const getDebtPayments = async (): Promise<DebtPaymentEntity[]> =>  {
    return await getAllEntities<DebtPaymentEntityResponse>(basicUrl)
        .then(debtPayments => debtPayments.map(prepareDebtPayment))
}

export const createDebtPayment = async (newDebtPayment: DebtPaymentEntity): Promise<DebtPaymentEntity | void> => {
    return await createEntity<DebtPaymentEntityRequest, DebtPaymentEntityResponse>(basicUrl, prepareDebtPaymentRequest(newDebtPayment))
        .then((debtPayment) => debtPayment && prepareDebtPayment(debtPayment));
}

export const updateDebtPayment = async (updatedDebtPayment: DebtPaymentEntity): Promise<boolean> => {
    return await updateEntity(basicUrl, prepareDebtPaymentRequest(updatedDebtPayment));
}

export const deleteDebtPayment = async (debtPaymentId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, debtPaymentId);
}