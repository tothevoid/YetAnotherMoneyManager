import { AccountEntity, AccountEntityRequest, AccountEntityResponse } from "../../models/accounts/AccountEntity";
import { convertToDateOnly } from "../../shared/utilities/dateUtils";

export const prepareAccountRequest = (account: AccountEntity): AccountEntityRequest => {
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

export const prepareAccountEntity = (account: AccountEntityResponse): AccountEntity => {
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