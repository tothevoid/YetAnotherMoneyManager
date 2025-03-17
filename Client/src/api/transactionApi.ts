import config from '../config' 
import { TransactionEntity } from '../models/TransactionEntity';
import { checkPromiseStatus, logPromiseError } from '../utils/PromiseUtils';
import { AccountToUpdate } from './models/accountToUpdate';

export const getTransactions = async (month: number, year: number): Promise<TransactionEntity[]> => {
    const url = `${config.api.URL}/Transaction?month=${month}&year=${year}`;
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
    const url = `${config.api.URL}/Transaction`;
    const newTransaction = await fetch(url, { method: "PUT", body: JSON.stringify(transaction),
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
    const url = `${config.api.URL}/Transaction`;
    const result: AccountToUpdate[] | void = await fetch(url, { method: "PATCH", body: JSON.stringify(modifiedTransaction),  
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .then(response => response.json())
        .catch(logPromiseError);

    return result ? result: [] as AccountToUpdate[];
}

export const deleteTransaction = async (deletedTransaction: TransactionEntity): Promise<boolean> => {
    const url = `${config.api.URL}/Transaction`;
    const result = await fetch(url, { method: "DELETE", body: JSON.stringify(deletedTransaction), 
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .catch(logPromiseError);

    return result?.ok ?? false;
}