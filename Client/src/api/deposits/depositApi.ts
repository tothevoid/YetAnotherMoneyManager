import { DepositMonthSummary } from '../../pages/Deposits/components/DepositStats/depositMonthSummary';
import config from '../../config' 
import { convertToDateOnly } from '../../shared/utilities/dateUtils';
import { checkPromiseStatus, logPromiseError } from '../../shared/utilities/webApiUtilities';
import { DepositsRange } from '../../models/deposits/depositsRange';
import { createEntity, deleteEntity, updateEntity } from '../basicApi';
import { DepositEntity, DepositEntityRequest, DepositEntityResponse } from '../../models/deposits/DepositEntity';
import { prepareDepositEntity, prepareDepositEntityRequest } from './depositApiMapping';

const basicUrl = `${config.api.URL}/Deposit`;

export const getDeposits = async (monthsFrom: number, monthsTo: number, onlyActive: boolean): Promise<DepositEntity[]> => {
    const url = `${basicUrl}/GetAll`;
    const deposits = await fetch(url, {method: "POST", 
        body: getQueryDepositsQueryParameters(monthsFrom, monthsTo, onlyActive),
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then((deposits: DepositEntityResponse[]) => deposits.map(prepareDepositEntity))
        .catch(logPromiseError);
  
    return deposits ?? [];
};

export const createDeposit = async (createdDeposit: DepositEntity): Promise<DepositEntity | void> => {
    return await createEntity<DepositEntityRequest, DepositEntityResponse>(basicUrl, prepareDepositEntityRequest(createdDeposit))
        .then((deposit) => deposit && prepareDepositEntity(deposit));
}

export const updateDeposit = async (modifiedDeposit: DepositEntity): Promise<boolean> => {
    return await updateEntity(basicUrl, prepareDepositEntityRequest(modifiedDeposit));
}

export const deleteDeposit = async (depositId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, depositId)
}

export const getDepositsRange = async (): Promise<DepositsRange | null> => {
    const url = `${basicUrl}/GetDepositsRange`;
    const result:  DepositsRange | null = await fetch(url, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);

    return result;
}

export const getDepositsSummary = async (monthsFrom: number, monthsTo: number, onlyActive: boolean): Promise<DepositMonthSummary | null> => {
    const url = `${basicUrl}/GetDepositsSummary`;
    const summary: DepositMonthSummary | null = await fetch(url, {method: "POST",  
        body: getQueryDepositsQueryParameters(monthsFrom, monthsTo, onlyActive),
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
  
    return summary;
};

const getQueryDepositsQueryParameters = (monthsFrom: number, monthsTo: number, onlyActive: boolean) => {
    return JSON.stringify({monthsFrom, monthsTo, onlyActive});
}