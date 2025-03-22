import config from "../config";
import { Transfer } from "../modals/AccountBalanceTransferModal/AccountBalanceTransferModal";
import { AccountEntity, ServerAccountEntity } from "../models/AccountEntity";
import { convertToInputDate } from "../utils/DateUtils";
import { checkPromiseStatus, logPromiseError } from "../utils/PromiseUtils";
import { AccountCurrencySummary } from "./models/accountsSummary";

const basicUrl = `${config.api.URL}/Account`;

export const getAccounts = async (): Promise<AccountEntity[]> =>  {
    const accounts: ServerAccountEntity[] = await fetch(basicUrl, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);

    return accounts ? accounts.map(prepareClientAccount) : [] as AccountEntity[];
}

export const createAccount = async (newAccount: AccountEntity): Promise<string | void> => {
    const addedAccountId: string | void = await fetch(basicUrl, { method: "PUT", body: prepareServerAccount(newAccount), 
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())

    return addedAccountId;
}

export const updateAccount = async (modifiedAccount: AccountEntity): Promise<boolean> => {
    const result = await fetch(basicUrl, { method: "PATCH", body: prepareServerAccount(modifiedAccount),  
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

export const getSummary = async (): Promise<AccountCurrencySummary[]> => {
    const summaries: AccountCurrencySummary[] = await fetch(`${basicUrl}/GetSummary`, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError)

    return summaries ? summaries: [] as AccountCurrencySummary[];
}

const prepareServerAccount = (account: AccountEntity): string => {
    const serverDeposit: ServerAccountEntity = {...account,
        // .NET DateOnly cast
        createdOn: convertToInputDate(account.createdOn)
    };
    return JSON.stringify(serverDeposit);
}

const prepareClientAccount = (account: ServerAccountEntity): AccountEntity => {
    const serverDeposit: AccountEntity = {...account,
        createdOn: new Date(account.createdOn)
    };
    return serverDeposit
}