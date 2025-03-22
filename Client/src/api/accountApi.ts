import config from "../config";
import { Transfer } from "../modals/AccountBalanceTransferModal/AccountBalanceTransferModal";
import { AccountEntity } from "../models/AccountEntity";
import { checkPromiseStatus, logPromiseError } from "../utils/PromiseUtils";

const basicUrl = `${config.api.URL}/Account`;

export const getAccounts = async (): Promise<AccountEntity[]> =>  {
    const accounts = await fetch(basicUrl, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);

    return accounts ? accounts : [] as AccountEntity[];
}

export const createAccount = async (newAccount: AccountEntity): Promise<string | void> => {
    const {id, ...fieldsExceptId} = newAccount;

    const addedAccountId: string | void = await fetch(basicUrl, { method: "PUT", body: JSON.stringify(fieldsExceptId), 
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())

    return addedAccountId;
}

export const updateAccount = async (modifiedAccount: AccountEntity): Promise<boolean> => {
    const result = await fetch(basicUrl, { method: "PATCH", body: JSON.stringify(modifiedAccount),  
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .catch(logPromiseError)

    return result?.ok ?? false;
}

export const deleteAccount = async (accountId: string): Promise<boolean> => {
    if (!accountId) {
        return false;
    }

    const url = `${basicUrl}?id=${accountId}`;
    const result = await fetch(url, { method: "DELETE"})
        .then(checkPromiseStatus)
        .catch(logPromiseError)

    return result?.ok ?? false;
}

export const transferBalance = async (transfer: Transfer): Promise<boolean> => {
    if (!transfer) {
        return false;
    }

    const requestTransfer = {
        ...transfer,
        from: transfer.from.id,
        to: transfer.to.id,
    }

    const result = await fetch(`${basicUrl}/Transfer`, { method: "POST", body: JSON.stringify(requestTransfer),  
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .catch(logPromiseError)

    return result?.ok ?? false;
}