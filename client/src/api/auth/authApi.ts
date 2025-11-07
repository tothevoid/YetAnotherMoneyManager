import config from '../../config' 
import { Nullable } from '../../shared/utilities/nullable';

const basicUrl = `${config.api.URL}/Auth`;

export const auth = async (userName: string, password: Nullable<string>): Promise<Nullable<string>> => {
    const response = await fetch(`${basicUrl}/Login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ userName: userName, password: password ?? null })
    });

    if (!response.ok) return null
    const parsedResponse = await response.json();
    return parsedResponse?.token;
}

