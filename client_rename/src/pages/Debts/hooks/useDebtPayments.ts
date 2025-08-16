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
		const addedDebtPayment = await createDebtPayment(createdDebtPayment);
		
		if (!addedDebtPayment) {
			return;
		}

		setDebtPayments([addedDebtPayment, ...debtPayments]);
	}

	const updateDebtPaymentEntity = async (updatedDebtPayment: DebtPaymentEntity) => {
		const deptPaymentUpdated = await updateDebtPayment(updatedDebtPayment);
	
		if (!deptPaymentUpdated) {
			return;
		}

		const updatedDebtsPayments = debtPayments.map(existingDebtPayment => 
			updatedDebtPayment.id === existingDebtPayment.id ?
				{...updatedDebtPayment}:
				existingDebtPayment
		);

		setDebtPayments(updatedDebtsPayments);
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
