import { useCallback, useEffect, useState } from "react";
import { createSecurityTransaction, deleteSecurityTransaction, getSecurityTransactions, updateSecurityTransaction } from "../../../api/securities/securityTransactionApi";
import { SecurityTransactionEntity } from "../../../models/securities/SecurityTransactionEntity";

export interface SecurityTransactionsQuery {
	currentPage: number,
	pageSize: number,
	brokerAccountId: string
}

export const useSecurityTransactions = (queryParameters: SecurityTransactionsQuery) => {
	const [securityTransactions, setSecurityTransactions] = useState<SecurityTransactionEntity[]>([]);
	const [isSecurityTransactionsLoading, setLoading] = useState(false);

	const [error, setError] = useState<string | null>(null);
	const [securityTransactionsQueryParameters, setSecurityTransactionsQueryParameters] = useState<SecurityTransactionsQuery>(queryParameters);

	const fetchData = useCallback(async () => {
		setLoading(true)
		try {
			const securityTransactions = await getSecurityTransactions({
				brokerAccountId: securityTransactionsQueryParameters.brokerAccountId,
				recordsQuantity: securityTransactionsQueryParameters.pageSize,
				pageIndex: securityTransactionsQueryParameters.currentPage,
			})
			setSecurityTransactions(securityTransactions);
		} catch (err: any) {
			setError(err.message || 'Ошибка загрузки данных')
		} finally {
			setLoading(false)
		}
	}, [securityTransactionsQueryParameters])

	useEffect(() => {
		fetchData();
	}, [fetchData])

	const createSecurityTransactionEntity = async (createdSecurityTransaction: SecurityTransactionEntity) => {
		const securityTransaction = await createSecurityTransaction(createdSecurityTransaction);
		if (!securityTransaction) {
			return;
		}

		await fetchData();
	}

	const updatedSecurityTransactionEntity = async (updatedSecurityTransaction: SecurityTransactionEntity) => {
	    const securityTransactionUpdated = await updateSecurityTransaction(updatedSecurityTransaction);
		if (!securityTransactionUpdated) {
			return;
		}

		await fetchData();
	}

	const deleteSecurityTransactionEntity = async (deletedSecurityTransaction: SecurityTransactionEntity) => {
		const securityTransactionDeleted = await deleteSecurityTransaction(deletedSecurityTransaction.id);
		if (!securityTransactionDeleted) {
			return;
		}

		await fetchData();
	}

	return {
		securityTransactions,
		isSecurityTransactionsLoading,
		error,
		createSecurityTransactionEntity,
		updatedSecurityTransactionEntity,
		deleteSecurityTransactionEntity,
		securityTransactionsQueryParameters, 
		setSecurityTransactionsQueryParameters,
		reloadSecurityTransactions: fetchData
	}
}
