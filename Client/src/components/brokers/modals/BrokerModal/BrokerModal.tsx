import { Field, Input} from "@chakra-ui/react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import { BrokerAccountTypeEntity } from "../../../../models/brokers/BrokerAccountTypeEntity";
import { BrokerFormInput, BrokerValidationSchema } from "./BrokerValidationSchema";
import { BrokerEntity } from "../../../../models/brokers/BrokerEntity";
import { BaseModalRef } from "../../../../common/ModalUtilities";
import { RefObject } from "react";
import BaseFormModal from "../../../common/BaseFormModal";

interface ModalProps {
    modalRef: RefObject<BaseModalRef | null>,
    brokerAccountType?: BrokerAccountTypeEntity | null,
    onSaved: (account: BrokerAccountTypeEntity) => void;
};

const BrokerModal: React.FC<ModalProps> = (props: ModalProps) => {
    const { register, handleSubmit, formState: { errors }} = useForm<BrokerFormInput>({
        resolver: zodResolver(BrokerValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.brokerAccountType?.id ?? crypto.randomUUID(),
            name: props.brokerAccountType?.name ?? ""
        }
    });

    const onSubmit = (broker: BrokerFormInput) => {
        props.onSaved(broker as BrokerEntity);
        props.modalRef?.current?.closeModal();
    }

    const {t} = useTranslation();

    return <BaseFormModal ref={props.modalRef} title={t("entity_broker_from_title")} submitHandler={handleSubmit(onSubmit)}>
        <Field.Root invalid={!!errors.name}>
            <Field.Label>{t("entity_broker_name")}</Field.Label>
            <Input {...register("name")} autoComplete="off" placeholder='Debit card' />
            <Field.ErrorText>{errors.name?.message}</Field.ErrorText>
        </Field.Root>
    </BaseFormModal>
}

export default BrokerModal;