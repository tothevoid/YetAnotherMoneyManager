import { forwardRef, useImperativeHandle } from 'react'
import { Button, Field, Dialog, useDisclosure, Portal, CloseButton, Input} from '@chakra-ui/react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from "react-hook-form";
import { useTranslation } from "react-i18next";
import DateSelect from "../../controls/DateSelect/DateSelect";
import { DividendEntity } from '../../models/securities/DividendEntity';
import { DividendFormInput, DividendValidationSchema } from './DividendValidationSchema';

type Props = {
    dividend?: DividendEntity | null,
    onSaved: (dividend: DividendEntity) => void
}

export interface DividendModalRef {
    openModal: () => void
}

const DividendModal = forwardRef<DividendModalRef, Props>((props: Props, ref)=> {
    const { open, onOpen, onClose } = useDisclosure();
    useImperativeHandle(ref, () => ({
        openModal: onOpen,
    }));

    const { register, control, handleSubmit, formState: { errors }} = useForm<DividendFormInput>({
        resolver: zodResolver(DividendValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.dividend?.id ?? crypto.randomUUID(),
            security: props.dividend?.security,
            amount: props.dividend?.amount ?? 0,
            declarationDate: props.dividend?.declarationDate ?? new Date(),
            paymentDate: props.dividend?.paymentDate ?? new Date(),
            snapshotDate: props.dividend?.snapshotDate ?? new Date()
        }
    });

    const onSubmit = (dividend: DividendFormInput) => {
        props.onSaved(dividend as DividendEntity);
        onClose();
    }

    const {t} = useTranslation()

    return <Dialog.Root placement="center" open={open} onEscapeKeyDown={onClose}>
        <Portal>
            <Dialog.Backdrop/>
            <Dialog.Positioner>
                <Dialog.Content as="form" onSubmit={handleSubmit(onSubmit)}>
                    <Dialog.Header>
                        <Dialog.Title>
                            {t("entity_dividend_form_title")}
                        </Dialog.Title>
                    </Dialog.Header>
                    <Dialog.Body pb={6}>
                        <Field.Root invalid={!!errors.amount} mt={4}>
                            <Field.Label>{t("entity_dividend_security_amount")}</Field.Label>
                            <Input {...register("amount", { valueAsNumber: true })} type="number" step="0.01" placeholder='10' />
                            <Field.ErrorText>{errors.amount?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root invalid={!!errors.declarationDate} mt={4}>
                            <Field.Label>{t("entity_dividend_declaration_date")}</Field.Label>
                            <DateSelect name="declarationDate" control={control}/>
                            <Field.ErrorText>{errors.declarationDate?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root invalid={!!errors.snapshotDate} mt={4}>
                            <Field.Label>{t("entity_dividend_snapshot_date")}</Field.Label>
                            <DateSelect name="snapshotDate" control={control}/>
                            <Field.ErrorText>{errors.snapshotDate?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root invalid={!!errors.paymentDate} mt={4}>
                            <Field.Label>{t("entity_dividend_payment_date")}</Field.Label>
                            <DateSelect name="paymentDate" control={control}/>
                            <Field.ErrorText>{errors.paymentDate?.message}</Field.ErrorText>
                        </Field.Root>
                    </Dialog.Body>
                    <Dialog.Footer>
                        <Button type="submit" backgroundColor='purple.600' mr={3}>{t("modals_save_button")}</Button>
                        <Button onClick={onClose}>{t("modals_cancel_button")}</Button>
                    </Dialog.Footer>
                    <Dialog.CloseTrigger asChild>
                        <CloseButton onClick={onClose} size="sm" />
                    </Dialog.CloseTrigger>
                </Dialog.Content>
            </Dialog.Positioner>
        </Portal>
    </Dialog.Root>
})


export default DividendModal;