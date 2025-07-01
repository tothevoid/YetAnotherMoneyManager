import { AccountEntity } from "../accounts/AccountEntity";

interface CommonCurrencyTransactionEntity {
	id: string,
	rate: number,
	amount: number
}

export interface ServerCurrencyTransactionEntity extends CommonCurrencyTransactionEntity {
	date: string,
	sourceAccountId: string,
	destinationAccountId: string
}

export interface ClientCurrencyTransactionEntity extends CommonCurrencyTransactionEntity {
	date: Date,
	sourceAccount: AccountEntity,
	destinationAccount: AccountEntity
}

