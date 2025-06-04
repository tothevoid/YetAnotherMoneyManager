import config from "../../config";
import { ClientDebtEntity, ServerDebtEntity } from "../../models/debts/DebtEntity";
import { convertToDateOnly } from "../../shared/utilities/dateUtils";
import { createEntity, deleteEntity, getAllEntities, updateEntity } from "../basicApi";

const basicUrl = `${config.api.URL}/Debt`;

export const getDebts = async (): Promise<ClientDebtEntity[]> =>  {
    return await getAllEntities<ServerDebtEntity>(basicUrl)
        .then(debts => debts.map(prepareClientDebt))
}

export const createDebt = async (newDebt: ClientDebtEntity): Promise<ClientDebtEntity | void> => {
    return await createEntity(basicUrl, prepareServerDebt(newDebt))
        .then(prepareClientDebt);
}

export const updateDebt = async (updatedDept: ClientDebtEntity): Promise<boolean> => {
    return await updateEntity(basicUrl, prepareServerDebt(updatedDept));
}

export const deleteDebt = async (debtId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, debtId);
}

const prepareClientDebt = (debt: ServerDebtEntity): ClientDebtEntity => {
    return {
        ...debt,
        date: new Date(debt.date),
        paidOn: new Date(debt.paidOn)
    };
}

const prepareServerDebt = (debt: ClientDebtEntity): ServerDebtEntity => {
    const convertedSecurity: ServerDebtEntity = {
        id: debt.id,
        name: debt.name,
        amount: debt.amount,
        currencyId: debt.currency.id
    };

    if (debt.date) {
        convertedSecurity.date = convertToDateOnly(debt.date);
    }

    if (debt.paidOn) {
        convertedSecurity.paidOn = convertToDateOnly(debt.paidOn);
    }

    return convertedSecurity;
}