import { Box, Flex, Text } from "@chakra-ui/react";
import { PropsWithChildren, ReactNode } from "react";
import { LuInbox } from "react-icons/lu";

interface Props {
    text: string;
    description?: string;
    icon?: ReactNode;
}

const Placeholder: React.FC<PropsWithChildren<Props>> = ({ text, description, icon, children }) => {
    return (
        <Flex
            direction="column"
            alignItems="center"
            justifyContent="center"
            py={12}
            px={6}
            width="100%"
            backgroundColor="background_primary"
            borderRadius="xl"
            borderWidth="1px"
            borderColor="border_primary"
            textAlign="center"
            gap={3}
        >
            <Box
                display="flex"
                alignItems="center"
                justifyContent="center"
                w="48px"
                h="48px"
                borderRadius="full"
                backgroundColor="background_secondary"
                color="text_secondary"
                fontSize="24px"
                mb={1}
            >
                {icon ?? <LuInbox />}
            </Box>
            <Text color="text_primary" fontSize="md" fontWeight="500">
                {text}
            </Text>
            {description && (
                <Text color="text_secondary" fontSize="sm" maxW="400px">
                    {description}
                </Text>
            )}
            {children && (
                <Box mt={2}>
                    {children}
                </Box>
            )}
        </Flex>
    );
};

export default Placeholder;