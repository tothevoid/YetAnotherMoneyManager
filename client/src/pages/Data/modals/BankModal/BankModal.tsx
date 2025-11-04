import { Field, Input} from "@chakra-ui/react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import { RefObject, useEffect, useState } from "react";
import BaseFormModal from "../../../../shared/modals/BaseFormModal/BaseFormModal";
import { generateGuid } from "../../../../shared/utilities/idUtilities";
import { BankFormInput, BankValidationSchema } from "./BankValidationSchema";
import { BankEntity } from "../../../../models/banks/BankEntity";
import { Nullable } from "../../../../shared/utilities/nullable";
import { getBankIconUrl } from "../../../../api/banks/bankApi";
import InputImage from "../../../../shared/components/Form/InputImage/InputImage";


interface ModalProps {
    modalRef: RefObject<BaseModalRef | null>,
    bank?: BankEntity | null,
    onSaved: (account: BankEntity, icon: Nullable<File>) => void;
};

const BankModal: React.FC<ModalProps> = (props: ModalProps) => {
    const { register, handleSubmit, formState: { errors }, reset} = useForm<BankFormInput>({
        resolver: zodResolver(BankValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.bank?.id ?? generateGuid(),
            name: props.bank?.name ?? ""
        }
    });

    const [icon, setIcon] = useState<File | null>(null);
    const [iconUrl, setIconUrl] = useState<string | null>(null);

    useEffect(() => {
        if (props.bank) {
            reset(props.bank);
        }
    }, [props.bank]);

    useEffect(() => {
        const url = getBankIconUrl(props.bank?.iconKey);
        setIconUrl(url);
    }, [props.bank]);

    const onSubmit = (transactionType: BankFormInput) => {
        props.onSaved({...transactionType, iconKey: props.bank?.iconKey } as BankEntity, icon);
        props.modalRef?.current?.closeModal();
    }

    const onImageSelected = (url: string, image: File) => {
        setIcon(image);
        setIconUrl(url);
    }
    
    const {t} = useTranslation();

    return <BaseFormModal ref={props.modalRef} title={t("entity_bank_from_title")} submitHandler={handleSubmit(onSubmit)}>
        <InputImage imageUrl={iconUrl} onImageSelected={onImageSelected}/>
        <Field.Root invalid={!!errors.name}>
            <Field.Label>{t("entity_bank_name")}</Field.Label>
            <Input {...register("name")} autoComplete="off" placeholder='Sample bank' />
            <Field.ErrorText>{errors.name?.message}</Field.ErrorText>
        </Field.Root>
    </BaseFormModal>
}

export default BankModal;