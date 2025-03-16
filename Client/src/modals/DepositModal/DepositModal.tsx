import "./DepositModal.scss"

import { forwardRef, useImperativeHandle } from 'react'
import DatePicker from "react-datepicker";
import { Button, Field, Input, Dialog, useDisclosure, Flex, Portal, CloseButton} from '@chakra-ui/react';
import { DepositEntity } from '../../models/DepositEntity';
import { zodResolver } from '@hookform/resolvers/zod';
import { Controller, useForm } from "react-hook-form";
import { DepositValidationSchema, DepositFormInput } from "./DepositValidationSchema";
import { useTranslation } from "react-i18next";

type Props = {
    deposit?: DepositEntity | null,
    onSaved: (deposit: DepositEntity) => void
}

export interface DepositModalRef {
    openModal: () => void
}

const DepositModal = forwardRef<DepositModalRef, Props>((props: Props, ref)=> {
    const { open, onOpen, onClose } = useDisclosure();
    useImperativeHandle(ref, () => ({
        openModal: onOpen,
    }));

    const { register, control, handleSubmit, formState: { errors }} = useForm<DepositFormInput>({
        resolver: zodResolver(DepositValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.deposit?.id ?? crypto.randomUUID(),
            name: props.deposit?.name ?? "",
            from: props.deposit?.from ?? new Date(),
            to: props.deposit?.to ?? new Date(),
            percentage: props.deposit?.percentage ?? 0,
            initialAmount: props.deposit?.initialAmount ?? 0,
        }
    });

    const onSubmit = (deposit: DepositFormInput) => {
        props.onSaved(deposit as DepositEntity);
        onClose();
    }

    const {t} = useTranslation()

    return <Dialog.Root placement="center" open={open}>
        <Portal>
            <Dialog.Backdrop/>
            <Dialog.Positioner>
                <Dialog.Content as="form" onSubmit={handleSubmit(onSubmit)}>
                    <Dialog.Header>
                        <Dialog.Title>
                        Deposit
                        </Dialog.Title>
                    </Dialog.Header>
                    <Dialog.Body pb={6}>
                        <Field.Root invalid={!!errors.name}>
                            <Field.Label>{t("entity_deposit_name")}</Field.Label>
                            <Input {...register("name")} autoComplete="off" placeholder='Deposit name' />
                            <Field.ErrorText>{errors.name?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root invalid={!!errors.percentage} mt={4}>
                            <Field.Label>{t("entity_deposit_percentage")}</Field.Label>
                            <Input {...register("percentage", { valueAsNumber: true })} type="number" placeholder='10' />
                            <Field.ErrorText>{errors.percentage?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root invalid={!!errors.initialAmount} mt={4}>
                            <Field.Label>{t("entity_deposit_initial_amount")}</Field.Label>
                            <Input {...register("initialAmount", { valueAsNumber: true })} type='number' placeholder='10' />
                            <Field.ErrorText>{errors.initialAmount?.message}</Field.ErrorText>
                        </Field.Root>
                        <Flex gap={4} direction="row">
                            <Field.Root invalid={!!errors.from} mt={4}>
                                <Field.Label>{t("entity_deposit_from")}</Field.Label>
                                <Controller
                                    name="from"
                                    control={control}
                                    render={({ field: {onChange, value} }) => (
                                        <DatePicker
                                        wrapperClassName="deposit-datepicker"
                                        selected={value}
                                        onChange={onChange}
                                        dateFormat="dd.MM.yyyy"
                                        placeholderText="Select from date"
                                        customInput={<Input/>}/>
                                    )}
                                    />
                                <Field.ErrorText>{errors.from?.message}</Field.ErrorText>
                            </Field.Root>
                            <Field.Root invalid={!!errors.to} mt={4}>
                                <Field.Label>{t("entity_deposit_to")}</Field.Label>
                                {/* TODO Fix date field duplication */}
                                <Controller
                                    name="to"
                                    control={control}
                                    render={({ field: {onChange, value} }) => (
                                        <DatePicker
                                        wrapperClassName="deposit-datepicker"
                                        selected={value}
                                        onChange={onChange}
                                        dateFormat="dd.MM.yyyy"
                                        placeholderText="Select to date"
                                        customInput={<Input/>}/>
                                    )}
                                    />
                                <Field.ErrorText>{errors.to?.message}</Field.ErrorText>
                            </Field.Root>
                        </Flex>
                    </Dialog.Body>
                    <Dialog.Footer>
                        <Button type="submit" backgroundColor='purple.600' mr={3}>Save</Button>
                        <Button onClick={onClose}>Cancel</Button>
                    </Dialog.Footer>
                    <Dialog.CloseTrigger asChild>
                        <CloseButton onClick={onClose} size="sm" />
                    </Dialog.CloseTrigger>
                </Dialog.Content>
            </Dialog.Positioner>
        </Portal>
    </Dialog.Root>
})


export default DepositModal;