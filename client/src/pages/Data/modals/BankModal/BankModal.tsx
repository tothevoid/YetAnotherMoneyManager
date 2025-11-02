import { Field, Input} from "@chakra-ui/react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import { RefObject } from "react";
import BaseFormModal from "../../../../shared/modals/BaseFormModal/BaseFormModal";
import { generateGuid } from "../../../../shared/utilities/idUtilities";
import { BankFormInput, BankValidationSchema } from "./BankValidationSchema";
import { BankEntity } from "../../../../models/banks/BankEntity";

interface ModalProps {
    modalRef: RefObject<BaseModalRef | null>,
    bank?: BankEntity | null,
    onSaved: (account: BankEntity) => void;
};

const BankModal: React.FC<ModalProps> = (props: ModalProps) => {
    const { register, handleSubmit, formState: { errors }} = useForm<BankFormInput>({
        resolver: zodResolver(BankValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.bank?.id ?? generateGuid(),
            name: props.bank?.name ?? ""
        }
    });

    const onSubmit = (bank: BankFormInput) => {
        props.onSaved(bank as BankEntity);
        props.modalRef?.current?.closeModal();
    }
    const {t} = useTranslation();

    return <BaseFormModal ref={props.modalRef} title={t("entity_bank_from_title")} submitHandler={handleSubmit(onSubmit)}>
        <Field.Root invalid={!!errors.name}>
            <Field.Label>{t("entity_bank_name")}</Field.Label>
            <Input {...register("name")} autoComplete="off" placeholder='Sample bank' />
            <Field.ErrorText>{errors.name?.message}</Field.ErrorText>
        </Field.Root>
    </BaseFormModal>
}

export default BankModal;