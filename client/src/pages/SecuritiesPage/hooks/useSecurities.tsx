import { useCallback, useEffect, useState } from "react";
import { SecurityEntity } from "../../../models/securities/SecurityEntity";
import { createSecurity, deleteSecurity, getSecurities, updateSecurity } from "../../../api/securities/securityApi";

export const useSecurities = () => {
	const [securities, setSecurities] = useState<SecurityEntity[]>([]);
	const [isSecuritiesLoading, setLoading] = useState(false);

	const [error, setError] = useState<string | null>(null);

	const fetchData = useCallback(async () => {
		setLoading(true)
		try {
			const accounts = await getSecurities();
			setSecurities(accounts);
		} catch (err: any) {
			setError(err.message || 'Ошибка загрузки данных')
		} finally {
			setLoading(false)
		}
	}, [])

	useEffect(() => {
		fetchData();
	}, [fetchData])

	const createSecurityEntity = async (createdSecurity: SecurityEntity, icon: File | null) => {
		const addedSecurity = await createSecurity(createdSecurity, icon);
		if (!addedSecurity) {
			return;
		}

		setSecurities([addedSecurity, ...securities]);
	}

	const updateSecurityEntity = async (updatedSecurity: SecurityEntity, icon: File | null) => {
		const isAccountUpdated = await updateSecurity(updatedSecurity, icon);
		if (!isAccountUpdated) {
			return;
		}
	
		await fetchData();
	}

	const deleteSecurityEntity = async (deletedSecurity: SecurityEntity) => {
		const isAccountDeleted = await deleteSecurity(deletedSecurity.id);
		if (!isAccountDeleted) {
			return;
		}

		await fetchData();
	}

	return {
		securities,
		isSecuritiesLoading,
		error,
		createSecurityEntity,
		updateSecurityEntity,
		deleteSecurityEntity,
		reloadSecurities: fetchData
	}
}
