import { useCallback, useEffect, useState } from "react";
import { DebtPaymentEntity } from "../../../models/debts/DebtPaymentEntity";
import { createDebtPayment, deleteDebtPayment, getDebtPayments, updateDebtPayment } from "../../../api/debts/debtPaymentApi";

export const useDebtPayments = () => {
	const [debtPayments, setDebtPayments] = useState<DebtPaymentEntity[]>([]);
	const [isDebtPaymentsLoading, setLoading] = useState(false);

	const [error, setError] = useState<string | null>(null);

	const fetchData = useCallback(async () => {
		setLoading(true)
		try {
			const debts = await getDebtPayments();
			setDebtPayments(debts);
		} catch (err: any) {
			setError(err.message || 'Ошибка загрузки данных')
		} finally {
			setLoading(false)
		}
	}, [])

	useEffect(() => {
		fetchData();
	}, [fetchData])

	const createDebtPaymentEntity = async (createdDebtPayment: DebtPaymentEntity) => {
		const debtPayment = await createDebtPayment(createdDebtPayment);
		if (!debtPayment) {
			return;
		}

		await fetchData();
	}

	const updateDebtPaymentEntity = async (updatedDebtPayment: DebtPaymentEntity) => {
		const deptPaymentUpdated = await updateDebtPayment(updatedDebtPayment);
	
		if (!deptPaymentUpdated) {
			return;
		}

		await fetchData();
	}

	const deleteDebtPaymentEntity = async (deletedDebt: DebtPaymentEntity) => {
		const deleted = await deleteDebtPayment(deletedDebt.id);

		if (!deleted) {
			return;
		}

		const updatedDebtsPayments = debtPayments
			.filter(debtPayment => debtPayment.id !== deletedDebt.id);

		setDebtPayments(updatedDebtsPayments);
	}

	return {
		debtPayments,
		isDebtPaymentsLoading,
		error,
		createDebtPaymentEntity,
		updateDebtPaymentEntity,
		deleteDebtPaymentEntity
	}
}
