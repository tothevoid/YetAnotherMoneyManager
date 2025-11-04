import { useCallback, useEffect, useState } from "react";
import { createDividendPayment, deleteDividendPayment, getDividendPaymentsByBrokerAccount, updateDividendPayment } from "../../../api/brokers/dividendPaymentApi";
import { DividendPaymentEntity } from "../../../models/brokers/DividendPaymentEntity";

export interface DividendPaymentsQuery {
	currentPage: number,
	pageSize: number,
	brokerAccountId: string
}

export const useDividendPayments = (queryParameters: DividendPaymentsQuery, onDataChanged: () => void) => {
	const [dividendPayments, setDividendPayments] = useState<DividendPaymentEntity[]>([]);
	const [isSecurityTransactionsLoading, setLoading] = useState(false);

	const [error, setError] = useState<string | null>(null);
	const [dividendPaymentsQueryParameters, setDividendPaymentsQueryParameters] = useState<DividendPaymentsQuery>(queryParameters);

	const fetchData = useCallback(async () => {
		setLoading(true)
		try {
			const payments = await getDividendPaymentsByBrokerAccount(dividendPaymentsQueryParameters)
			setDividendPayments(payments);
		} catch (err: any) {
			setError(err.message || 'Ошибка загрузки данных')
		} finally {
			setLoading(false)
		}
	}, [dividendPaymentsQueryParameters])

	useEffect(() => {
		fetchData();
	}, [fetchData])

	const createDividendPaymentEntity = async (createdDividendPayment: DividendPaymentEntity) => {
		await createDividendPayment(createdDividendPayment);

		await fetchData();
		onDataChanged();
	}

	const updateDividendPaymentEntity = async (updatedDividendPayment: DividendPaymentEntity) => {
		await updateDividendPayment(updatedDividendPayment);
		
		await fetchData();
		onDataChanged();
	}

	const deleteDividendPaymentEntity = async (deletedDividendPayment: DividendPaymentEntity) => {
		const securityTransactionDeleted = await deleteDividendPayment(deletedDividendPayment.id);
		if (!securityTransactionDeleted) {
			return;
		}

		await fetchData();
		onDataChanged();
	}

	return {
		dividendPayments,
		isSecurityTransactionsLoading,
		error,
		createDividendPaymentEntity,
		updateDividendPaymentEntity,
		deleteDividendPaymentEntity,
		reloadDividendPayments: fetchData,
		dividendPaymentsQueryParameters,
		setDividendPaymentsQueryParameters
	}
}
