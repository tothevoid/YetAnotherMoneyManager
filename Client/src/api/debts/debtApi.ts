import config from "../../config";
import { DebtEntity, DebtEntityRequest, DebtEntityResponse } from "../../models/debts/DebtEntity";
import { convertToDateOnly } from "../../shared/utilities/dateUtils";
import { checkPromiseStatus, logPromiseError } from "../../shared/utilities/webApiUtilities";
import { createEntity, deleteEntity, updateEntity } from "../basicApi";

const basicUrl = `${config.api.URL}/Debt`;

export const getDebts = async (onlyActive: boolean): Promise<DebtEntity[]> =>  {
    const debts = await fetch(`${basicUrl}/GetAll?onlyActive=${onlyActive}`, {
            method: "GET", 
        })
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then((debtsResponses: DebtEntityResponse[]) => debtsResponses.map(prepareDebt))
        .catch(logPromiseError);
    return debts ?? [];
}

export const createDebt = async (newDebt: DebtEntity): Promise<DebtEntity | void> => {
    return await createEntity<DebtEntityRequest, DebtEntityResponse>(basicUrl, prepareDebtRequest(newDebt))
        .then((debt) => debt && prepareDebt(debt));
}

export const updateDebt = async (updatedDept: DebtEntity): Promise<boolean> => {
    return await updateEntity(basicUrl, prepareDebtRequest(updatedDept));
}

export const deleteDebt = async (debtId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, debtId);
}

const prepareDebt = (debt: DebtEntityResponse): DebtEntity => {
    return {
        id: debt.id,
        amount: debt.amount,
        name: debt.name,
        currency: debt.currency,
        date: new Date(debt.date),
        paidOn: new Date(debt.paidOn)
    };
}

const prepareDebtRequest = (debt: DebtEntity): DebtEntityRequest => {
    return {
        id: debt.id,
        name: debt.name,
        amount: debt.amount,
        currencyId: debt.currency.id,
        date: convertToDateOnly(debt.date),
        paidOn: convertToDateOnly(debt.paidOn)
    };
}