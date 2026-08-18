import { PaginationConfig } from '../../shared/models/PaginationConfig';
import { UserRefreshTokenEntity } from '../../models/auth/UserRefreshTokenEntity';
import { getAllEntities, getEntity } from '../basicApi';
import httpClient from '../httpClient';
import { logPromiseError } from '../../shared/utilities/webApiUtilities';

const basicUrl = 'Auth';

export const getRefreshTokens = async (
    isActive: boolean = true,
    pageIndex: number = 1,
    recordsQuantity: number = 10
): Promise<UserRefreshTokenEntity[]> => {
    const url = `${basicUrl}/RefreshTokens?isActive=${isActive}&pageIndex=${pageIndex}&recordsQuantity=${recordsQuantity}`;
    const result = await getAllEntities<UserRefreshTokenEntity>(url);
    return result ?? [];
};

export const getRefreshTokensPagination = async (
    isActive: boolean = true
): Promise<PaginationConfig | void> => {
    const url = `${basicUrl}/RefreshTokens/Pagination?isActive=${isActive}`;
    return await getEntity<PaginationConfig>(url);
};

export const revokeToken = async (id: string): Promise<boolean> => {
    const url = `${basicUrl}/RefreshTokens/${id}`;
    const result = await httpClient.delete(url)
        .then(() => true)
        .catch(logPromiseError);
    return result ?? false;
};

export const revokeOtherTokens = async (): Promise<boolean> => {
    const url = `${basicUrl}/RevokeOthers`;
    const result = await httpClient.post(url, {})
        .then(() => true)
        .catch(logPromiseError);
    return result ?? false;
};
