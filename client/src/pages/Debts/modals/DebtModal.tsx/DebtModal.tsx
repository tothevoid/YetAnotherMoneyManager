import { Field, Input } from "@chakra-ui/react";
import React, { RefObject, useCallback, useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import CollectionSelect from "../../../../shared/components/CollectionSelect/CollectionSelect";
import { getCurrencies } from "../../../../api/currencies/currencyApi";
import { getUserProfile } from "../../../../api/user/userProfileApi";
import { CurrencyEntity } from "../../../../models/currencies/CurrencyEntity";
import { DebtFormInput, DebtValidationSchema } from "./DebtValidationSchema";
import { DebtEntity } from "../../../../models/debts/DebtEntity";
import DateSelect from "../../../../shared/components/DateSelect/DateSelect";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import BaseFormModal from "../../../../shared/modals/BaseFormModal/BaseFormModal";
import { generateGuid } from "../../../../shared/utilities/idUtilities";

interface Props {
	debt?: DebtEntity | null,
	modalRef: RefObject<BaseModalRef | null>,
	onSaved: (debt: DebtEntity) => void
};

interface State {
	currencies: CurrencyEntity[],
	profileCurrency?: CurrencyEntity
}

const DebtModal: React.FC<Props> = (props: Props) => {
	const [state, setState] = useState<State>({ currencies: [] });

	const loadData = useCallback(async () => {
		const [currencies, userProfile] = await Promise.all([
			getCurrencies(),
			getUserProfile()
		]);
		const profileCurrency = userProfile ? userProfile.currency : currencies[0];
		setState({ currencies, profileCurrency });
	}, []);

	useEffect(() => {
		loadData();
	}, [loadData]);

	const getDefaultFormState = useCallback(() => {
		return {
			id: props.debt?.id ?? generateGuid(),
			name: props.debt?.name ?? "",
			amount: props.debt?.amount ?? 0,
			currency: props.debt?.currency ?? state.profileCurrency ?? state.currencies[0],
			date: props.debt?.date ?? new Date()
		}
	}, [props.debt, state.profileCurrency, state.currencies]);

	const { register, handleSubmit, control, formState: { errors }, reset } = useForm<DebtFormInput>({
		resolver: zodResolver(DebtValidationSchema),
		mode: "onBlur",
		defaultValues: getDefaultFormState()
	});

	useEffect(() => {
		reset(getDefaultFormState());
	}, [reset, getDefaultFormState]);

	const onSubmit = (debt: DebtFormInput) => {
		props.onSaved(debt as DebtEntity);
		props.modalRef.current?.closeModal();
	}

	const { t } = useTranslation();

	return (
		<BaseFormModal ref={props.modalRef} title={t("entity_debt_form_title")} submitHandler={handleSubmit(onSubmit)}>
			<Field.Root invalid={!!errors.name}>
				<Field.Label>{t("entity_debt_name")}</Field.Label>
				<Input {...register("name")} autoComplete="off" placeholder='Debit card' />
				<Field.ErrorText>{errors.name?.message}</Field.ErrorText>
			</Field.Root>
			<Field.Root invalid={!!errors.amount}>
				<Field.Label>{t("entity_debt_amount")}</Field.Label>
				<Input {...register("amount", { valueAsNumber: true })} min={0} step="0.01" autoComplete="off" type='number' placeholder='500' />
				<Field.ErrorText>{errors.amount?.message}</Field.ErrorText>
			</Field.Root>
			<Field.Root mt={4} invalid={!!errors.currency}>
				<Field.Label>{t("entity_debt_currency")}</Field.Label>
				<CollectionSelect name="currency" control={control} placeholder="Select type"
					collection={state.currencies} 
					labelSelector={(currency => currency.name)} 
					valueSelector={(currency => currency.id)} />
				<Field.ErrorText>{errors.currency?.message}</Field.ErrorText>
			</Field.Root>
			<Field.Root mt={4} invalid={!!errors.date}>
				<Field.Label>{t("entity_debt_date")}</Field.Label>
				<DateSelect name="date" control={control} />
				<Field.ErrorText>{errors.date?.message}</Field.ErrorText>
			</Field.Root>
		</BaseFormModal>
	);
}

export default DebtModal;