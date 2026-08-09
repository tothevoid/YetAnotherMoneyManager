import React, { ReactNode } from "react";
import { Box, HStack, Stack, Text } from "@chakra-ui/react";

interface HeaderMetricItemProps {
    icon: ReactNode;
    iconBg: string;
    iconColor: string;
    label: string;
    value: string;
    valueColor?: string;
}

export const HeaderMetricItem: React.FC<HeaderMetricItemProps> = ({
    icon,
    iconBg,
    iconColor,
    label,
    value,
    valueColor = "text_primary",
}) => {
    return (
        <HStack gap={2} alignItems="center">
            <Box
                p={1.5}
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
                <Text fontSize="10px" fontWeight={600} textTransform="uppercase" color="text_secondary">
                    {label}
                </Text>
                <Text fontSize="sm" fontWeight={800} color={valueColor}>
                    {value}
                </Text>
            </Stack>
        </HStack>
    );
};
