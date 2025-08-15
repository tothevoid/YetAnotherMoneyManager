import config from '../../config' 
import { convertToDateOnly } from '../../shared/utilities/dateUtils';
import { checkPromiseStatus, logPromiseError } from '../../shared/utilities/webApiUtilities';
import { AccountToUpdate } from '../../models/accounts/accountToUpdate';
import { createEntity, deleteEntity, updateEntity } from '../basicApi';
import { TransactionEntity, TransactionEntityRequest, TransactionEntityResponse } from '../../models/transactions/TransactionEntity';

const basicUrl = `${config.api.URL}/Transaction`;

export const getTransactions = async (month: number, year: number, showSystem: boolean): Promise<TransactionEntity[]> => {
    const url = `${basicUrl}?month=${month}&year=${year}&showSystem=${showSystem}`;
    const transactions = await fetch(url, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then((transactionsResponses: TransactionEntityResponse[]) => transactionsResponses.map(prepareTransaction))
        .catch(logPromiseError);
  
    return transactions ?? [];
};

export const createTransaction = async (transaction: TransactionEntity): Promise<TransactionEntity | void> => {
    return await createEntity<TransactionEntityRequest, TransactionEntityResponse>(basicUrl, prepareTransactionRequest(transaction))
        .then(transaction => transaction && prepareTransaction(transaction));
}

export const updateTransaction = async (modifiedTransaction: TransactionEntity): Promise<AccountToUpdate[]> => {
    // TODO: Fix return type
    return await updateEntity(basicUrl, prepareTransactionRequest(modifiedTransaction));
}

export const deleteTransaction = async (transactionId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, transactionId);
}

const prepareTransaction = (transaction: TransactionEntityResponse): TransactionEntity => {
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

const prepareTransactionRequest = (transaction: TransactionEntity): TransactionEntityRequest => {
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