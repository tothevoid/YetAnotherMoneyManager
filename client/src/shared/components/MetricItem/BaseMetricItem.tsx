import React, { ReactNode } from "react";
import { Box, HStack, Stack, Text } from "@chakra-ui/react";

export interface BaseMetricItemProps {
    icon: ReactNode;
    iconBg: string;
    iconColor: string;
    label: string;
    value: ReactNode;
    valueColor?: string;
    size?: "sm" | "md";
}

export const BaseMetricItem: React.FC<BaseMetricItemProps> = ({
    icon,
    iconBg,
    iconColor,
    label,
    value,
    valueColor = "text_primary",
    size = "md",
}) => {
    return (
        <HStack gap={size === "sm" ? 2 : 2.5} alignItems="center">
            <Box
                p={size === "sm" ? 1.5 : 2}
                borderRadius="md"
                backgroundColor={iconBg}
                color={iconColor}
                display="flex"
                alignItems="center"
                justifyContent="center"
            >
                {icon}
            </Box>
            <Stack gap={0}>
                <Text
                    fontSize="10px"
                    fontWeight={600}
                    textTransform="uppercase"
                    color="text_secondary"
                    letterSpacing="0.5px"
                >
                    {label}
                </Text>
                <Text fontSize={size === "sm" ? "sm" : "md"} fontWeight={800} color={valueColor}>
                    {value ?? "—"}
                </Text>
            </Stack>
        </HStack>
    );
};

export default BaseMetricItem;
