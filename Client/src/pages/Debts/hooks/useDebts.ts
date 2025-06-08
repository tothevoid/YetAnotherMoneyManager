import { useCallback, useEffect, useState } from "react";
import { ClientDebtEntity } from "../../../models/debts/DebtEntity";
import { createDebt, deleteDebt, getDebts, updateDebt } from "../../../api/debts/debtApi";

export const useDebts = () => {
	const [debts, setDebts] = useState<ClientDebtEntity[]>([]);
	const [isDebtsLoading, setLoading] = useState(false);

	const [error, setError] = useState<string | null>(null);

	const fetchData = useCallback(async () => {
		setLoading(true)
		try {
			const debts = await getDebts();
			setDebts(debts);
		} catch (err: any) {
			setError(err.message || 'Ошибка загрузки данных')
		} finally {
			setLoading(false)
		}
	}, [])

	useEffect(() => {
		fetchData();
	}, [fetchData])

	const createDebtEntity = async (createdDebt: ClientDebtEntity) => {
		const addedDebt = await createDebt(createdDebt);
		if (!addedDebt) {
			return;
		}

		setDebts([addedDebt, ...debts])
	}

	const updateDebtEntity = async (updatedDebt: ClientDebtEntity) => {
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

	const deleteDebtEntity = async (deletedDebt: ClientDebtEntity) => {
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
		deleteDebtEntity
	}
}
