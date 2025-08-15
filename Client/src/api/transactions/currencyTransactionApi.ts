import config from '../../config' 
import { CurrencyTransactionEntity, CurrencyTransactionEntityRequest, CurrencyTransactionEntityResponse } from '../../models/transactions/CurrencyTransactionEntity';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from '../basicApi';
import { prepareCurrencyTransaction, prepareCurrencyTransactionRequest } from './currencyTransactionApiMapping';

const basicUrl = `${config.api.URL}/CurrencyTransaction`;

export const getCurrencyTransactions = async (): Promise<CurrencyTransactionEntity[]> => {
	return await getAllEntities<CurrencyTransactionEntityResponse>(basicUrl)
		.then((securityTransactions: CurrencyTransactionEntityResponse[]) => {
			return securityTransactions.map(prepareCurrencyTransaction)
		})
};

export const createCurrencyTransaction = async (addedSecurityTransaction: CurrencyTransactionEntity): Promise<CurrencyTransactionEntity | void> => {
	return await createEntity<CurrencyTransactionEntityRequest, CurrencyTransactionEntityResponse>(basicUrl, 
		prepareCurrencyTransactionRequest(addedSecurityTransaction))
			.then((currencyTransaction) => currencyTransaction && prepareCurrencyTransaction(currencyTransaction));
}

export const updateCurrencyTransaction = async (modifiedSecurityTransaction: CurrencyTransactionEntity): Promise<boolean> => {
	return await updateEntity(basicUrl, prepareCurrencyTransactionRequest(modifiedSecurityTransaction));
}

export const deleteCurrencyTransaction = async (securityTransactionId: string): Promise<boolean> => {
	return await deleteEntity(basicUrl, securityTransactionId);
}