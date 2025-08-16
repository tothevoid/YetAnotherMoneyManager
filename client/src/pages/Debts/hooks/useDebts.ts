import { useCallback, useEffect, useState } from "react";
import { DebtEntity } from "../../../models/debts/DebtEntity";
import { createDebt, deleteDebt, getDebts, updateDebt } from "../../../api/debts/debtApi";

interface DebtPaymentsQueryParameters {
	onlyActive: boolean
}

export const useDebts = (queryParameters: DebtPaymentsQueryParameters) => {
	const [debts, setDebts] = useState<DebtEntity[]>([]);
	const [isDebtsLoading, setLoading] = useState(false);

	const [debtQueryParameters, setDebtQueryParameters] = useState<DebtPaymentsQueryParameters>(queryParameters);
	const [error, setError] = useState<string | null>(null);

	const fetchData = useCallback(async () => {
		if (!debtQueryParameters) {
			return;
		}

		setLoading(true)
		try {
			const debts = await getDebts(debtQueryParameters.onlyActive);
			setDebts(debts);
		} catch (err: any) {
			setError(err.message || 'Ошибка загрузки данных')
		} finally {
			setLoading(false)
		}
	}, [debtQueryParameters])

	useEffect(() => {
		fetchData();
	}, [fetchData])

	const createDebtEntity = async (createdDebt: DebtEntity) => {
		const addedDebt = await createDebt(createdDebt);
		if (!addedDebt) {
			return;
		}

		setDebts([addedDebt, ...debts])
	}

	const updateDebtEntity = async (updatedDebt: DebtEntity) => {
		const debtUpdated = await updateDebt(updatedDebt);
		if (!debtUpdated) {
			return;
		}

		const updatedDebts = debts.map(existingDebt => 
			updatedDebt.id === existingDebt.id ?
				{...updatedDebt}:
				existingDebt
		);

		setDebts(updatedDebts);
	}

	const deleteDebtEntity = async (deletedDebt: DebtEntity) => {
		const debtDeleted = await deleteDebt(deletedDebt.id);
		if (!debtDeleted) {
			return;
		}

		const updatedDebts = debts.filter(debt => debt.id !== deletedDebt.id);
		setDebts(updatedDebts);
	}

	return {
		debts,
		isDebtsLoading,
		error,
		createDebtEntity,
		updateDebtEntity,
		deleteDebtEntity,
		debtQueryParameters,
		setDebtQueryParameters
	}
}
