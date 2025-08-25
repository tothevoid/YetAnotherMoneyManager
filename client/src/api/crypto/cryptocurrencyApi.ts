import config from '../../config' 
import { CryptocurrencyEntity } from '../../models/crypto/CryptocurrencyEntity';
import { Nullable } from '../../shared/utilities/nullable';
import { checkPromiseStatus, logPromiseError } from '../../shared/utilities/webApiUtilities';
import { deleteEntity, getAllEntities } from '../basicApi';

const basicUrl = `${config.api.URL}/Cryptocurrency`;

export const getCryptocurrencies = async (): Promise<CryptocurrencyEntity[]> => {
   return await getAllEntities<CryptocurrencyEntity>(basicUrl);
};

export const createCryptocurrency = async (addedCryptocurrency: CryptocurrencyEntity, file: File | null): Promise<CryptocurrencyEntity | void> => {
    return await fetch(basicUrl, { method: "PUT", body: generateForm(addedCryptocurrency, file)})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then(id => {
            return {...addedCryptocurrency, id} as CryptocurrencyEntity;
        })
        .catch(logPromiseError);
}

export const updateCryptocurrency = async (modifiedCryptocurrency: CryptocurrencyEntity, file: File | null): Promise<boolean> => {
    const cryptoCurrencyResponse = await fetch(basicUrl, { method: "PATCH", body: generateForm(modifiedCryptocurrency, file)})
        .then(checkPromiseStatus)
        .catch(logPromiseError)

    return cryptoCurrencyResponse?.ok ?? false;
}

export const deleteCryptocurrency = async (cryptoCurrencyId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, cryptoCurrencyId);
}

const generateForm = (cryptocurrency: CryptocurrencyEntity, file: File | null) => {
    const formData = new FormData();
    formData.append("cryptocurrencyJson", JSON.stringify(cryptocurrency));
    if (file) {
        formData.append("cryptoCurrencyIcon", file);
    }
    return formData;
}

export const getIconUrl = (iconKey: Nullable<string>, date: Nullable<Date> = null): string => {
    if (!iconKey) {
        return "";
    }

    const baseIconUrl = `${basicUrl}/icon?iconKey=${iconKey}`;

    return date ?
        `${baseIconUrl}&date=${date}`:
        baseIconUrl;
}