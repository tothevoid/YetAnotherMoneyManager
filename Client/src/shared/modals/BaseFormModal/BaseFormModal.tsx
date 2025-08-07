import { Button, CloseButton, Dialog, Portal, useDisclosure} from "@chakra-ui/react"
import { FormEventHandler, forwardRef, useEffect, useImperativeHandle } from "react"
import { useTranslation } from "react-i18next";
import { BaseModalRef } from "../../utilities/modalUtilities";

interface BaseFormModalProps {
    title: string,
    submitHandler: FormEventHandler,
    children: React.ReactNode,
    visibilityChanged?: (open: boolean) => void
};

const BaseFormModal = forwardRef<BaseModalRef, BaseFormModalProps>((props: BaseFormModalProps, ref) => {
    const { open, onOpen, onClose } = useDisclosure()

    useImperativeHandle(ref, () => ({
        openModal: onOpen,
        closeModal: onClose
    }));

    useEffect(() => {
        if (!props.visibilityChanged) {
            return;
        }
        props.visibilityChanged(open);
    }, [open])

    const { t } = useTranslation();

    return (
        <Dialog.Root placement="center" open={open} onEscapeKeyDown={onClose}>
          <Portal>
            <Dialog.Backdrop/>
            <Dialog.Positioner>
                <Dialog.Content as="form" onSubmit={props.submitHandler}>
                    <Dialog.Header>
                        <Dialog.Title>{props.title}</Dialog.Title>
                    </Dialog.Header>
                    <Dialog.Body pb={6}>
                        {props.children}
                    </Dialog.Body>
                    <Dialog.Footer>
                        <Button type="submit" background='purple.600' mr={3}>{t("modals_save_button")}</Button>
                        <Button onClick={onClose}>{t("modals_cancel_button")}</Button>
                    </Dialog.Footer>
                    <Dialog.CloseTrigger asChild>
                        <CloseButton onClick={onClose} size="sm" />
                    </Dialog.CloseTrigger>
                </Dialog.Content>
            </Dialog.Positioner>
          </Portal>
        </Dialog.Root>
    )
})
export default BaseFormModal;