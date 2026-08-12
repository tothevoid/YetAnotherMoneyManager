import { forwardRef, useImperativeHandle, useEffect, useState } from "react";
import { Dialog, Portal, Button, CloseButton, useDisclosure, Stack, Text, Box } from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { DebtEntity } from "../../../../models/debts/DebtEntity";
import { DebtTagEntity } from "../../../../models/debts/DebtTagEntity";
import DebtTagSelect from "../../components/DebtTagSelect/DebtTagSelect";
import DebtTagEditor from "../../components/DebtTagEditor/DebtTagEditor";
import { createDebtTag } from "../../../../api/debts/debtTagApi";
import { MdAdd } from "react-icons/md";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";

interface Props {
    debt: DebtEntity | null;
    availableTags: DebtTagEntity[];
    onSaved: (debt: DebtEntity, newTags: DebtTagEntity[]) => Promise<void>;
    onTagsReloadRequested: () => Promise<void>;
}

export const DebtTagAssignModal = forwardRef<BaseModalRef, Props>(
    ({ debt, availableTags, onSaved, onTagsReloadRequested }, ref) => {
        const { open, onOpen, onClose } = useDisclosure();
        const { t } = useTranslation();
        const [selectedTags, setSelectedTags] = useState<DebtTagEntity[]>([]);

        useImperativeHandle(ref, () => ({
            openModal: onOpen,
            closeModal: onClose
        }));

        useEffect(() => {
            if (debt) {
                setSelectedTags(debt.debtTags || []);
            }
        }, [debt]);

        const handleSave = async () => {
            if (!debt) return;
            await onSaved(debt, selectedTags);
            onClose();
        };

        const handleCreateTag = async (name: string, colorHex: string) => {
            const createdTag = await createDebtTag({ name, colorHex });
            await onTagsReloadRequested();
            if (createdTag) {
                setSelectedTags((prev) => [...prev, createdTag]);
            }
        };

        return (
            <Dialog.Root onEscapeKeyDown={onClose} placement="center" open={open}>
                <Portal>
                    <Dialog.Backdrop />
                    <Dialog.Positioner>
                        <Dialog.Content backgroundColor="background_primary" borderColor="border_primary" color="text_primary">
                            <Dialog.Header fontSize="lg" fontWeight="bold">
                                {t("debt_tag_assign_modal_title", { name: debt?.name || "" })}
                            </Dialog.Header>

                            <Dialog.Body>
                                <Stack gap={4}>
                                    <Text fontSize="sm" color="gray.400">
                                        {t("debt_tag_assign_modal_subtitle")}
                                    </Text>
                                    <Box>
                                        <DebtTagSelect
                                            availableTags={availableTags}
                                            selectedTags={selectedTags}
                                            onChange={setSelectedTags}
                                        />
                                    </Box>
                                    <Box borderTopWidth="1px" borderColor="border_primary" pt={3}>
                                        <DebtTagEditor
                                            existingTags={availableTags}
                                            submitButtonText={t("debt_tag_create")}
                                            submitButtonColorPalette="green"
                                            onSubmit={handleCreateTag}
                                            icon={<MdAdd />}
                                        />
                                    </Box>
                                </Stack>
                            </Dialog.Body>

                            <Dialog.Footer>
                                <Dialog.ActionTrigger asChild>
                                    <Button onClick={onClose} variant="outline" color="text_primary">
                                        {t("modals_close_button")}
                                    </Button>
                                </Dialog.ActionTrigger>
                                <Button colorPalette="green" onClick={handleSave}>
                                    {t("modals_save_button")}
                                </Button>
                            </Dialog.Footer>

                            <Dialog.CloseTrigger asChild>
                                <CloseButton onClick={onClose} size="sm" />
                            </Dialog.CloseTrigger>
                        </Dialog.Content>
                    </Dialog.Positioner>
                </Portal>
            </Dialog.Root>
        );
    }
);
