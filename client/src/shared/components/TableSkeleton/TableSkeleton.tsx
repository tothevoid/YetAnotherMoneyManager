import { Skeleton, Stack } from "@chakra-ui/react";
import React from "react";

interface TableSkeletonProps {
    rows?: number;
    columns?: number;
}

export const TableSkeleton: React.FC<TableSkeletonProps> = ({ rows = 5, columns = 3 }) => {
    return (
        <Stack gap={3} width="100%" py={2}>
            {Array.from({ length: rows }).map((_, rowIndex) => (
                <Stack key={rowIndex} direction="row" gap={4} width="100%" alignItems="center">
                    {Array.from({ length: columns }).map((_, colIndex) => (
                        <Skeleton
                            key={colIndex}
                            height="38px"
                            flex={colIndex === 0 ? 2 : 1}
                            borderRadius="md"
                        />
                    ))}
                </Stack>
            ))}
        </Stack>
    );
};

export default TableSkeleton;
