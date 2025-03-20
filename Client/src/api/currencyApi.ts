import config from "../config";
import { CurrencyEntity } from "../models/CurrencyEntity";
import { checkPromiseStatus, logPromiseError } from "../utils/PromiseUtils";

const basicUrl = `${config.api.URL}/Currency`;

export const getCurrencies = async (): Promise<CurrencyEntity[]> =>  {
    const currencies = await fetch(basicUrl, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);

    return currencies ? currencies : [] as CurrencyEntity[];
}

export const createCurrency = async (newCurrency: CurrencyEntity): Promise<string | void> => {
    const {id, ...fieldsExceptId} = newCurrency;

    const addedCurrencyId: string | void = await fetch(basicUrl, { method: "PUT", body: JSON.stringify(fieldsExceptId), 
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())

    return addedCurrencyId;
}

export const updateCurrency = async (modifiedCurrency: CurrencyEntity): Promise<boolean> => {
    const result = await fetch(basicUrl, { method: "PATCH", body: JSON.stringify(modifiedCurrency),  
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .catch(logPromiseError)

    return result?.ok ?? false;
}

export const deleteCurrency = async (currencyId: string): Promise<boolean> => {
    if (!currencyId) {
        return false;
    }

    const url = `${basicUrl}?id=${currencyId}`;
    const result = await fetch(url, { method: "DELETE"})
        .then(checkPromiseStatus)
        .catch(logPromiseError)

    return result?.ok ?? false;
}