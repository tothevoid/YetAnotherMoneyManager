import config from '../../config' 
import { BankEntity } from '../../models/banks/BankEntity';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/Bank`;

export const getBanks = async (): Promise<BankEntity[]> => {
   return await getAllEntities<BankEntity>(basicUrl);
};

export const createBank = async (addedBank: BankEntity): Promise<BankEntity | void> => {
    return await createEntity<BankEntity, BankEntity>(basicUrl, addedBank);
}

export const updateBank = async (modifiedBank: BankEntity): Promise<boolean> => {
    return await updateEntity<BankEntity>(basicUrl, modifiedBank);
}

export const deleteBank = async (bankId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, bankId);
}