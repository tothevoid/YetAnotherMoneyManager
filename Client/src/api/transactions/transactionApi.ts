import config from '../../config' 
import { ServerTransactionEntity, TransactionEntity } from '../../models/transactions/TransactionEntity';
import { convertToDateOnly } from '../../utils/DateUtils';
import { checkPromiseStatus, logPromiseError } from '../../utils/PromiseUtils';
import { AccountToUpdate } from '../../models/accounts/accountToUpdate';
import { createEntity, deleteEntity, updateEntity } from '../basicApi';

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
    return await createEntity(basicUrl, prepareServerTransaction(transaction));
}

export const updateTransaction = async (modifiedTransaction: TransactionEntity): Promise<AccountToUpdate[]> => {
    return await updateEntity(basicUrl, prepareServerTransaction(modifiedTransaction));
}

export const deleteTransaction = async (transactionId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, transactionId);
}

const prepareServerTransaction = (transaction: TransactionEntity): ServerTransactionEntity => {
    return {
        id: transaction.id,
        name: transaction.name,
        date: convertToDateOnly(transaction.date),
        isSystem: transaction.isSystem,
        cashback: transaction.cashback,
        moneyQuantity: transaction.moneyQuantity,
        transactionType: transaction.transactionType,
        accountId: transaction.account.id
    }
}