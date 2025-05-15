import { Button, CloseButton, Dialog, Field, Input, Portal, useDisclosure} from "@chakra-ui/react"
import { forwardRef, useImperativeHandle } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import CheckboxInput from "../../controls/CheckboxInput/CheckboxInput";
import { TransactionTypeFormInput, TransactionTypeValidationSchema } from "./TransactionTypeValidationSchema";
import { TransactionTypeEntity } from "../../models/transactions/TransactionTypeEntity";

type Props = {
    transactionType?: TransactionTypeEntity | null,
    onSaved: (account: TransactionTypeEntity) => void;
};

export interface TransactionTypeModalRef {
    openModal: () => void
}

const TransactionTypeModal = forwardRef<TransactionTypeModalRef, Props>((props: Props, ref)=> {
    const { open, onOpen, onClose } = useDisclosure()

    const { register, handleSubmit, control, formState: { errors }} = useForm<TransactionTypeFormInput>({
        resolver: zodResolver(TransactionTypeValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.transactionType?.id ?? crypto.randomUUID(),
            name: props.transactionType?.name ?? "",
            active: props.transactionType?.active ?? true,
        }
    });

    useImperativeHandle(ref, () => ({
        openModal: onOpen,
    }));


    const onSubmit = (transactionType: TransactionTypeFormInput) => {
        props.onSaved(transactionType as TransactionTypeFormInput);
        onClose();
    }

    const {t} = useTranslation()

    return (
        <Dialog.Root placement="center" open={open} onEscapeKeyDown={onClose}>
          <Portal>
            <Dialog.Backdrop/>
            <Dialog.Positioner>
                <Dialog.Content as="form" onSubmit={handleSubmit(onSubmit)}>
                    <Dialog.Header>
                        <Dialog.Title>{t("entity_transaction_type_name_form_title")}</Dialog.Title>
                    </Dialog.Header>
                    <Dialog.Body pb={6}>
                        <Field.Root invalid={!!errors.name}>
                            <Field.Label>{t("entity_transaction_type_name")}</Field.Label>
                            <Input {...register("name")} autoComplete="off" placeholder='USD' />
                            <Field.ErrorText>{errors.name?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root invalid={!!errors.active} mt={4}>
                            <CheckboxInput name="active" control={control} title={t("entity_transaction_type_active")}/>
                            <Field.ErrorText>{errors.active?.message}</Field.ErrorText>
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
export default TransactionTypeModal