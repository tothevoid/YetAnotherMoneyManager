import config from "../config";
import { CurrencyEntity } from "../models/currencies/CurrencyEntity";
import { createEntity, deleteEntity, getAllEntities, updateEntity } from "./basicApi";

const basicUrl = `${config.api.URL}/Currency`;

export const getCurrencies = async (): Promise<CurrencyEntity[]> =>  {
    return await getAllEntities<CurrencyEntity>(basicUrl)
}

export const createCurrency = async (newCurrency: CurrencyEntity): Promise<string | void> => {
    const result = await createEntity<CurrencyEntity>(basicUrl, newCurrency);
    return result?.id;
}

export const updateCurrency = async (modifiedCurrency: CurrencyEntity): Promise<boolean> => {
    return await updateEntity<CurrencyEntity>(basicUrl, modifiedCurrency);
}

export const deleteCurrency = async (currencyId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, currencyId);
}