import { Field, Input} from "@chakra-ui/react"
import React, { RefObject } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import { BrokerAccountTypeFormInput, BrokerAccountTypeValidationSchema } from "./BrokerAccountTypeValidationSchema";
import { BrokerAccountTypeEntity } from "../../../../models/brokers/BrokerAccountTypeEntity";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import BaseFormModal from "../../../../shared/modals/BaseFormModal/BaseFormModal";

interface ModalProps {
	modalRef: RefObject<BaseModalRef | null>,
	brokerAccountType?: BrokerAccountTypeEntity | null,
	onSaved: (account: BrokerAccountTypeEntity) => void;
};

const BrokerAccountTypeTypeModal: React.FC<ModalProps> = (props: ModalProps) => {
	const { register, handleSubmit, formState: { errors }} = useForm<BrokerAccountTypeFormInput>({
		resolver: zodResolver(BrokerAccountTypeValidationSchema),
		mode: "onBlur",
		defaultValues: {
			id: props.brokerAccountType?.id ?? crypto.randomUUID(),
			name: props.brokerAccountType?.name ?? ""
		}
	});

	const onSubmit = (brokerAccountType: BrokerAccountTypeFormInput) => {
		props.onSaved(brokerAccountType as BrokerAccountTypeEntity);
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
export default BrokerAccountTypeTypeModal
