import { Field, Input} from "@chakra-ui/react"
import React, { RefObject } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import BaseFormModal from "../../../../shared/modals/BaseFormModal/BaseFormModal";
import { CryptoProviderEntity } from "../../../../models/crypto/CryptoProviderEntity";
import { CryptoProviderFormInput, CryptoProviderValidationSchema } from "./CryptoProviderValidationSchema";

interface ModalProps {
    modalRef: RefObject<BaseModalRef | null>,
    cryptoProvider?: CryptoProviderEntity | null,
    onSaved: (cryptoProvider: CryptoProviderEntity) => void;
};

const CryptoProviderModal: React.FC<ModalProps> = (props: ModalProps) => {
    const { register, handleSubmit, formState: { errors }} = useForm<CryptoProviderFormInput>({
        resolver: zodResolver(CryptoProviderValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.cryptoProvider?.id ?? crypto.randomUUID(),
            name: props.cryptoProvider?.name ?? ""
        }
    });

    const onSubmit = (cryptoProvider: CryptoProviderFormInput) => {
        props.onSaved(cryptoProvider as CryptoProviderEntity);
        props.modalRef?.current?.closeModal();
    }

    const {t} = useTranslation()

    return <BaseFormModal ref={props.modalRef} title={t("entity_broker_account_type_from_title")} submitHandler={handleSubmit(onSubmit)}>
        <Field.Root invalid={!!errors.name}>
            <Field.Label>{t("entity_broker_account_type_name")}</Field.Label>
            <Input {...register("name")} autoComplete="off" placeholder='Debit card' />
            <Field.ErrorText>{errors.name?.message}</Field.ErrorText>
        </Field.Root>
    </BaseFormModal>
    
}
export default CryptoProviderModal
