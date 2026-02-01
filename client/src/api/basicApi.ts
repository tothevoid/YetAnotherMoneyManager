import config from "../config";
import { PaginationConfig } from "../shared/models/PaginationConfig";
import { Nullable } from "../shared/utilities/nullable";
import { logPromiseError } from "../shared/utilities/webApiUtilities";
import axios from "axios";

const api = axios.create({
    baseURL: config.api.URL,
});

api.interceptors.request.use(config => {
    const token = getToken();
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
});

export const getAllEntities = async <T> (basicUrl: string): Promise<T[]> => {
    const entities = await api.get(basicUrl)
        .then((response) => response.data)
        .catch(logPromiseError);
  
    return entities ?? [] as T[];
};

export const getAllEntitiesByConfig = async <TInput, TOutput> (basicUrl: string, data: TInput): Promise<TOutput[]> => {
    const entities = await api.post(basicUrl, data)
        .then((response) => response.data)
        .catch(logPromiseError);

    return entities ?? [] as TOutput[];
};

export const createEntity = async<TRequest, TResponse> (basicUrl: string, addedEntity: TRequest): Promise<TResponse | void> => {
    return sendCreateRequest(basicUrl, addedEntity, (id) => {return {...addedEntity, id} as TResponse})
}

export const createAndGetFullEntity = async<TRequest, TResponse> (basicUrl: string, addedEntity: TRequest): Promise<TResponse | void> => {
    return sendCreateRequest(basicUrl, addedEntity, (createdEntity) => {return {...createdEntity} as TResponse})
}

export const createEntityWithIcon = async<TRequest, TResponse>(basicUrl: string, addedEntity: TRequest, 
        entityFieldName: string, iconFieldName: string, file: Nullable<File>): Promise<TResponse | void> => {
    return await api.put(basicUrl, generateForm(addedEntity, entityFieldName, iconFieldName, file))
        .then((response) => response.data)
        .then(responseEntity => {
            return {...responseEntity} as TResponse;
        })
        .catch(logPromiseError);
}

export const updateEntity = async<TRequest> (basicUrl: string, modifiedEntity: TRequest): Promise<boolean> => {
    const updatedEntity = await api.patch(basicUrl, modifiedEntity)
        .then(() => true)
        .catch(logPromiseError);

    return updatedEntity ?? false;
}

export const updateEntityWithIcon = async<TRequest, TResponse> (basicUrl: string, modifiedEntity: TRequest, 
    entityFieldName: string, iconFieldName: string, file: Nullable<File>): Promise<TResponse | void> => {

    return await api.patch(basicUrl, generateForm(modifiedEntity, entityFieldName, iconFieldName, file ))
        .then((response) => response.data)
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
    const result = await api.delete(url)
        .then(() => true)
        .catch(logPromiseError);

    return result ?? false;
}

export const getEntity = async <T> (basicUrl: string): Promise<T | void> => {
    const entity: T | void = await api.get(`${basicUrl}`)
        .then((response) => response.data)
        .catch(logPromiseError);
    return entity;
}

export const getEntityByConfig = async <T> (basicUrl: string, body: unknown): Promise<T | void> => {
    return await api.post(basicUrl, body)
        .then((response) => response.data)
        .catch(logPromiseError);
}

export const getEntityById = async <T> (basicUrl: string, id: string): Promise<T | void> => {
    return getEntity(`${basicUrl}/GetById?id=${id}`);
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
    const newEntity = await api.put(basicUrl, addedEntity)
        .then((response) => response.data)
        .then(responseHandler)
        .catch(logPromiseError);
    return newEntity;
}

export const getAction = async (url: string): Promise<boolean> => {
    const result = await api.get(url)
        .then(() => true)
        .catch(logPromiseError)

    return result ?? false;
}

export const postAction = async (url: string, data: unknown): Promise<boolean> => {
    const result = await api.post(url, data)
        .then(() => true)
        .catch(logPromiseError)

    return result ?? false;
}

export const getPagination = async (url: string): Promise<PaginationConfig | void> => {
    return await api.get(url)
        .then((response) => response.data)
        .catch(logPromiseError);
};

export const getAuthHeader = (): HeadersInit => {
    const token = getToken();
    return {
        "Authorization": `Bearer ${token}`
    };
}

export const downloadFileByUrl = async (url: string): Promise<Blob | null> => {
    try {
        const response = await api.get(url, { responseType: "blob" });
        return response.data;
    } catch (e) {
        logPromiseError(e);
        return null;
    }
};

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