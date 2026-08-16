export enum NotificationSeverity {
    Info = 1,
    Success = 2,
    Warning = 3,
    Danger = 4
}

export interface NotificationEntity {
    id: string;
    userProfileId: string;
    title: string;
    message: string;
    severity: NotificationSeverity;
    actionUrl?: string;
    category: string;
    isRead: boolean;
    createdAt: Date;
    readAt?: Date;
}

export interface NotificationEntityResponse {
    id: string;
    userProfileId: string;
    title: string;
    message: string;
    severity: NotificationSeverity;
    actionUrl?: string;
    category: string;
    isRead: boolean;
    createdAt: string;
    readAt?: string;
}
