import config from "../../config";
import { TransactionTypeEntity } from "../../models/transactions/TransactionTypeEntity";
import { checkPromiseStatus, logPromiseError } from "../../utils/PromiseUtils";
import { deleteEntity, getAllEntities } from "../basicApi";

const basicUrl = `${config.api.URL}/TransactionType`;

export const getTransactionTypes = async (onlyActive: boolean = false): Promise<TransactionTypeEntity[]> =>  {
    const url = onlyActive ?
        `${basicUrl}?onlyActive=true`:
        basicUrl;

    return await getAllEntities<TransactionTypeEntity>(url)
}

export const createTransactionType = async (addedSecurity: TransactionTypeEntity, file: File | null): Promise<TransactionTypeEntity | void> => {
    return await fetch(basicUrl, { method: "PUT", body: generateForm(addedSecurity, file)})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then(id => {
            return {...addedSecurity, id} as T;
        })
        .catch(logPromiseError);
}

export const updateTransactionType = async (modifiedSecurity: TransactionTypeEntity, file: File | null): Promise<TransactionTypeEntity | void> => {
    return await fetch(basicUrl, { method: "PATCH", body: generateForm(modifiedSecurity, file)})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError)
}

export const deleteTransactionType = async (transactionTypeId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, transactionTypeId);
}

export const getTransactionTypeIconUrl = (iconKey: string | null): string => {
    if (!iconKey) {
        return "";
    }

    return `${basicUrl}/icon?iconKey=${iconKey}`;
}

const generateForm = (transactionType: TransactionTypeEntity, file: File | null) => {
    const formData = new FormData();
    formData.append("transactionTypeJson", JSON.stringify(transactionType));
    if (file) {
        formData.append("transactionTypeIcon", file);
    }
    return formData;
}