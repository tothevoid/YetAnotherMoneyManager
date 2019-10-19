export const checkPromiseStatus = (response: Response) => 
    (response.status >= 200 && response.status < 300) ?
        Promise.resolve(response) :
        Promise.reject(new Error(response.statusText));

export const logPromiseError = (reason: any) => 
    console.error(reason);