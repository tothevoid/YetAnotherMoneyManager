import { NotificationEntity, NotificationEntityResponse } from "../../models/notifications/NotificationEntity";
import { deleteEntity, getAllEntitiesByConfig, getEntity, getPagination, postAction } from "../basicApi";
import { PaginationConfig } from "../../shared/models/PaginationConfig";
import { prepareNotification } from "./notificationApiMapping";

const basicUrl = "Notification";

export interface NotificationsQuery {
    pageIndex: number;
    recordsQuantity: number;
    onlyUnread?: boolean;
    category?: string;
}

export const getNotifications = async (query: NotificationsQuery): Promise<NotificationEntity[]> => {
    return await getAllEntitiesByConfig<NotificationsQuery, NotificationEntityResponse>(`${basicUrl}/GetAll`, query)
        .then((responses: NotificationEntityResponse[]) => (responses || []).map(prepareNotification));
};

export const getNotificationsPagination = async (
    onlyUnread: boolean = false,
    category?: string
): Promise<PaginationConfig | void> => {
    const params = new URLSearchParams();
    if (onlyUnread) params.append("onlyUnread", "true");
    if (category && category !== "All") params.append("category", category);

    const queryString = params.toString();
    const url = queryString ? `${basicUrl}/GetPagination?${queryString}` : `${basicUrl}/GetPagination`;
    return await getPagination(url);
};

export const getUnreadNotificationCount = async (): Promise<number> => {
    const result = await getEntity<number>(`${basicUrl}/unread-count`);
    return typeof result === "number" ? result : 0;
};

export const markNotificationAsRead = async (id: string): Promise<boolean> => {
    return await postAction(`${basicUrl}/${id}/read`, {});
};

export const markAllNotificationsAsRead = async (): Promise<boolean> => {
    return await postAction(`${basicUrl}/read-all`, {});
};

export const deleteNotification = async (id: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, id);
};
