import config from "../../config";
import { ClientDebtPaymentEntity, ServerDebtPaymentEntity } from "../../models/debts/DebtPaymentEntity";
import { convertToDateOnly } from "../../shared/utilities/dateUtils";
import { createEntity, deleteEntity, getAllEntities, updateEntity } from "../basicApi";

const basicUrl = `${config.api.URL}/DebtPayment`;

export const getDebtPayments = async (): Promise<ClientDebtPaymentEntity[]> =>  {
    return await getAllEntities<ServerDebtPaymentEntity>(basicUrl)
        .then(debtPayments => debtPayments.map(prepareClientDebtPayment))
}

export const createDebtPayment = async (newDebtPayment: ClientDebtPaymentEntity): Promise<ClientDebtPaymentEntity | void> => {
    return await createEntity(basicUrl, prepareServerDebtPayment(newDebtPayment))
        .then(prepareClientDebtPayment);
}

export const updateDebtPayment = async (updatedDebtPayment: ClientDebtPaymentEntity): Promise<boolean> => {
    return await updateEntity(basicUrl, prepareServerDebtPayment(updatedDebtPayment));
}

export const deleteDebtPayment = async (debtPaymentId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, debtPaymentId);
}

const prepareClientDebtPayment = (debtPayment: ServerDebtPaymentEntity): ClientDebtPaymentEntity => {
    return {
        ...debtPayment,
        date: new Date(debtPayment.date)
    };
}

const prepareServerDebtPayment = (debtPayment: ClientDebtPaymentEntity): ServerDebtPaymentEntity => {
    const convertedSecurity: ServerDebtPaymentEntity = {
        id: debtPayment.id,
        amount: debtPayment.amount,
        debtId: debtPayment.debt.id,
        targetAccountId: debtPayment.targetAccount.id,
        date: convertToDateOnly(debtPayment.date)
    };

    return convertedSecurity;
}