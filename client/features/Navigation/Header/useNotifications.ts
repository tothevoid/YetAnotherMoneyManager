import { useCallback, useEffect, useState } from "react";
import { NotificationEntity } from "../../../src/models/notifications/NotificationEntity";
import {
    getNotifications,
    getUnreadNotificationCount,
    markAllNotificationsAsRead,
    markNotificationAsRead
} from "../../../src/api/notifications/notificationApi";
import { prepareNotification } from "../../../src/api/notifications/notificationApiMapping";
import { useSignalR } from "../../../src/shared/hooks/SignalRHook";

export const useNotifications = () => {
    const [notifications, setNotifications] = useState<NotificationEntity[]>([]);
    const [unreadCount, setUnreadCount] = useState<number>(0);
    const [isLoading, setIsLoading] = useState<boolean>(false);

    const loadNotifications = useCallback(async () => {
        setIsLoading(true);
        try {
            const [items, count] = await Promise.all([
                getNotifications(),
                getUnreadNotificationCount()
            ]);
            setNotifications(items);
            setUnreadCount(count);
        } catch (error) {
            console.error("Failed to load notifications", error);
        } finally {
            setIsLoading(false);
        }
    }, []);

    useEffect(() => {
        const token = localStorage.getItem("auth_token");
        if (token) {
            loadNotifications();
        }
    }, [loadNotifications]);

    const handleSignalRMessage = useCallback(async (rawMessage: string) => {
        try {
            const data = typeof rawMessage === "string" ? JSON.parse(rawMessage) : rawMessage;
            if (data?.type === "NotificationReceived" && data.payload) {
                const newNotification = prepareNotification(data.payload);
                setNotifications(prev => [newNotification, ...prev.filter(n => n.id !== newNotification.id)]);
                setUnreadCount(prev => prev + 1);
            } else if (data?.type === "NotificationRead" && data.notificationId) {
                setNotifications(prev =>
                    prev.map(n => n.id === data.notificationId ? { ...n, isRead: true, readAt: new Date() } : n)
                );
                setUnreadCount(prev => Math.max(0, prev - 1));
            } else if (data?.type === "AllNotificationsRead") {
                setNotifications(prev =>
                    prev.map(n => ({ ...n, isRead: true, readAt: new Date() }))
                );
                setUnreadCount(0);
            }
        } catch {
            // Ignore non-JSON messages
        }
    }, []);

    useSignalR(handleSignalRMessage);

    const markAsRead = async (id: string) => {
        setNotifications(prev =>
            prev.map(n => n.id === id ? { ...n, isRead: true, readAt: new Date() } : n)
        );
        setUnreadCount(prev => Math.max(0, prev - 1));
        try {
            await markNotificationAsRead(id);
        } catch (error) {
            console.error("Failed to mark notification as read", error);
            loadNotifications();
        }
    };

    const markAllAsRead = async () => {
        setNotifications(prev =>
            prev.map(n => ({ ...n, isRead: true, readAt: new Date() }))
        );
        setUnreadCount(0);
        try {
            await markAllNotificationsAsRead();
        } catch (error) {
            console.error("Failed to mark all notifications as read", error);
            loadNotifications();
        }
    };

    return {
        notifications,
        unreadCount,
        isLoading,
        markAsRead,
        markAllAsRead,
        refresh: loadNotifications
    };
};
