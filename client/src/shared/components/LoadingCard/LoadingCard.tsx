import { Box, Flex, ProgressCircle } from "@chakra-ui/react";
import React from "react";

interface Props {
    minH?: string | number;
    p?: string | number;
    borderRadius?: string;
}

const LoadingCard: React.FC<Props> = ({ minH, p = 8, borderRadius = "xl" }) => {
    return (
        <Box
            bg="background_primary"
            border="1px solid"
            borderColor="border_primary"
            borderRadius={borderRadius}
            p={p}
            minH={minH}
            width="100%"
        >
            <Flex justifyContent="center" alignItems="center" height="100%">
                <ProgressCircle.Root color="spinner_primary">
                    <ProgressCircle.Circle>
                        <ProgressCircle.Track />
                        <ProgressCircle.Range />
                    </ProgressCircle.Circle>
                </ProgressCircle.Root>
            </Flex>
        </Box>
    );
};

export default LoadingCard;
