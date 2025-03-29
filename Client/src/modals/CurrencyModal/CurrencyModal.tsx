import { Button, CloseButton, Dialog, Field, Input, Portal, useDisclosure} from "@chakra-ui/react"
import { forwardRef, useImperativeHandle } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { CurrencyValidationSchema, CurrencyFormInput } from "./CurrencyValidationSchema";
import { useTranslation } from "react-i18next";
import { CurrencyEntity } from "../../models/CurrencyEntity";
import CheckboxInput from "../../controls/CheckboxInput/CheckboxInput";

type Props = {
    currency?: CurrencyEntity | null,
    onSaved: (account: CurrencyEntity) => void;
};

export interface CurrencyModalRef {
    openModal: () => void
}

const CurrencyModal = forwardRef<CurrencyModalRef, Props>((props: Props, ref)=> {
    const { open, onOpen, onClose } = useDisclosure()

    const { register, handleSubmit, control, formState: { errors }} = useForm<CurrencyFormInput>({
        resolver: zodResolver(CurrencyValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.currency?.id ?? crypto.randomUUID(),
            name: props.currency?.name ?? "",
            active: props.currency?.active ?? true,
        }
    });

    useImperativeHandle(ref, () => ({
        openModal: onOpen,
    }));


    const onSubmit = (currency: CurrencyFormInput) => {
        props.onSaved(currency as CurrencyEntity);
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
                        <Dialog.Title>{t("entity_currency_name_form_title")}</Dialog.Title>
                    </Dialog.Header>
                    <Dialog.Body pb={6}>
                        <Field.Root invalid={!!errors.name}>
                            <Field.Label>{t("entity_currency_name")}</Field.Label>
                            <Input {...register("name")} autoComplete="off" placeholder='USD' />
                            <Field.ErrorText>{errors.name?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root invalid={!!errors.active} mt={4}>
                            <CheckboxInput name="active" control={control} title={t("entity_currency_active")}/>
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
export default CurrencyModal