import { useCallback, useEffect, useState } from "react";
import { DividendEntity } from "../../../models/securities/DividendEntity";
import { createDividend, deleteDividend, getDividends, updateDividend } from "../../../api/securities/dividendApi";

export interface DividendsQuery {
	securityId: string
}

export const useDividends = (query: DividendsQuery) => {
	const [dividends, setDividends] = useState<DividendEntity[]>([]);
	const [isDividendsLoading, setLoading] = useState(false);

	const [error, setError] = useState<string | null>(null);
	const [dividendsQueryParameters, setDividendsQueryParameters] = useState<DividendsQuery>(query);

	const fetchData = useCallback(async () => {
		setLoading(true)
		try {
			const dividends = await getDividends(dividendsQueryParameters.securityId);
			setDividends(dividends);
		} catch (err: any) {
			setError(err.message || 'Ошибка загрузки данных')
		} finally {
			setLoading(false)
		}
	}, [dividendsQueryParameters])

	useEffect(() => {
		fetchData();
	}, [fetchData])

	const createDividendEntity = async (createdDividend: DividendEntity) => {
		const addedDividend = await createDividend(createdDividend);
		if (!addedDividend) {
			return;
		}

		// TODO: add by order
		setDividends([addedDividend, ...dividends])
	}

	const updateDividendEntity = async (updatedDividend: DividendEntity) => {
		const updated = await updateDividend(updatedDividend);
		if (!updated) {
			return;
		}

		await fetchData();
	}

	const deleteDividendEntity = async (deletedDividend: DividendEntity) => {
		const deleted = await deleteDividend(deletedDividend.id);
		if (!deleted) {
			return;
		}

		await fetchData();
	}

	return {
		dividends,
		isDividendsLoading,
		error,
		createDividendEntity,
		updateDividendEntity,
		deleteDividendEntity,
		setDividendsQueryParameters,
		dividendsQueryParameters,
		reloadDividends: fetchData
	}
}
