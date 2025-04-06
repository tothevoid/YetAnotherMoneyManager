import { DepositMonthSummary } from '../components/DepositStats/depositMonthSummary';
import config from '../config' 
import { DepositEntity } from '../models/DepositEntity';
import { convertToInputDate } from '../utils/DateUtils';
import { checkPromiseStatus, logPromiseError } from '../utils/PromiseUtils';
import { DepositsRange } from './models/depositsRange';

const basicUrl = `${config.api.URL}/Deposit`;

export const getDeposits = async (monthsFrom: number, monthsTo: number, onlyActive: boolean): Promise<DepositEntity[]> => {
    const url = `${basicUrl}/GetAll`;
    const deposits = await fetch(url, {method: "POST", 
        body: getQueryDepositsQueryParamers(monthsFrom, monthsTo, onlyActive),
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then((deposits: DepositEntity[]) => 
            deposits.map((deposit: DepositEntity) => {
                const from = new Date(deposit.from);
                const to = new Date(deposit.to);
                return {...deposit, from, to} as DepositEntity;
            })
        )
        .catch(logPromiseError);
  
    return deposits ?
    deposits: 
        [] as DepositEntity[];
};

export const createDeposit = async (deposit: DepositEntity): Promise<DepositEntity | void> => {
    const newDeposit = await fetch(basicUrl, { method: "PUT", body: prepareDepositEntity(deposit),
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then(id => {
            return {...deposit, id} as DepositEntity;
        })
        .catch(logPromiseError);
    return newDeposit;
}

export const updateDeposit = async (modifiedDeposit: DepositEntity): Promise<boolean> => {
    const result = await fetch(basicUrl, { method: "PATCH", body: prepareDepositEntity(modifiedDeposit),  
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .catch(logPromiseError);

    return result?.ok ?? false;
}

export const deleteDeposit = async (depositId: string): Promise<boolean> => {
    if (!depositId) {
        return false;
    }

    const url = `${basicUrl}?id=${depositId}`;
    const result = await fetch(url, { method: "DELETE"})
        .then(checkPromiseStatus)
        .catch(logPromiseError);

    return result?.ok ?? false;
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
        body: getQueryDepositsQueryParamers(monthsFrom, monthsTo, onlyActive),
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
  
    return summary;
};

const prepareDepositEntity = (deposit: DepositEntity): string => {
    const serverDeposit = {...deposit};

    // .NET DateOnly cast
    serverDeposit.from = convertToInputDate(deposit.from);
    serverDeposit.to = convertToInputDate(deposit.to);

    return JSON.stringify(serverDeposit);
}

const getQueryDepositsQueryParamers = (monthsFrom: number, monthsTo: number, onlyActive: boolean) => {
    return JSON.stringify({monthsFrom, monthsTo, onlyActive});
}