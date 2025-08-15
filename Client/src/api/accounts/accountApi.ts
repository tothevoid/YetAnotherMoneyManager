import config from "../../config";
import { AccountEntity, AccountEntityRequest, AccountEntityResponse } from "../../models/accounts/AccountEntity";
import { convertToDateOnly } from "../../shared/utilities/dateUtils";
import { checkPromiseStatus, logPromiseError } from "../../shared/utilities/webApiUtilities";
import { AccountCurrencySummary } from "../../models/accounts/accountsSummary";
import { createEntity, deleteEntity, updateEntity } from "../basicApi";
import { Transfer } from "../../pages/Accounts/modals/AccountBalanceTransferModal/AccountBalanceTransferModal";

const basicUrl = `${config.api.URL}/Account`;

export const getAccounts = async (onlyActive: boolean = false): Promise<AccountEntity[]> =>  {
	const accounts = await fetch(`${basicUrl}/GetAll`, {
			method: "POST", 
			body: JSON.stringify({onlyActive}),
			headers: {"Content-Type": "application/json"}
		})
		.then(checkPromiseStatus)
		.then((response: Response) => response.json())
		.then((accountResponses: AccountEntityResponse[]) => accountResponses.map(prepareAccountEntity))
		.catch(logPromiseError);

	return accounts ?? []
}

export const getAccountsByTypes = async (typesIds: string[], onlyActive: boolean = false): Promise<AccountEntity[]> =>  {
	const accounts = await fetch(`${basicUrl}/GetAllByTypes`, {
			method: "POST", 
			body: JSON.stringify({onlyActive, typesIds}),
			headers: {"Content-Type": "application/json"}
		})
		.then(checkPromiseStatus)
		.then((response: Response) => response.json())
		.then((accountResponses: AccountEntityResponse[]) => accountResponses.map(prepareAccountEntity))
		.catch(logPromiseError);

	return accounts ?? [];
}

export const createAccount = async (newAccount: AccountEntity): Promise<string | void> => {
	const createdEntity = await createEntity<AccountEntityRequest, AccountEntityResponse>(basicUrl, prepareAccountRequest(newAccount));
	return createdEntity?.id;
}

export const updateAccount = async (modifiedAccount: AccountEntity): Promise<boolean> => {
	return await updateEntity(basicUrl, prepareAccountRequest(modifiedAccount));
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

const prepareAccountRequest = (account: AccountEntity): AccountEntityRequest => {
	return {
		id: account.id,
		active: account.active,
		balance: account.balance,
		name: account.name,
		createdOn: convertToDateOnly(account.createdOn),
		accountTypeId: account.accountType.id,
		currencyId: account.currency.id
	};
}

const prepareAccountEntity = (account: AccountEntityResponse): AccountEntity => {
	return {
		id: account.id,
		name: account.name,
		active: account.active,
		balance: account.balance,
		accountType: account.accountType,
		currency: account.currency,
		createdOn: new Date(account.createdOn)
	};
}