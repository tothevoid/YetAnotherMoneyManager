import axios from 'axios';
import config from '../../config';
import { Nullable } from '../../shared/utilities/nullable';
import { setAccessToken, clearAccessToken } from '../tokenStorage';

const basicUrl = `${config.api.URL}/Auth`;

export interface AuthResult {
    passwordChangeRequired: boolean;
    token: Nullable<string>;
}

export const auth = async (
    userName: string,
    password: Nullable<string>
): Promise<Nullable<AuthResult>> => {
    try {
        const response = await axios.post(
            `${basicUrl}/Login`,
            { userName, password: password ?? null },
            { withCredentials: true }
        );

        const data = response.data;
        if (data?.passwordChangeRequired) {
            return { passwordChangeRequired: true, token: null };
        }

        if (data?.accessToken) {
            setAccessToken(data.accessToken);
            return { passwordChangeRequired: false, token: data.accessToken };
        }

        return null;
    } catch {
        return null;
    }
};

export const changePassword = async (
    userName: string,
    currentPassword: Nullable<string>,
    newPassword: string
): Promise<Nullable<string>> => {
    try {
        const response = await axios.post(
            `${basicUrl}/ChangePassword`,
            { userName, currentPassword, newPassword },
            { withCredentials: true }
        );

        const data = response.data;
        if (data?.accessToken) {
            setAccessToken(data.accessToken);
            return data.accessToken;
        }

        return null;
    } catch {
        return null;
    }
};

let inFlightRefreshPromise: Nullable<Promise<Nullable<string>>> = null;

export const refreshTokenApi = async (): Promise<Nullable<string>> => {
    if (inFlightRefreshPromise) {
        return inFlightRefreshPromise;
    }

    inFlightRefreshPromise = (async () => {
        try {
            const response = await axios.post(
                `${basicUrl}/RefreshToken`,
                {},
                { withCredentials: true }
            );

            const data = response.data;
            if (data?.accessToken) {
                setAccessToken(data.accessToken);
                return data.accessToken;
            }

            return null;
        } catch {
            clearAccessToken();
            return null;
        } finally {
            inFlightRefreshPromise = null;
        }
    })();

    return inFlightRefreshPromise;
};

export const logoutApi = async (): Promise<void> => {
    try {
        await axios.post(
            `${basicUrl}/RevokeToken`,
            {},
            { withCredentials: true }
        );
    } catch (e) {
        console.error("Logout revoke failed", e);
    } finally {
        clearAccessToken();
    }
};
