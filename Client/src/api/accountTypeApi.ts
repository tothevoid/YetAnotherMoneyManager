import config from '../config' 
import { AccountTypeEntity } from '../models/AccountTypeEntity';
import { TransactionEntity } from '../models/TransactionEntity';
import { checkPromiseStatus, logPromiseError } from '../utils/PromiseUtils';
import { AccountToUpdate } from './models/accountToUpdate';

const basicUrl = `${config.api.URL}/AccountType`;

export const getAccountTypes = async (): Promise<AccountTypeEntity[]> => {
    const accountTypes = await fetch(basicUrl, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
  
    return accountTypes ?
        accountTypes: 
        [] as AccountTypeEntity[];
};

export const createAccountType = async (accountType: AccountTypeEntity): Promise<AccountTypeEntity | void> => {
    const newAccountType = await fetch(basicUrl, { method: "PUT", body: JSON.stringify(accountType),
            headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then(id => {
            return {...accountType, id} as AccountTypeEntity;
        })
        .catch(logPromiseError);

    return newAccountType;
}

export const updateAccountType = async (accountType: AccountTypeEntity): Promise<boolean> => {
    if (!accountType) {
        return false;
    }

    const result = await fetch(basicUrl, { method: "PATCH", body: JSON.stringify(accountType),  
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .then(response => response.json())
        .catch(logPromiseError);

    return result?.ok ?? false;
}

export const deleteAccountType = async (accountTypeId: string): Promise<boolean> => {
    if (!accountTypeId) {
        return false;
    }

    const url = `${basicUrl}?id=${accountTypeId}`;
    const result = await fetch(url, { method: "DELETE"})
        .then(checkPromiseStatus)
        .catch(logPromiseError);

    return result?.ok ?? false;
}