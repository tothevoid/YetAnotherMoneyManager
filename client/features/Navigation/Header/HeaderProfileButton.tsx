import React from 'react';
import { Button, Flex, Icon, Text } from '@chakra-ui/react';
import { MdKeyboardArrowDown, MdPerson } from 'react-icons/md';

interface HeaderProfileButtonProps {
    userName: string;
    userInitial: string;
    isOpen: boolean;
}

export const HeaderProfileButton = React.forwardRef<HTMLButtonElement, HeaderProfileButtonProps>(
    ({ userName, userInitial, isOpen, ...props }, ref) => (
        <Button
            ref={ref}
            size="md"
            borderRadius="full"
            backgroundColor="rgba(255, 255, 255, 0.04)"
            borderColor="border_primary"
            borderWidth="1px"
            px={2.5}
            py={1.5}
            height="40px"
            display="flex"
            alignItems="center"
            gap={2}
            cursor="pointer"
            transition="all 0.2s cubic-bezier(0.4, 0, 0.2, 1)"
            _hover={{
                backgroundColor: 'rgba(255, 255, 255, 0.08)',
                borderColor: 'action_primary',
                boxShadow: '0 0 12px rgba(10, 142, 58, 0.25)'
            }}
            {...props}
        >
            <Flex
                w="26px"
                h="26px"
                borderRadius="full"
                background="linear-gradient(135deg, #0a8e3a 0%, #055021 100%)"
                color="white"
                align="center"
                justify="center"
                fontSize="xs"
                fontWeight="bold"
                boxShadow="0 2px 6px rgba(0,0,0,0.4)"
            >
                {userInitial || <Icon><MdPerson /></Icon>}
            </Flex>

            <Text
                fontSize="sm"
                fontWeight="medium"
                color="text_primary"
                maxW="110px"
                truncate
            >
                {userName}
            </Text>

            <Icon
                fontSize="16px"
                color="text_secondary"
                transition="transform 0.2s ease"
                transform={isOpen ? 'rotate(180deg)' : 'rotate(0deg)'}
            >
                <MdKeyboardArrowDown />
            </Icon>
        </Button>
    )
);

HeaderProfileButton.displayName = 'HeaderProfileButton';
