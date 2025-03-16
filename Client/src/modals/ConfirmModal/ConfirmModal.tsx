import { Button, CloseButton, Dialog, Portal, useDisclosure } from "@chakra-ui/react"
import React, { useImperativeHandle } from "react";
import { forwardRef } from "react"
import { useTranslation } from "react-i18next";

export interface ConfirmModalRef {
    openModal: () => void
}

interface Props {
    onConfirmed: () => void
    title: string,
    message: string,
    confirmActionName: string
}

export const ConfirmModal = forwardRef<ConfirmModalRef, Props>((props: Props, ref) => {
    const { open, onOpen, onClose } = useDisclosure();
    const cancelRef = React.useRef<HTMLButtonElement>(null!);
    useImperativeHandle(ref, () => ({
        openModal: onOpen,
    }));

    const onConfirmed = () => {
        onClose();
        props.onConfirmed();
    }

    const { t } = useTranslation();

    return <Dialog.Root
        placement="center"
        open={open}
        role="alertdialog">
        <Portal>
            <Dialog.Backdrop/>
            <Dialog.Positioner>
                <Dialog.Content>
                    <Dialog.Header fontSize='lg' fontWeight='bold'>
                        {props.title}
                    </Dialog.Header>
                    <Dialog.Body>
                        {props.message}
                    </Dialog.Body>
                    <Dialog.Footer>
                        <Button background="red.600" onClick={onConfirmed} ml={3}>
                            {props.confirmActionName}
                        </Button>
                        <Dialog.ActionTrigger asChild>
                            <Button ref={cancelRef} onClick={onClose} variant="outline">{t("modals_cancel_button")}</Button>
                        </Dialog.ActionTrigger>
                    </Dialog.Footer>
                    <Dialog.CloseTrigger asChild>
                        <CloseButton onClick={onClose} size="sm" />
                    </Dialog.CloseTrigger>
                </Dialog.Content>
            </Dialog.Positioner>
        </Portal>
    </Dialog.Root>;
});