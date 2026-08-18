import React from 'react';
import { Flex, HStack, Icon, Text, VStack } from '@chakra-ui/react';
import { MdChevronRight } from 'react-icons/md';

interface HeaderProfileMenuItemProps {
    icon: React.ReactNode;
    title: string;
    description: string;
    onClick: () => void;
    isDanger?: boolean;
}

export const HeaderProfileMenuItem: React.FC<HeaderProfileMenuItemProps> = ({
    icon,
    title,
    description,
    onClick,
    isDanger = false
}) => (
    <Flex
        as="button"
        align="center"
        justify="space-between"
        p={2.5}
        borderRadius="xl"
        cursor="pointer"
        transition="all 0.15s ease"
        _hover={{
            backgroundColor: isDanger ? 'rgba(220, 38, 38, 0.08)' : 'rgba(255, 255, 255, 0.05)',
            transform: 'translateX(3px)'
        }}
        onClick={onClick}
    >
        <HStack gap={3}>
            <Flex
                w="36px"
                h="36px"
                borderRadius="lg"
                backgroundColor={isDanger ? 'rgba(220, 38, 38, 0.12)' : 'rgba(255, 255, 255, 0.05)'}
                color={isDanger ? '#f87171' : 'card_action_icon_primary'}
                align="center"
                justify="center"
                fontSize="18px"
                border="1px solid"
                borderColor={isDanger ? 'rgba(220, 38, 38, 0.25)' : 'rgba(255, 255, 255, 0.08)'}
            >
                {icon}
            </Flex>
            <VStack align="flex-start" gap={0}>
                <Text fontSize="sm" fontWeight="medium" color={isDanger ? '#f87171' : 'text_primary'}>
                    {title}
                </Text>
                <Text fontSize="xs" color="text_secondary">
                    {description}
                </Text>
            </VStack>
        </HStack>
        <Icon fontSize="18px" color={isDanger ? '#f87171' : 'text_secondary'}>
            <MdChevronRight />
        </Icon>
    </Flex>
);
