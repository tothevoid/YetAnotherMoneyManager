import { TransactionEntityResponse, TransactionEntity, TransactionEntityRequest } from "../../models/transactions/TransactionEntity"
import { convertToDateOnly } from "../../shared/utilities/dateUtils"

export const prepareTransaction = (transaction: TransactionEntityResponse): TransactionEntity => {
    return {
        id: transaction.id,
        name: transaction.name,
        date: new Date(transaction.date),
        isSystem: transaction.isSystem,
        cashback: transaction.cashback,
        amount: transaction.amount,
        transactionType: transaction.transactionType,
        account: transaction.account
    }
}

export const prepareTransactionRequest = (transaction: TransactionEntity): TransactionEntityRequest => {
    return {
        id: transaction.id,
        name: transaction.name,
        date: convertToDateOnly(transaction.date),
        isSystem: transaction.isSystem,
        cashback: transaction.cashback,
        amount: transaction.amount,
        transactionTypeId: transaction.transactionType.id,
        accountId: transaction.account.id
    }
}