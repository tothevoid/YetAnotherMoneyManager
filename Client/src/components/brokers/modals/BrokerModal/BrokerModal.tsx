import { Button, CloseButton, Dialog, Field, Input, Portal, useDisclosure} from "@chakra-ui/react"
import { forwardRef, useImperativeHandle } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import { BrokerAccountTypeEntity } from "../../../../models/brokers/BrokerAccountTypeEntity";
import { BrokerFormInput, BrokerValidationSchema } from "./BrokerValidationSchema";
import { BrokerEntity } from "../../../../models/brokers/BrokerEntity";

interface BrokerModalProps {
    brokerAccountType?: BrokerAccountTypeEntity | null,
    onSaved: (account: BrokerAccountTypeEntity) => void;
};

export interface BrokerModalRef {
    openModal: () => void
}

const BrokerModal = forwardRef<BrokerModalRef, BrokerModalProps>((props: BrokerModalProps, ref)=> {
    const { open, onOpen, onClose } = useDisclosure()

    const { register, handleSubmit, formState: { errors }} = useForm<BrokerFormInput>({
        resolver: zodResolver(BrokerValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.brokerAccountType?.id ?? crypto.randomUUID(),
            name: props.brokerAccountType?.name ?? ""
        }
    });

    useImperativeHandle(ref, () => ({
        openModal: onOpen,
    }));

    const onSubmit = (broker: BrokerFormInput) => {
        props.onSaved(broker as BrokerEntity);
        onClose();
    }

    const {t} = useTranslation();

    return (
        <Dialog.Root placement="center" open={open} onEscapeKeyDown={onClose}>
          <Portal>
            <Dialog.Backdrop/>
            <Dialog.Positioner>
                <Dialog.Content as="form" onSubmit={handleSubmit(onSubmit)}>
                    <Dialog.Header>
                        <Dialog.Title>{t("entity_broker_from_title")}</Dialog.Title>
                    </Dialog.Header>
                    <Dialog.Body pb={6}>
                        <Field.Root invalid={!!errors.name}>
                            <Field.Label>{t("entity_broker_name")}</Field.Label>
                            <Input {...register("name")} autoComplete="off" placeholder='Debit card' />
                            <Field.ErrorText>{errors.name?.message}</Field.ErrorText>
                        </Field.Root>
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
export default BrokerModal;