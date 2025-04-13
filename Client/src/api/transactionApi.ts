import config from '../config' 
import { TransactionEntity } from '../models/transactions/TransactionEntity';
import { convertToDateOnly } from '../utils/DateUtils';
import { checkPromiseStatus, logPromiseError } from '../utils/PromiseUtils';
import { AccountToUpdate } from './models/accountToUpdate';

const basicUrl = `${config.api.URL}/Transaction`;

export const getTransactions = async (month: number, year: number): Promise<TransactionEntity[]> => {
    const url = `${basicUrl}?month=${month}&year=${year}`;
    const transactions = await fetch(url, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then((transactions: TransactionEntity[]) => 
            transactions.map((transaction: TransactionEntity) => {
                const date = new Date(transaction.date);
                return {...transaction, date: date} as TransactionEntity;
            })
        )
        .catch(logPromiseError);
  
    return transactions ?
        transactions: 
        [] as TransactionEntity[];
};

export const createTransaction = async (transaction: TransactionEntity): Promise<TransactionEntity | void> => {
    const newTransaction = await fetch(basicUrl, { method: "PUT", body: prepareTransactionEntity(transaction),
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then(id => {
            return {...transaction, id} as TransactionEntity;
        })
        .catch(logPromiseError);
    return newTransaction;
}

export const updateTransaction = async (modifiedTransaction: TransactionEntity): Promise<AccountToUpdate[]> => {
    const result: AccountToUpdate[] | void = await fetch(basicUrl, { method: "PATCH", body: prepareTransactionEntity(modifiedTransaction),  
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .then(response => response.json())
        .catch(logPromiseError);

    return result ? result: [] as AccountToUpdate[];
}

export const deleteTransaction = async (transactionId: string): Promise<boolean> => {
    if (!transactionId) {
        return false;
    }

    const url = `${basicUrl}?id=${transactionId}`;
    const result = await fetch(url, { method: "DELETE"})
        .then(checkPromiseStatus)
        .catch(logPromiseError);

    return result?.ok ?? false;
}

const prepareTransactionEntity = (transaction: TransactionEntity): string => {
    const serverTransaction = {...transaction};

    // .NET DateOnly cast
    serverTransaction.date = convertToDateOnly(serverTransaction.date );

    return JSON.stringify(serverTransaction);
}