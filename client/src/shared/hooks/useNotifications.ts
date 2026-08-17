import { useCallback, useEffect, useState } from "react";
import { NotificationEntity } from "../../models/notifications/NotificationEntity";
import {
    deleteNotification as apiDeleteNotification,
    getNotifications,
    getNotificationsPagination,
    getUnreadNotificationCount,
    markAllNotificationsAsRead as apiMarkAllAsRead,
    markNotificationAsRead as apiMarkAsRead
} from "../../api/notifications/notificationApi";
import { useNotificationEvents } from "./useNotificationEvents";

export interface UseNotificationsOptions {
    autoLoad?: boolean;
    initialOnlyUnread?: boolean;
    initialCategory?: string;
}

export const useNotifications = (options: UseNotificationsOptions = {}) => {
    const {
        autoLoad = true,
        initialOnlyUnread = false,
        initialCategory = "All"
    } = options;

    const [notifications, setNotifications] = useState<NotificationEntity[]>([]);
    const [unreadCount, setUnreadCount] = useState<number>(0);
    const [isLoading, setIsLoading] = useState<boolean>(false);

    // Filter states
    const [onlyUnreadFilter, setOnlyUnreadFilter] = useState<boolean>(initialOnlyUnread);
    const [selectedCategory, setSelectedCategory] = useState<string>(initialCategory);

    // Pagination states
    const [pageIndex, setPageIndex] = useState<number>(1);
    const [recordsQuantity, setRecordsQuantity] = useState<number>(0);
    const [hasMore, setHasMore] = useState<boolean>(false);
    const [isLoadingMore, setIsLoadingMore] = useState<boolean>(false);

    const loadUnreadCount = useCallback(async () => {
        try {
            const count = await getUnreadNotificationCount();
            setUnreadCount(count);
        } catch (error) {
            console.error("Failed to load unread notification count", error);
        }
    }, []);

    const loadPage = useCallback(async (page: number, size: number) => {
        setIsLoading(true);
        try {
            const [items, count] = await Promise.all([
                getNotifications({
                    pageIndex: page,
                    recordsQuantity: size,
                    onlyUnread: onlyUnreadFilter,
                    category: selectedCategory
                }),
                getUnreadNotificationCount()
            ]);
            setNotifications(items);
            setUnreadCount(count);
            setHasMore(items.length >= size);
        } catch (error) {
            console.error("Failed to load notifications", error);
        } finally {
            setIsLoading(false);
        }
    }, [onlyUnreadFilter, selectedCategory]);

    const loadMore = useCallback(async () => {
        if (isLoadingMore || !hasMore) return;
        setIsLoadingMore(true);
        try {
            const nextPage = Math.floor(notifications.length / recordsQuantity) + 1;
            const nextItems = await getNotifications({
                pageIndex: nextPage,
                recordsQuantity,
                onlyUnread: onlyUnreadFilter,
                category: selectedCategory
            });
            if (nextItems.length < recordsQuantity) {
                setHasMore(false);
            }
            setNotifications(prev => {
                const existingIds = new Set(prev.map(n => n.id));
                const uniqueNew = nextItems.filter(n => !existingIds.has(n.id));
                return [...prev, ...uniqueNew];
            });
        } catch (error) {
            console.error("Failed to load more notifications", error);
        } finally {
            setIsLoadingMore(false);
        }
    }, [isLoadingMore, hasMore, notifications.length, recordsQuantity, onlyUnreadFilter, selectedCategory]);

    const onPageChanged = useCallback((size: number, page: number) => {
        const actualPage = page === 0 ? 1 : page;
        setPageIndex(actualPage);
        setRecordsQuantity(size);
        loadPage(actualPage, size);
    }, [loadPage]);

    const getPaginationConfig = useCallback(async () => {
        return await getNotificationsPagination(onlyUnreadFilter, selectedCategory);
    }, [onlyUnreadFilter, selectedCategory]);

    useEffect(() => {
        if (autoLoad) {
            getPaginationConfig().then(config => {
                const size = config?.pageSize || 15;
                setRecordsQuantity(size);
                loadPage(1, size);
            });
        }
    }, [autoLoad, getPaginationConfig, loadPage]);

    useNotificationEvents({
        onNotificationReceived: useCallback((notification: NotificationEntity) => {
            setUnreadCount(prev => prev + 1);
            if (pageIndex === 1) {
                setNotifications(prev => [notification, ...prev.filter(n => n.id !== notification.id)].slice(0, recordsQuantity));
            }
        }, [pageIndex, recordsQuantity]),
        onNotificationRead: useCallback((notificationId: string) => {
            setNotifications(prev =>
                prev.map(n => n.id === notificationId ? { ...n, isRead: true, readAt: new Date() } : n)
            );
            setUnreadCount(prev => Math.max(0, prev - 1));
        }, []),
        onAllNotificationsRead: useCallback(() => {
            setNotifications(prev =>
                prev.map(n => ({ ...n, isRead: true, readAt: new Date() }))
            );
            setUnreadCount(0);
        }, [])
    });

    const markAsRead = async (id: string, e?: React.MouseEvent) => {
        e?.stopPropagation();
        setNotifications(prev =>
            prev.map(n => n.id === id ? { ...n, isRead: true, readAt: new Date() } : n)
        );
        setUnreadCount(prev => Math.max(0, prev - 1));
        try {
            await apiMarkAsRead(id);
            if (onlyUnreadFilter) {
                loadPage(pageIndex, recordsQuantity);
            }
        } catch (error) {
            console.error("Failed to mark notification as read", error);
            loadPage(pageIndex, recordsQuantity);
        }
    };

    const markAllAsRead = async () => {
        setNotifications(prev =>
            prev.map(n => ({ ...n, isRead: true, readAt: new Date() }))
        );
        setUnreadCount(0);
        try {
            await apiMarkAllAsRead();
            if (onlyUnreadFilter) {
                loadPage(1, recordsQuantity);
            }
        } catch (error) {
            console.error("Failed to mark all notifications as read", error);
            loadPage(1, recordsQuantity);
        }
    };

    const deleteNotification = async (id: string) => {
        setNotifications(prev => prev.filter(n => n.id !== id));
        try {
            await apiDeleteNotification(id);
            loadPage(pageIndex, recordsQuantity);
        } catch (error) {
            console.error("Failed to delete notification", error);
            loadPage(pageIndex, recordsQuantity);
        }
    };

    const categories = Array.from(new Set(notifications.map(n => n.category || "System")));

    return {
        notifications,
        unreadCount,
        isLoading,
        hasMore,
        isLoadingMore,
        onlyUnreadFilter,
        selectedCategory,
        categories,
        setOnlyUnreadFilter,
        setSelectedCategory,
        loadPage,
        loadMore,
        onPageChanged,
        getPaginationConfig,
        markAsRead,
        markAllAsRead,
        deleteNotification,
        refresh: () => loadPage(pageIndex, recordsQuantity),
        loadUnreadCount
    };
};
