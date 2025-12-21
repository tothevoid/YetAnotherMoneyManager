import config from '../../config' 
import { Nullable } from '../../shared/utilities/nullable';

const basicUrl = `${config.api.URL}/Auth`;

export const auth = async (userName: string, password: Nullable<string>)
    : Promise<Nullable<{passwordChangeRequired: boolean, token: Nullable<string>}>> => {
    const response = await fetch(`${basicUrl}/Login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ userName: userName, password: password ?? null })
    });

    if (!response.ok) return null
    const parsedResponse = await response.json();
    if (parsedResponse?.passwordChangeRequired) {
        return { passwordChangeRequired: true, token: null };
    } else if (parsedResponse?.token) {
        return { passwordChangeRequired: false, token: parsedResponse.token };
    }
    return null;
}

export const changePassword = async (userName: string, currentPassword: Nullable<string>, 
    newPassword: string)
    : Promise<Nullable<string>> => {
    const response = await fetch(`${basicUrl}/ChangePassword`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ userName, currentPassword, newPassword })
    });

    if (!response.ok) return null
    const parsedResponse = await response.json();
    return parsedResponse?.token ?? null;
}

