import config from "../../config";
import { TransactionTypeEntity } from "../../models/transactions/TransactionTypeEntity";
import { createEntity, deleteEntity, getAllEntities, updateEntity } from "../basicApi";

const basicUrl = `${config.api.URL}/TransactionType`;

export const getTransactionTypes = async (): Promise<TransactionTypeEntity[]> =>  {
    return await getAllEntities<TransactionTypeEntity>(basicUrl)
}

export const createTransactionType = async (transactionType: TransactionTypeEntity): Promise<string | void> => {
    const result = await createEntity<TransactionTypeEntity>(basicUrl, transactionType);
    return result?.id;
}

export const updateTransactionType = async (modifiedTransactionType: TransactionTypeEntity): Promise<boolean> => {
    return await updateEntity<TransactionTypeEntity>(basicUrl, modifiedTransactionType);
}

export const deleteTransactionType = async (transactionTypeId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, transactionTypeId);
}