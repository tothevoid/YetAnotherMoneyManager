import { Button, CloseButton, Dialog, Portal, useDisclosure } from "@chakra-ui/react"
import React, { useImperativeHandle } from "react";
import { forwardRef } from "react"

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

    return <Dialog.Root
        placement="center"
        role="alertdialog"
        open={open}
        leastDestructiveRef={cancelRef}
        onClose={onClose}>
        {/* <AlertDialogOverlay>
        </AlertDialogOverlay> */}
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
                        <Button ref={cancelRef} onClick={onClose}>
                            Cancel
                        </Button>
                        <Button background="red.600" onClick={onConfirmed} ml={3}>
                            {props.confirmActionName}
                        </Button>
                    </Dialog.Footer>
                    <Dialog.CloseTrigger asChild>
                        <CloseButton size="sm" />
                    </Dialog.CloseTrigger>
                </Dialog.Content>
            </Dialog.Positioner>
        </Portal>
    </Dialog.Root>;
});