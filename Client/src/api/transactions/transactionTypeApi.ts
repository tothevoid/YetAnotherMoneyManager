import config from "../../config";
import { TransactionTypeEntity } from "../../models/transactions/TransactionTypeEntity";
import { createEntity, deleteEntity, getAllEntities, updateEntity } from "../basicApi";

const basicUrl = `${config.api.URL}/TransactionType`;

export const getTransactionTypes = async (onlyActive: boolean = false): Promise<TransactionTypeEntity[]> =>  {
    const url = onlyActive ?
        `${basicUrl}?onlyActive=true`:
        basicUrl;

    return await getAllEntities<TransactionTypeEntity>(url)
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