import React, { RefObject } from 'react'
import { Field, Input} from '@chakra-ui/react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from "react-hook-form";
import { useTranslation } from "react-i18next";
import DateSelect from "../../../../shared/components/DateSelect/DateSelect";
import { DividendEntity } from '../../../../models/securities/DividendEntity';
import { DividendFormInput, DividendValidationSchema } from './DividendValidationSchema';
import { BaseModalRef } from '../../../../shared/utilities/modalUtilities';
import BaseFormModal from '../../../../shared/modals/BaseFormModal/BaseFormModal';

export interface CreateDividendContext {
	securityId: string
}

export interface EditDividendContext {
	dividend: DividendEntity
}

interface ModalProps {
	modalRef: RefObject<BaseModalRef | null>,
	onSaved: (dividend: DividendEntity) => void,
	context: CreateDividendContext | EditDividendContext
}

const DividendModal: React.FC<ModalProps> = (props: ModalProps) => {
	const {t} = useTranslation();

	const dividend = "dividend" in props.context ? props.context.dividend : null;

	const { register, control, handleSubmit, formState: { errors }} = useForm<DividendFormInput>({
		resolver: zodResolver(DividendValidationSchema),
		mode: "onBlur",
		defaultValues: {
			id: dividend?.id ?? crypto.randomUUID(),
			security: dividend?.security,
			amount: dividend?.amount ?? 0,
			declarationDate: dividend?.declarationDate ?? new Date(),
			snapshotDate: dividend?.snapshotDate ?? new Date()
		}
	});

	const onSubmit = (dividend: DividendFormInput) => {
		props.onSaved(dividend as DividendEntity);
		props.modalRef?.current?.closeModal();
	}

	return <BaseFormModal ref={props.modalRef} title={t("entity_dividend_form_title")} submitHandler={handleSubmit(onSubmit)}>
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
	</BaseFormModal>
}

export default DividendModal;