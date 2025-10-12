
import { useCallback, useEffect, useState } from "react";
import { createBrokerAccountFundsTransfer, deleteBrokerAccountFundsTransfer, getBrokerAccountFundsTransfers, updateBrokerAccountFundsTransfer } from "../../../api/brokers/BrokerAccountFundsTransferApi";
import { BrokerAccountFundTransferEntity } from "../../../models/brokers/BrokerAccountFundTransfer";

interface FundTransfersQuery {
    brokerAccountId: string
}

export const useBrokerAccountFundTransfers = (queryParameters: FundTransfersQuery) => {

    const [fundTransfers, setFundTransfers] = useState<BrokerAccountFundTransferEntity[]>([]);
    const [isFundTransfersLoading, setLoading] = useState(false);

    const [error, setError] = useState<string | null>(null);
    const [fundTransfersQueryParameters] = useState<FundTransfersQuery>(queryParameters);

    const fetchData = useCallback(async () => {
        setLoading(true)
        try {
            const transfers = await getBrokerAccountFundsTransfers(fundTransfersQueryParameters.brokerAccountId)
            setFundTransfers(transfers);
        } catch (err: any) {
            setError(err.message || 'Ошибка загрузки данных')
        } finally {
            setLoading(false)
        }
    }, [fundTransfersQueryParameters])

    useEffect(() => {
        fetchData();
    }, [fetchData])

    const createFundTransferEntity = async (createdFundTransfer: BrokerAccountFundTransferEntity) => {
        await createBrokerAccountFundsTransfer(createdFundTransfer);

        await fetchData();
    }

    const updateFundTransferEntity = async (updatedFundTransfer: BrokerAccountFundTransferEntity) => {
        await updateBrokerAccountFundsTransfer(updatedFundTransfer);
        
        await fetchData();
    }

    const deleteFundTransferEntity = async (deletedFundTransfer: BrokerAccountFundTransferEntity) => {
        const fundTransferDeleted = await deleteBrokerAccountFundsTransfer(deletedFundTransfer.id);
        if (!fundTransferDeleted) {
            return;
        }

        await fetchData();
    }

    return {
        fundTransfers,
        isFundTransfersLoading,
        error,
        createFundTransferEntity,
        updateFundTransferEntity,
        deleteFundTransferEntity,
        reloadFundTransfers: fetchData
    }
}
