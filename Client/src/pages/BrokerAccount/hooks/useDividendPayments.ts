import { useCallback, useEffect, useState } from "react";
import { createDividendPayment, deleteDividendPayment, getDividendPaymentsByBrokerAccount, updateDividendPayment } from "../../../api/brokers/dividendPaymentApi";
import { ClientDividendPaymentEntity } from "../../../models/brokers/DividendPaymentEntity";

export interface DividendPaymentsQuery {
	brokerAccountId: string
}

export const useDividendPayments = (queryParameters: DividendPaymentsQuery, onDataChanged: () => void) => {
	const [dividendPayments, setDividendPayments] = useState<ClientDividendPaymentEntity[]>([]);
	const [isSecurityTransactionsLoading, setLoading] = useState(false);

	const [error, setError] = useState<string | null>(null);
	const [dividendPaymentsQueryParameters, setDividendPaymentsQueryParameters] = useState<DividendPaymentsQuery>(queryParameters);

	const fetchData = useCallback(async () => {
		setLoading(true)
		try {
			const payments = await getDividendPaymentsByBrokerAccount(dividendPaymentsQueryParameters.brokerAccountId)
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

	const createDividendPaymentEntity = async (createdDividendPayment: ClientDividendPaymentEntity) => {
		await createDividendPayment(createdDividendPayment);

		await fetchData();
		onDataChanged();
	}

	const updateDividendPaymentEntity = async (updatedDividendPayment: ClientDividendPaymentEntity) => {
		await updateDividendPayment(updatedDividendPayment);
		
		await fetchData();
		onDataChanged();
	}

	const deleteDividendPaymentEntity = async (deletedDividendPayment: ClientDividendPaymentEntity) => {
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
		reloadDividendPayments: fetchData
	}
}
