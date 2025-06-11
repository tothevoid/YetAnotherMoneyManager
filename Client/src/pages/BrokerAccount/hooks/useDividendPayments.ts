import { useCallback, useEffect, useState } from "react";
import { createDividendPayment, deleteDividendPayment, getDividendPaymentsByBrokerAccount, updateDividendPayment } from "../../../api/brokers/dividendPaymentApi";
import { ClientDividendPaymentEntity } from "../../../models/brokers/DividendPaymentEntity";

export interface DividendPaymentsQuery {
    brokerAccountId: string
}

export const useDividendPayments = (queryParameters: DividendPaymentsQuery) => {
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
        const securityTransaction = await createDividendPayment(createdDividendPayment);
        if (!securityTransaction) {
            return;
        }

        await fetchData();
    }

    const updateDividendPaymentEntity = async (updatedDividendPayment: ClientDividendPaymentEntity) => {
        const securityTransactionUpdated = await updateDividendPayment(updatedDividendPayment);
        if (!securityTransactionUpdated) {
            return;
        }

        await fetchData();
    }

    const deleteDividendPaymentEntity = async (deletedDividendPayment: ClientDividendPaymentEntity) => {
        const securityTransactionDeleted = await deleteDividendPayment(deletedDividendPayment.id);
        if (!securityTransactionDeleted) {
            return;
        }

        await fetchData();
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
