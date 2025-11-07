import { PaginationConfig } from "../shared/models/PaginationConfig";
import { Nullable } from "../shared/utilities/nullable";
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
        entityFieldName: string, iconFieldName: string, file: Nullable<File>): Promise<TResponse | void> => {
    return await fetch(basicUrl, { method: "PUT",  headers: {...getAuthHeader()}, body: generateForm(addedEntity, entityFieldName, iconFieldName, file)})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then(responseEntity => {
            return {...responseEntity} as TResponse;
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

export const updateEntityWithIcon = async<TRequest, TResponse> (basicUrl: string, modifiedEntity: TRequest, 
    entityFieldName: string, iconFieldName: string, file: Nullable<File>): Promise<TResponse | void> => {
    return await fetch(basicUrl, { method: "PATCH", headers: {...getAuthHeader()}, 
        body: generateForm(modifiedEntity, entityFieldName, iconFieldName, file )})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then(responseEntity => {
            return {...responseEntity} as TResponse;
        })
        .catch(logPromiseError);
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

export const getEntity = async <T> (basicUrl: string): Promise<T | void> => {
    const entity: T | void = await fetch(`${basicUrl}`, { method: "GET", headers: getAuthHeader() })
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
    return entity;
}

export const getEntityByConfig = async <T> (basicUrl: string, body: unknown): Promise<T | void> => {
    return await fetch(basicUrl, {method: "POST",  
        body: JSON.stringify(body),
        headers: {"Content-Type": "application/json", ...getAuthHeader()}})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
}

export const getEntityById = async <T> (basicUrl: string, id: string): Promise<T | void> => {
    return getEntity(`${basicUrl}/GetById?id=${id}`);
}

export const convertRecordToJson = <T>(record: T): string => {
    return JSON.stringify(record);
}

const generateForm = <T>(entity: T, entityField: string, iconField: string, file: Nullable<File>) => {
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

export const getAction = async (url: string): Promise<boolean> => {
    const result = await fetch(url, { method: "GET", headers: {...getAuthHeader()}})
        .then(checkPromiseStatus)
        .catch(logPromiseError)

    return result?.ok ?? false;
}

export const postAction = async (url: string, data: unknown): Promise<boolean> => {
    const result = await fetch(url, { method: "POST", body: JSON.stringify(data),  
        headers: {"Content-Type": "application/json", ...getAuthHeader()}})
        .then(checkPromiseStatus)
        .catch(logPromiseError)

    return result?.ok ?? false;
}

export const getPagination = async (url: string): Promise<PaginationConfig | void> => {
    return await fetch(`${url}/GetPagination`, {method: "GET", headers: {...getAuthHeader()}})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
};

export const getAuthHeader = (): HeadersInit => {
    const token = getToken();
    return {
        "Authorization": `Bearer ${token}`
    };
}

const getToken = (): Nullable<string> =>  {
    const token = localStorage.getItem("auth_token");
    if (!token) {
        removeToken();
        return null;
    }

    try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        if (payload.exp * 1000 < Date.now()) {
             removeToken();
            return null;
        }
    } catch {
        removeToken();
        return null;
    }

    return token;
}

const removeToken = (): void => {
    localStorage.removeItem("auth_token");
    if (!window.location.href.endsWith("/auth")){
        window.location.href = "/auth";
    }
}