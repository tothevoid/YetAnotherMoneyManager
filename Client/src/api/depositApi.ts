import { DepositMonthSummary } from '../components/DepositStats/depositMonthSummary';
import config from '../config' 
import { DepositEntity } from '../models/DepositEntity';
import { convertToInputDate } from '../utils/DateUtils';
import { checkPromiseStatus, logPromiseError } from '../utils/PromiseUtils';

const basicUrl = `${config.api.URL}/Deposit`;

export const getDeposits = async (): Promise<DepositEntity[]> => {
    const deposits = await fetch(basicUrl, {method: "GET"})
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

export const getDepositsSummary = async (): Promise<DepositMonthSummary | null> => {
    const url = `${basicUrl}/GetDepositsSummary`;
    const summary: DepositMonthSummary | null = await fetch(url, {method: "GET"})
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