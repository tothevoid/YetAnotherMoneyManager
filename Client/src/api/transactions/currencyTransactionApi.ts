import config from '../../config' 
import { ClientCurrencyTransactionEntity, ServerCurrencyTransactionEntity } from '../../models/transactions/CurrencyTransactionEntity';
import { convertToDateOnly } from '../../shared/utilities/dateUtils';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/CurrencyTransaction`;

export const getCurrencyTransactions = async (): Promise<ClientCurrencyTransactionEntity[]> => {
	return await getAllEntities<ServerCurrencyTransactionEntity>(basicUrl)
		.then((securityTransactions: ServerCurrencyTransactionEntity[]) => {
			return securityTransactions.map(prepareClientCurrencyTransaction)
		})
};

export const createCurrencyTransaction = async (addedSecurityTransaction: ClientCurrencyTransactionEntity): Promise<ClientCurrencyTransactionEntity | void> => {
	return await createEntity<ServerCurrencyTransactionEntity>(basicUrl, prepareServerCurrencyTransaction(addedSecurityTransaction))
		.then(prepareClientCurrencyTransaction);
}

export const updateCurrencyTransaction = async (modifiedSecurityTransaction: ClientCurrencyTransactionEntity): Promise<boolean> => {
	return await updateEntity(basicUrl, prepareServerCurrencyTransaction(modifiedSecurityTransaction));
}

export const deleteCurrencyTransaction = async (securityTransactionId: string): Promise<boolean> => {
	return await deleteEntity(basicUrl, securityTransactionId);
}

const prepareClientCurrencyTransaction = (currencyTransaction: ServerCurrencyTransactionEntity): ClientCurrencyTransactionEntity => {
	return {
		...currencyTransaction,
		date: new Date(currencyTransaction.date)
	}
}

const prepareServerCurrencyTransaction = (currencyTransaction: ClientCurrencyTransactionEntity): ServerCurrencyTransactionEntity => {
	return {
		id: currencyTransaction.id,
		date: convertToDateOnly(currencyTransaction.date),
		amount: currencyTransaction.amount,
		rate: currencyTransaction.rate,
		destinationAccountId: currencyTransaction.destinationAccount.id,
		sourceAccountId: currencyTransaction.sourceAccount.id
	}
}
