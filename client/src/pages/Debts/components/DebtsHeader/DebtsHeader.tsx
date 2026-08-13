import React from "react";
import { Stack, Flex, Button } from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { MdSettings, MdBarChart } from "react-icons/md";
import AddButton from "../../../../shared/components/AddButton/AddButton";
import SwitchButton from "../../../../shared/components/SwitchButton/SwitchButton";
import Placeholder from "../../../../shared/components/Placeholder/Placeholder";
import DebtTagBadge from "../DebtTagBadge/DebtTagBadge";
import { DebtTagEntity } from "../../../../models/debts/DebtTagEntity";

interface Props {
    hasDebts: boolean;
    tags: DebtTagEntity[];
    onlyActive: boolean;
    onOnlyActiveChange: (onlyActive: boolean) => void;
    selectedTagFilter: string | null;
    onSelectedTagFilterChange: (tagId: string | null) => void;
    onAddClicked: () => void;
    onOpenTagManagerModal?: () => void;
    onOpenTagStatsModal?: () => void;
}

export const DebtsHeader: React.FC<Props> = ({
    hasDebts,
    tags,
    onlyActive,
    onOnlyActiveChange,
    selectedTagFilter,
    onSelectedTagFilterChange,
    onAddClicked,
    onOpenTagManagerModal,
    onOpenTagStatsModal,
}) => {
    const { t } = useTranslation();

    const addButton = <AddButton buttonTitle={t("debts_page_add_debt")} onClick={onAddClicked} />;

    if (!hasDebts) {
        return <Placeholder text={t("debts_page_no_debts")}>{addButton}</Placeholder>;
    }

    return (
        <Stack gap={3}>
            <Flex justifyContent="flex-start" alignItems="center" wrap="wrap" gap={3}>
                {addButton}

                {onOpenTagStatsModal && (
                    <Button
                        size="xs"
                        variant="outline"
                        onClick={onOpenTagStatsModal}
                        color="text_primary"
                        borderColor="border_primary"
                        _hover={{
                            backgroundColor: "background_secondary",
                            borderColor: "action_primary",
                            color: "text_primary",
                        }}
                    >
                        <MdBarChart /> {t("debt_tag_stats_btn")}
                    </Button>
                )}

                {onOpenTagManagerModal && (
                    <Button
                        size="xs"
                        variant="outline"
                        onClick={onOpenTagManagerModal}
                        color="text_primary"
                        borderColor="border_primary"
                        _hover={{
                            backgroundColor: "background_secondary",
                            borderColor: "action_primary",
                            color: "text_primary",
                        }}
                    >
                        <MdSettings /> {t("debt_tag_manage_title")}
                    </Button>
                )}
            </Flex>

            <Flex gap={3} alignItems="center" wrap="wrap">
                <SwitchButton
                    active={onlyActive}
                    title={t("debts_page_only_active")}
                    onSwitch={onOnlyActiveChange}
                />

                {tags && tags.length > 0 && (
                    <Flex gap={2} alignItems="center" wrap="wrap">
                        <DebtTagBadge
                            name={t("debts_all_tags")}
                            isSelected={selectedTagFilter === null}
                            cursor="pointer"
                            onClick={() => onSelectedTagFilterChange(null)}
                            px={3}
                            py={1}
                        />
                        {tags.map((tag) => {
                            const isSelected = selectedTagFilter === tag.id;
                            return (
                                <DebtTagBadge
                                    key={tag.id}
                                    name={tag.name}
                                    colorHex={tag.colorHex}
                                    isSelected={isSelected}
                                    cursor="pointer"
                                    onClick={() => onSelectedTagFilterChange(isSelected ? null : tag.id)}
                                    px={3}
                                    py={1}
                                />
                            );
                        })}
                    </Flex>
                )}
            </Flex>
        </Stack>
    );
};

export default DebtsHeader;
