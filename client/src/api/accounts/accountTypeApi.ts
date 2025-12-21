import { AccountTypeEntity } from '../../models/accounts/AccountTypeEntity';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from '../basicApi';

const basicUrl = `AccountType`;

export const getAccountTypes = async (): Promise<AccountTypeEntity[]> => {
    return await getAllEntities(basicUrl)
};

export const createAccountType = async (accountType: AccountTypeEntity): Promise<AccountTypeEntity | void> => {
    return await createEntity(basicUrl, accountType);
}

export const updateAccountType = async (accountType: AccountTypeEntity): Promise<boolean> => {
    return await updateEntity(basicUrl, accountType);
}

export const deleteAccountType = async (accountTypeId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, accountTypeId);
}