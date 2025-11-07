import { checkPromiseStatus, logPromiseError } from "../shared/utilities/webApiUtilities";

export const getAllEntities = async <T> (basicUrl: string): Promise<T[]> => {
    const entities = await fetch(basicUrl, {method: "GET",  headers: {...getAuthHeader()}})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
  
    return entities ?
        entities: 
        [] as T[];
};

export const getAllEntitiesByConfig = async <TInput, TOutput> (basicUrl: string, data: TInput): Promise<TOutput[]> => {
    const entities = await fetch(basicUrl, {method: "POST", body: convertRecordToJson(data),
            headers: {"Content-Type": "application/json", ...getAuthHeader()}
        })
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
  
    return entities ?
        entities: 
        [] as TOutput[];
};

export const createEntity = async<TRequest, TResponse> (basicUrl: string, addedEntity: TRequest): Promise<TResponse | void> => {
    return sendCreateRequest(basicUrl, addedEntity, (id) => {return {...addedEntity, id} as TResponse})
}

export const createAndGetFullEntity = async<TRequest, TResponse> (basicUrl: string, addedEntity: TRequest): Promise<TResponse | void> => {
    return sendCreateRequest(basicUrl, addedEntity, (createdEntity) => {return {...createdEntity} as TResponse})
}

export const createEntityWithIcon = async<TRequest, TResponse>(basicUrl: string, addedEntity: TRequest, 
        entityFieldName: string, iconFieldName: string, file: File | null): Promise<TResponse | void> => {
    return await fetch(basicUrl, { method: "PUT",  headers: {...getAuthHeader()}, body: generateForm(addedEntity, entityFieldName, iconFieldName, file)})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then(id => {
            return {...addedEntity, id} as TResponse;
        })
        .catch(logPromiseError);
}

export const updateEntity = async<TRequest> (basicUrl: string, modifiedEntity: TRequest): Promise<boolean> => {
    const updatedEntity = await fetch(basicUrl, { method: "PATCH", body: convertRecordToJson(modifiedEntity),  
        headers: {"Content-Type": "application/json", ...getAuthHeader()}})
        .then(checkPromiseStatus)
        .catch(logPromiseError)

    return updatedEntity?.ok ?? false;
}

export const updateEntityWithIcon = async<TRequest> (basicUrl: string, modifiedEntity: TRequest, 
    entityFieldName: string, iconFieldName: string, file: File | null): Promise<boolean> => {
    const securityResponse = await fetch(basicUrl, { method: "PATCH", headers: {...getAuthHeader()}, body: generateForm(modifiedEntity, entityFieldName, iconFieldName, file )})
        .then(checkPromiseStatus)
        .catch(logPromiseError)

    return securityResponse?.ok ?? false;
}

export const deleteEntity = async (basicUrl: string, recordId: string): Promise<boolean> => {
    if (!recordId) {
        return false;
    }

    const url = `${basicUrl}?id=${recordId}`;
    const result = await fetch(url, { method: "DELETE",  headers: {...getAuthHeader()}})
        .then(checkPromiseStatus)
        .catch(logPromiseError);

    return result?.ok ?? false;
}

export const getEntityById = async <T> (basicUrl: string, id: string): Promise<T | void> => {
    const entity: T | void = await fetch(`${basicUrl}/GetById?id=${id}`, { method: "GET", headers: getAuthHeader() })
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
    return entity;
}

export const convertRecordToJson = <T>(record: T): string => {
    return JSON.stringify(record);
}

const generateForm = <T>(entity: T, entityField: string, iconField: string, file: File | null) => {
    if (entityField === iconField) {
        throw new Error(`Entity field (${entityField}) same as icon field (${iconField})`)
    }

    const formData = new FormData();
    formData.append(entityField, JSON.stringify(entity));
    if (file) {
        formData.append(iconField, file);
    }
    return formData;
    }

export const sendCreateRequest = async<TRequest, TResponse> (basicUrl: string, addedEntity: TRequest, 
    responseHandler: (response: TResponse) => TResponse): Promise<TResponse | void> => {
    const newEntity = await fetch(basicUrl, { method: "PUT", body: convertRecordToJson(addedEntity),
        headers: {"Content-Type": "application/json", ...getAuthHeader()}})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then(responseHandler)
        .catch(logPromiseError);
    return newEntity;
}

export const getAuthHeader = (): HeadersInit => {
    const token = localStorage.getItem("auth_token");
    return {
        "Authorization": `Bearer ${token}`
    };
}