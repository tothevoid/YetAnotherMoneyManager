import config from "../config";
import { FundEntity } from "../models/FundEntity";
import { checkPromiseStatus, logPromiseError } from "../utils/PromiseUtils";

export const getFunds = async (): Promise<FundEntity[]> =>  {
    const url = `${config.api.URL}/Fund`;
    const funds = await fetch(url, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);

    return funds ? funds : [] as FundEntity[];
}

export const createFund = async (newFund: FundEntity): Promise<string | void> => {
    const url = `${config.api.URL}/Fund`;

    const {id, ...fieldsExceptId} = newFund;

    const addedFundId: string | void = await fetch(url, { method: "PUT", body: JSON.stringify(fieldsExceptId), 
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())

    return addedFundId;
}

export const updateFund = async (modifiedFund: FundEntity): Promise<boolean> => {
    const url = `${config.api.URL}/Fund`;
    const result = await fetch(url, { method: "PATCH", body: JSON.stringify(modifiedFund),  
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .catch(logPromiseError)

    return result?.ok ?? false;
}

export const deleteFund = async (deletingFund: FundEntity): Promise<boolean> => {
    const {id} = deletingFund;
    const url = `${config.api.URL}/Fund?id=${id}`;
    const result = await fetch(url, { method: "DELETE"})
        .then(checkPromiseStatus)
        .catch(logPromiseError)

    return result?.ok ?? false;
}