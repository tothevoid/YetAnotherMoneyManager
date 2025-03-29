import { Box, Stack, Link, Text } from "@chakra-ui/react";
import { useParams } from "react-router-dom";
import { getDataTablesConfig } from "../Tables/dataTablesUtilities";
import { useTranslation } from "react-i18next";

const dataTablesConfig = getDataTablesConfig();

export default function SettingsSidebar() {
	const { tab = dataTablesConfig[0].id } = useParams();
	const { t } = useTranslation();

	return (
		<Box color={"text_primary"} bg="background_primary" w={{ base: "full", md: 60 }} pos="fixed" h="full"
			borderRight="1px" borderColor={"white"}>
			<Stack gap={4} p={4}>
				<Text fontSize="xl" fontWeight="bold" p={2}>
					{t("data_title")}
				</Text>
				{dataTablesConfig.map((menuItem) => (
					<Link key={menuItem.id} href={menuItem.href} p={2} borderRadius="md"
						bg={tab === menuItem.id ? "purple.500" : "transparent"}
						color="text_primary"
						_hover={{bg: "purple.500"}}>
						{menuItem.label}
					</Link>
				))}
			</Stack>
		</Box>
	);
}