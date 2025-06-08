import { useCallback, useEffect, useState } from "react";
import { DepositEntity } from "../../../models/deposits/DepositEntity";
import { createDeposit, getDeposits } from "../../../api/deposits/depositApi";

export interface DepositsQuery {
	selectedMinMonths: number,
	selectedMaxMonths: number,
	onlyActive: boolean
}

export const useDeposits = (queryParameters: DepositsQuery) => {
	const [deposits, setDeposits] = useState<DepositEntity[]>([]);
	const [isDepositsLoading, setLoading] = useState(false);

	const [error, setError] = useState<string | null>(null);
	const [depositsQueryParameters, setDepositsQueryParameters] = useState<DepositsQuery>(queryParameters);

	const fetchData = useCallback(async () => {
		setLoading(true)
		try {
			const { selectedMinMonths, selectedMaxMonths, onlyActive } = depositsQueryParameters;

			if (!selectedMinMonths || !selectedMaxMonths) {
				setDeposits([]);
				return;
			}

			const accounts = await getDeposits(selectedMinMonths, selectedMaxMonths, onlyActive);
			
			setDeposits(accounts);
		} catch (err: any) {
			setError(err.message || 'Ошибка загрузки данных')
		} finally {
			setLoading(false)
		}
	}, [depositsQueryParameters])

	useEffect(() => {
		fetchData();
	}, [fetchData])

	const createDepositEntity = async (createdDeposit: DepositEntity) => {
		const addedDeposit = await createDeposit(createdDeposit);
		if (!addedDeposit) {
			return;
		}

		setDeposits([addedDeposit, ...deposits]);
	}

	const updateDepositEntity = async (updatedDeposit: DepositEntity) => {
		const updatedDeposits = deposits.map(deposit => 
			deposit.id === updatedDeposit.id ?
				{...updatedDeposit}:
				deposit
		);

		setDeposits(updatedDeposits);
	}

	const deleteDepositEntity = async (deletedDeposit: DepositEntity) => {
		const updatedDeposits = deposits.filter(deposit => deposit.id !== deletedDeposit.id);
		
		setDeposits(updatedDeposits);
	}

	return {
		deposits,
		isDepositsLoading,
		error,
		createDepositEntity,
		updateDepositEntity,
		deleteDepositEntity,
		depositsQueryParameters, 
		setDepositsQueryParameters
	}
}
