import { PaginationConfig } from '../../shared/models/PaginationConfig';
import { CurrencyAccountSummaryEntity } from '../../models/transactions/CurrencyAccountSummaryEntity';
import { CurrencyTransactionEntity, CurrencyTransactionEntityRequest, CurrencyTransactionEntityResponse } from '../../models/transactions/CurrencyTransactionEntity';
import { createEntity, deleteEntity, getAllEntities, updateEntity, getEntityById, getEntity } from '../basicApi';
import { prepareCurrencyTransaction, prepareCurrencyTransactionRequest } from './currencyTransactionApiMapping';

const basicUrl = `CurrencyTransaction`;

export const getCurrencyTransactions = async (): Promise<CurrencyTransactionEntity[]> => {
	return await getAllEntities<CurrencyTransactionEntityResponse>(basicUrl)
		.then((securityTransactions: CurrencyTransactionEntityResponse[]) => {
			return securityTransactions.map(prepareCurrencyTransaction)
		})
};

export const createCurrencyTransaction = async (addedSecurityTransaction: CurrencyTransactionEntity): Promise<void> => {
	await createEntity<CurrencyTransactionEntityRequest, CurrencyTransactionEntityResponse>(basicUrl, 
		prepareCurrencyTransactionRequest(addedSecurityTransaction));
}

export const updateCurrencyTransaction = async (modifiedSecurityTransaction: CurrencyTransactionEntity): Promise<boolean> => {
	return await updateEntity(basicUrl, prepareCurrencyTransactionRequest(modifiedSecurityTransaction));
}

export const deleteCurrencyTransaction = async (securityTransactionId: string): Promise<boolean> => {
	return await deleteEntity(basicUrl, securityTransactionId);
}

export const getCurrencyTransactionById = async (id: string): Promise<CurrencyTransactionEntity | null> => {
	const dto = await getEntityById<CurrencyTransactionEntityResponse>(basicUrl, id);
	if (!dto) return null;
	return prepareCurrencyTransaction(dto);
};

export const getCurrencyTransactionsByAccountId = async (
	accountId: string,
	pageIndex = 1,
	recordsQuantity = 10
): Promise<CurrencyTransactionEntity[]> => {
	const dtos = await getAllEntities<CurrencyTransactionEntityResponse>(
		`${basicUrl}/GetAllByAccountId?accountId=${accountId}&pageIndex=${pageIndex}&recordsQuantity=${recordsQuantity}`
	);
	return dtos.map(prepareCurrencyTransaction);
};

export const getCurrencyTransactionsPagination = async (accountId: string): Promise<PaginationConfig | void> => {
	return await getEntity<PaginationConfig>(`${basicUrl}/GetPaginationByAccountId?accountId=${accountId}`);
};

export const getCurrencyAccountSummary = async (accountId: string): Promise<CurrencyAccountSummaryEntity | null> => {
	const result = await getEntity<CurrencyAccountSummaryEntity>(`${basicUrl}/GetSummaryByAccountId?accountId=${accountId}`);
	return result ?? null;
};