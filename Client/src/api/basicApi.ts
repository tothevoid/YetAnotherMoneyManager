import { checkPromiseStatus, logPromiseError } from "../utils/PromiseUtils";

export const getAllEntities = async <T> (basicUrl: string): Promise<T[]> => {
    const entities = await fetch(basicUrl, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
  
    return entities ?
        entities: 
        [] as T[];
};

export const createEntity = async<T> (basicUrl: string, addedEntity: T): Promise<T | void> => {
    const newEntity = await fetch(basicUrl, { method: "PUT", body: convertRecordToJson(addedEntity),
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then(id => {
            return {...addedEntity, id} as T;
        })
        .catch(logPromiseError);
    return newEntity;
}

export const updateEntity = async<T> (basicUrl: string, modifiedEntity: T): Promise<boolean> => {
    const updatedEntity = await fetch(basicUrl, { method: "PATCH", body: convertRecordToJson(modifiedEntity),  
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .catch(logPromiseError)

    return updatedEntity?.ok ?? false;
}

export const deleteEntity = async (basicUrl: string, recordId: string): Promise<boolean> => {
    if (!recordId) {
        return false;
    }

    const url = `${basicUrl}?id=${recordId}`;
    const result = await fetch(url, { method: "DELETE"})
        .then(checkPromiseStatus)
        .catch(logPromiseError);

    return result?.ok ?? false;
}

const convertRecordToJson = <T>(record: T): string => {
    return JSON.stringify(record);
}