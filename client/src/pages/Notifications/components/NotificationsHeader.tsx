import React from "react";
import { Badge, Button, Flex, HStack, Icon, Text } from "@chakra-ui/react";
import { MdDoneAll, MdNotifications } from "react-icons/md";
import { useTranslation } from "react-i18next";

interface NotificationsHeaderProps {
    unreadCount: number;
    onMarkAllAsRead: () => void;
}

export const NotificationsHeader: React.FC<NotificationsHeaderProps> = ({
    unreadCount,
    onMarkAllAsRead
}) => {
    const { t } = useTranslation();

    return (
        <Flex justify="space-between" align="center" wrap="wrap" gap={3}>
            <HStack gap={3}>
                <Icon fontSize="28px" color="action_primary">
                    <MdNotifications />
                </Icon>
                <Text fontSize="2xl" fontWeight="bold" color="text_primary">
                    {t("notifications_page_title")}
                </Text>
                {unreadCount > 0 && (
                    <Badge colorPalette="red" variant="solid" borderRadius="full" px={2} py={0.5}>
                        {unreadCount} {t("notifications_unread_count")}
                    </Badge>
                )}
            </HStack>

            {unreadCount > 0 && (
                <Button
                    size="sm"
                    variant="outline"
                    borderColor="border_primary"
                    color="action_primary"
                    onClick={onMarkAllAsRead}
                >
                    <Icon mr={1}><MdDoneAll /></Icon>
                    {t("notifications_mark_all_as_read")}
                </Button>
            )}
        </Flex>
    );
};
