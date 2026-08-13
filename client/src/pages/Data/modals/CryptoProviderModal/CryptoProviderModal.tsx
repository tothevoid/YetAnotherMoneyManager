import { Field, Input} from "@chakra-ui/react"
import React, { RefObject, useMemo } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import BaseFormModal from "../../../../shared/modals/BaseFormModal/BaseFormModal";
import { CryptoProviderEntity } from "../../../../models/crypto/CryptoProviderEntity";
import { CryptoProviderFormInput, getCryptoProviderValidationSchema } from "./CryptoProviderValidationSchema";
import { generateGuid } from "../../../../shared/utilities/idUtilities";

interface ModalProps {
    modalRef: RefObject<BaseModalRef | null>,
    cryptoProvider?: CryptoProviderEntity | null,
    onSaved: (cryptoProvider: CryptoProviderEntity) => void;
};

const CryptoProviderModal: React.FC<ModalProps> = (props: ModalProps) => {
    const { t } = useTranslation();
    const validationSchema = useMemo(() => getCryptoProviderValidationSchema(t), [t]);

    const { register, handleSubmit, formState: { errors }} = useForm<CryptoProviderFormInput>({
        resolver: zodResolver(validationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.cryptoProvider?.id ?? generateGuid(),
            name: props.cryptoProvider?.name ?? ""
        }
    });

    const onSubmit = (cryptoProvider: CryptoProviderFormInput) => {
        props.onSaved(cryptoProvider as CryptoProviderEntity);
        props.modalRef?.current?.closeModal();
    }

    return <BaseFormModal ref={props.modalRef} title={t("entity_crypto_provider_form_title")} submitHandler={handleSubmit(onSubmit)}>
        <Field.Root invalid={!!errors.name}>
            <Field.Label>{t("entity_crypto_provider_name")}</Field.Label>
            <Input {...register("name")} autoComplete="off" placeholder="Binance" />
            <Field.ErrorText>{errors.name?.message}</Field.ErrorText>
        </Field.Root>
    </BaseFormModal>
}

export default CryptoProviderModal;
