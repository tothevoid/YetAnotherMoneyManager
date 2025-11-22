import { useCallback, useEffect, useState } from "react";
import { DebtPaymentEntity } from "../../../models/debts/DebtPaymentEntity";
import { createDebtPayment, deleteDebtPayment, getDebtPayments, updateDebtPayment } from "../../../api/debts/debtPaymentApi";

export interface DebtPaymentsQuery {
	pageIndex: number,
	recordsQuantity: number
}

export const useDebtPayments = (queryParameters: DebtPaymentsQuery) => {
	const [debtPayments, setDebtPayments] = useState<DebtPaymentEntity[]>([]);
	const [isDebtPaymentsLoading, setLoading] = useState(false);

	const [debtPaymentsQueryParameters, setDebtPaymentsQueryParameters] = useState<DebtPaymentsQuery>(queryParameters);
	const [error, setError] = useState<string | null>(null);

	const fetchData = useCallback(async () => {
		setLoading(true)
		try {
			const debts = await getDebtPayments(debtPaymentsQueryParameters);
			setDebtPayments(debts);
		} catch (err: any) {
			setError(err.message || 'Ошибка загрузки данных')
		} finally {
			setLoading(false)
		}
	}, [debtPaymentsQueryParameters])

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
		deleteDebtPaymentEntity,
		debtPaymentsQueryParameters,
		setDebtPaymentsQueryParameters
	}
}
