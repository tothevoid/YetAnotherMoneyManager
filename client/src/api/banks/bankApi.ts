import config from '../../config' 
import { BankEntity } from '../../models/banks/BankEntity';
import { Nullable } from '../../shared/utilities/nullable';
import { checkPromiseStatus, logPromiseError } from '../../shared/utilities/webApiUtilities';
import { deleteEntity, getAllEntities } from '../basicApi';

const basicUrl = `${config.api.URL}/Bank`;

export const getBanks = async (): Promise<BankEntity[]> => {
   return await getAllEntities<BankEntity>(basicUrl);
};

export const createBank = async (addedBank: BankEntity, file: Nullable<File>): Promise<BankEntity | void> => {
    return await fetch(basicUrl, { method: "PUT", body: generateForm(addedBank, file)})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then((bank: BankEntity) => bank)
        .catch(logPromiseError);
}

export const updateBank = async (modifiedBank: BankEntity, file: Nullable<File>): Promise<BankEntity | void> => {
    return await fetch(basicUrl, { method: "PATCH", body: generateForm(modifiedBank, file)})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then((bank: BankEntity) => bank)
        .catch(logPromiseError)
}

export const deleteBank = async (bankId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, bankId);
}

export const getBankIconUrl = (iconKey: Nullable<string>): string => {
    if (!iconKey) {
        return "";
    }

    return `${basicUrl}/icon?iconKey=${iconKey}`;
}

const generateForm = (transactionType: BankEntity, file: Nullable<File>) => {
    const formData = new FormData();
    formData.append("bankJson", JSON.stringify(transactionType));
    if (file) {
        formData.append("bankIcon", file);
    }
    return formData;
}