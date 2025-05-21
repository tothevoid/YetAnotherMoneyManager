import { Box, Flex } from "@chakra-ui/react";
import DataSidebar from "../DataSidebar/DataSidebar";

export default function DataLayout({ children }: { children: React.ReactNode }) {
  return (
    <Flex>
      <DataSidebar />
      <Box flex={1} p={6} ml={{ base: 0, md: 60 }}>
        {children}
      </Box>
    </Flex>
  );
}