import { AlertDialog, AlertDialogBody, AlertDialogContent, AlertDialogFooter, 
    AlertDialogHeader, AlertDialogOverlay, Button, useDisclosure } from "@chakra-ui/react"
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
    const { isOpen, onOpen, onClose } = useDisclosure();

    const cancelRef = React.useRef<HTMLButtonElement>(null!);

    useImperativeHandle(ref, () => ({
        openModal: onOpen,
    }));

    const onConfirmed = () => {
        onClose();
        props.onConfirmed();
    }

    return <AlertDialog
        isOpen={isOpen}
        leastDestructiveRef={cancelRef}
        onClose={onClose}>
        <AlertDialogOverlay>
            <AlertDialogContent>
                <AlertDialogHeader fontSize='lg' fontWeight='bold'>
                   {props.title}
                </AlertDialogHeader>
                <AlertDialogBody>
                    {props.message}
                </AlertDialogBody>

                <AlertDialogFooter>
                <Button ref={cancelRef} onClick={onClose}>
                    Cancel
                </Button>
                <Button colorScheme='red' onClick={onConfirmed} ml={3}>
                    {props.confirmActionName}
                </Button>
                </AlertDialogFooter>
            </AlertDialogContent>
        </AlertDialogOverlay>
    </AlertDialog>;
});