import { CurrencyTransactionEntityResponse, CurrencyTransactionEntity, CurrencyTransactionEntityRequest } from "../../models/transactions/CurrencyTransactionEntity"
import { convertToDateOnly } from "../../shared/utilities/dateUtils"

export const prepareCurrencyTransaction = (currencyTransaction: CurrencyTransactionEntityResponse): CurrencyTransactionEntity => {
	return {
		id: currencyTransaction.id,
		amount: currencyTransaction.amount,
		rate: currencyTransaction.rate,
		destinationAccount: currencyTransaction.destinationAccount,
		sourceAccount: currencyTransaction.sourceAccount,
		date: new Date(currencyTransaction.date)
	}
}

export const prepareCurrencyTransactionRequest = (currencyTransaction: CurrencyTransactionEntity): CurrencyTransactionEntityRequest => {
	return {
		id: currencyTransaction.id,
		amount: currencyTransaction.amount,
		rate: currencyTransaction.rate,
		destinationAccountId: currencyTransaction.destinationAccount.id,
		sourceAccountId: currencyTransaction.sourceAccount.id,
		date: convertToDateOnly(currencyTransaction.date)
	}
}
