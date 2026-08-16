import { NotificationEntity, NotificationEntityResponse } from "../../models/notifications/NotificationEntity";
import { deleteEntity, getAllEntitiesByConfig, getEntity, postAction } from "../basicApi";
import { prepareNotification } from "./notificationApiMapping";

const basicUrl = "Notification";

export const getNotifications = async (onlyUnread?: boolean): Promise<NotificationEntity[]> => {
    return await getAllEntitiesByConfig<unknown, NotificationEntityResponse>(
        basicUrl,
        onlyUnread !== undefined ? { onlyUnread } : {}
    ).then((responses: NotificationEntityResponse[]) => responses.map(prepareNotification));
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
