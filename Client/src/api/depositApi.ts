import config from '../config' 
import { DepositEntity } from '../models/DepositEntity';
import { checkPromiseStatus, logPromiseError } from '../utils/PromiseUtils';

export const getDeposits = async (): Promise<DepositEntity[]> => {
    const url = `${config.api.URL}/Deposit`;
    const deposits = await fetch(url, {method: "GET"})
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
    const url = `${config.api.URL}/Deposit`;
    const newDeposit = await fetch(url, { method: "PUT", body: JSON.stringify(deposit),
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
    const url = `${config.api.URL}/Deposit`;
    const result = await fetch(url, { method: "PATCH", body: JSON.stringify(modifiedDeposit),  
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .catch(logPromiseError);

    return result?.ok ?? false;
}

export const deleteDeposit = async (deletedDeposit: DepositEntity): Promise<boolean> => {
    const url = `${config.api.URL}/Deposit`;
    const result = await fetch(url, { method: "DELETE", body: JSON.stringify(deletedDeposit), 
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .catch(logPromiseError);

    return result?.ok ?? false;
}