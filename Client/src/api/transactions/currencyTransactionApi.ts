import config from '../../config' 
import { CurrencyTransactionEntity, CurrencyTransactionEntityRequest, CurrencyTransactionEntityResponse } from '../../models/transactions/CurrencyTransactionEntity';
import { convertToDateOnly } from '../../shared/utilities/dateUtils';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from '../basicApi';

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

const prepareCurrencyTransaction = (currencyTransaction: CurrencyTransactionEntityResponse): CurrencyTransactionEntity => {
	return {
		id: currencyTransaction.id,
		amount: currencyTransaction.amount,
		rate: currencyTransaction.rate,
		destinationAccount: currencyTransaction.destinationAccount,
		sourceAccount: currencyTransaction.sourceAccount,
		date: new Date(currencyTransaction.date)
	}
}

const prepareCurrencyTransactionRequest = (currencyTransaction: CurrencyTransactionEntity): CurrencyTransactionEntityRequest => {
	return {
		id: currencyTransaction.id,
		amount: currencyTransaction.amount,
		rate: currencyTransaction.rate,
		destinationAccountId: currencyTransaction.destinationAccount.id,
		sourceAccountId: currencyTransaction.sourceAccount.id,
		date: convertToDateOnly(currencyTransaction.date)
	}
}
