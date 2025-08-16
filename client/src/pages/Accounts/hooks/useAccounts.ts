import { useCallback, useEffect, useState } from "react";
import { AccountEntity } from "../../../models/accounts/AccountEntity";
import { createAccount, getAccounts } from "../../../api/accounts/accountApi";

export interface AccountsQuery {
	onlyActive: boolean
}

export const useAccounts = (queryParameters: AccountsQuery) => {
	const [accounts, setAccounts] = useState<AccountEntity[]>([]);
	const [isAccountsLoading, setLoading] = useState(false);

	const [error, setError] = useState<string | null>(null);
	const [accountQueryParameters, setAccountQueryParameters] = useState<AccountsQuery>(queryParameters);

	const fetchData = useCallback(async () => {
		setLoading(true)
		try {
			const accounts = await getAccounts(accountQueryParameters.onlyActive);
			setAccounts(accounts);
		} catch (err: any) {
			setError(err.message || 'Ошибка загрузки данных')
		} finally {
			setLoading(false)
		}
	}, [accountQueryParameters])

	useEffect(() => {
		fetchData();
	}, [fetchData])

	const createAccountEntity = async (account: AccountEntity) => {
		const createdAccountId = await createAccount(account);
		if (!createdAccountId) {
			return;
		}

		account.id = createdAccountId;
		setAccounts([...accounts, account]);
	}

	const updateAccountEntity = async (updatedAccount: AccountEntity) => {
	   if (!updatedAccount) {
			return;
		}

		const updatedAccounts = accounts.map((account: AccountEntity) => {
			if (account.id === updatedAccount.id) {
				return {...updatedAccount}
			} 
			return account
		});

		setAccounts(updatedAccounts)
	}

	const deleteAccountEntity = async (deletedAccount: AccountEntity) => {
		if (!deletedAccount) {
			return;
		}

		setAccounts(accounts.filter((account: AccountEntity) => account.id !== deletedAccount.id));
	}

	return {
		accounts,
		isAccountsLoading,
		error,
		createAccountEntity,
		updateAccountEntity,
		deleteAccountEntity,
		setAccountQueryParameters: setAccountQueryParameters,
		accountQueryParameters: accountQueryParameters,
		reloadAccounts: fetchData
	}
}
