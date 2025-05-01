import config from "../../config";
import { Transfer } from "../../modals/AccountBalanceTransferModal/AccountBalanceTransferModal";
import { AccountEntity, ServerAccountEntity } from "../../models/accounts/AccountEntity";
import { convertToDateOnly } from "../../utils/DateUtils";
import { checkPromiseStatus, logPromiseError } from "../../utils/PromiseUtils";
import { AccountCurrencySummary } from "../../models/accounts/accountsSummary";
import { createEntity, deleteEntity, updateEntity } from "../basicApi";

const basicUrl = `${config.api.URL}/Account`;

export const getAccounts = async (onlyActive: boolean = false): Promise<AccountEntity[]> =>  {
    const accounts: ServerAccountEntity[] = await fetch(`${basicUrl}/GetAll`, {
        method: "POST", 
        body: JSON.stringify({onlyActive}),
        headers: {"Content-Type": "application/json"}
    })
    .then(checkPromiseStatus)
    .then((response: Response) => response.json())
    .catch(logPromiseError);

    return accounts ? accounts.map(prepareClientAccount) : [] as AccountEntity[];
}

export const createAccount = async (newAccount: AccountEntity): Promise<string | void> => {
    const createdEntity = await createEntity(basicUrl, prepareServerAccount(newAccount));
    return createdEntity?.id;
}

export const updateAccount = async (modifiedAccount: AccountEntity): Promise<boolean> => {
    return await updateEntity(basicUrl, prepareServerAccount(modifiedAccount));
}

export const deleteAccount = async (accountId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, accountId);
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

const prepareServerAccount = (account: AccountEntity): ServerAccountEntity => {
    return {...account,
        id: account.id,
        active: account.active,
        balance: account.balance,
        name: account.name,
        createdOn: convertToDateOnly(account.createdOn),
        accountTypeId: account.accountType.id,
        currencyId: account.currency.id
    };
}

const prepareClientAccount = (account: ServerAccountEntity): AccountEntity => {
    const serverDeposit: AccountEntity = {...account,
        createdOn: new Date(account.createdOn)
    };
    return serverDeposit
}