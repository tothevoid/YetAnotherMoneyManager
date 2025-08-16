import { useCallback, useEffect, useState } from "react";
import { BrokerAccountSecurityEntity } from "../../../models/brokers/BrokerAccountSecurityEntity";
import { deleteBrokerAccountSecurity, getSecuritiesByBrokerAccount, updateBrokerAccountSecurity } from "../../../api/brokers/brokerAccountSecurityApi";

export interface BrokerAccountSecuritiesQuery {
	brokerAccountId: string
}

export const useBrokerAccountsSecurities = (queryParameters: BrokerAccountSecuritiesQuery) => {
	const [brokerAccountSecurities, setBrokerAccountSecurities] = useState<BrokerAccountSecurityEntity[]>([]);
	const [isBrokerAccountSecuritiesLoading, setLoading] = useState(false);

	const [error, setError] = useState<string | null>(null);
	const [brokerAccountSecurityQueryParameters, setBrokerAccountSecurityQueryParameters] = useState<BrokerAccountSecuritiesQuery>(queryParameters);

	const fetchData = useCallback(async () => {
		setLoading(true)
		try {
			const securities = await getSecuritiesByBrokerAccount(brokerAccountSecurityQueryParameters.brokerAccountId);
			setBrokerAccountSecurities(securities);
		} catch (err: any) {
			setError(err.message || 'Ошибка загрузки данных')
		} finally {
			setLoading(false)
		}
	}, [brokerAccountSecurityQueryParameters])

	useEffect(() => {
		fetchData();
	}, [fetchData])

	const updateBrokerAccountSecurityEntity = async (updatedBrokerAccountSecurity: BrokerAccountSecurityEntity) => {
		const brokerAccountSecurityUpdated = await updateBrokerAccountSecurity(updatedBrokerAccountSecurity);

		if (!brokerAccountSecurityUpdated) {
			return
		}

		await fetchData();
	}

	const deleteBrokerAccountSecurityEntity = async (deletedBrokerAccountSecurity: BrokerAccountSecurityEntity) => {
		const brokerAccountSecurityDeleted = await deleteBrokerAccountSecurity(deletedBrokerAccountSecurity.id);
		
		if (!brokerAccountSecurityDeleted) {
			return;
		}

	   await fetchData();
	}
	
	return {
		brokerAccountSecurities,
		isBrokerAccountSecuritiesLoading,
		error,
		updateBrokerAccountSecurityEntity,
		deleteBrokerAccountSecurityEntity,
		setBrokerAccountSecurityQueryParameters,
		brokerAccountSecurityQueryParameters,
		reloadBrokerAccountSecurities: fetchData
	}
}
