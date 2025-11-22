import { Field, Input} from "@chakra-ui/react"
import React, { RefObject, useCallback, useEffect, useState } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import CollectionSelect from "../../../../shared/components/CollectionSelect/CollectionSelect";
import { DebtEntity } from "../../../../models/debts/DebtEntity";
import DateSelect from "../../../../shared/components/DateSelect/DateSelect";
import { DebtPaymentFormInput, DebtPaymentValidationSchema } from "./DebtPaymentValidationSchema";
import { DebtPaymentEntity } from "../../../../models/debts/DebtPaymentEntity";
import { AccountEntity } from "../../../../models/accounts/AccountEntity";
import { getDebts } from "../../../../api/debts/debtApi";
import { getAccounts } from "../../../../api/accounts/accountApi";
import BaseFormModal from "../../../../shared/modals/BaseFormModal/BaseFormModal";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import { generateGuid } from "../../../../shared/utilities/idUtilities";
import CheckboxInput from "../../../../shared/components/CheckboxInput/CheckboxInput";

interface Props {
	debtPayment?: DebtPaymentEntity | null,
	onSaved: (debt: DebtPaymentEntity) => void;
	modalRef: RefObject<BaseModalRef | null>
};
	
interface State {
	accounts: AccountEntity[],
	debts: DebtEntity[]
}

const DebtPaymentModal: React.FC<Props> = (props: Props) => {
	const [state, setState] = useState<State>({accounts: [], debts: []})

	useEffect(() => {
		const initData = async () => {
			await requestData();
		}
		initData();
	}, []);

	const requestData = async () => {
		const debts = await getDebts(true);
		const accounts = await getAccounts(true);

		setState((currentState) => {
			return {...currentState, debts, accounts }
		})
	};

	const getDefaultFormState = useCallback(() => {
		return {
			id: props.debtPayment?.id ?? generateGuid(),
			amount: props.debtPayment?.amount ?? 0,
			date: props.debtPayment?.date ?? new Date(),
			debt: props.debtPayment?.debt,
			targetAccount: props.debtPayment?.targetAccount,
			isPercentagePayment: props.debtPayment?.isPercentagePayment ?? false
		}
	}, [props.debtPayment]);

	const { register, handleSubmit, control, formState: { errors }, reset} = useForm<DebtPaymentFormInput>({
		resolver: zodResolver(DebtPaymentValidationSchema),
		mode: "onBlur",
		defaultValues: getDefaultFormState()
	});

	useEffect(() => {
		reset(getDefaultFormState());
	}, [reset, getDefaultFormState, props.debtPayment]);

	const onSubmit = (debt: DebtPaymentFormInput) => {
		props.onSaved(debt as DebtPaymentEntity);
		props.modalRef?.current?.closeModal();
	}

	const {t} = useTranslation()

	return <BaseFormModal ref={props.modalRef} title={t("entity_debt_payment_form_title")} submitHandler={handleSubmit(onSubmit)}>
		<Field.Root mt={4} invalid={!!errors.debt}>
			<Field.Label>{t("entity_debt_payment_debt")}</Field.Label>
			<CollectionSelect name="debt" control={control} placeholder="Select debt"
				collection={state.debts} 
				labelSelector={(debt => debt.name)} 
				valueSelector={(debt => debt.id)}/>
			<Field.ErrorText>{errors.debt?.message}</Field.ErrorText>
		</Field.Root>
		<Field.Root invalid={!!errors.amount}>
			<Field.Label>{t("entity_debt_payment_amount")}</Field.Label>
			<Input {...register("amount", {valueAsNumber: true})} min={0} step="0.01" autoComplete="off" type='number' placeholder='500' />
			<Field.ErrorText>{errors.amount?.message}</Field.ErrorText>
		</Field.Root>
			<Field.Root mt={4} invalid={!!errors.targetAccount}>
			<Field.Label>{t("entity_debt_payment_target_account")}</Field.Label>
			<CollectionSelect name="targetAccount" control={control} placeholder="Select account"
				collection={state.accounts} 
				labelSelector={(account => account.name)} 
				valueSelector={(account => account.id)}/>
			<Field.ErrorText>{errors.targetAccount?.message}</Field.ErrorText>
		</Field.Root>		
		<Field.Root mt={4} invalid={!!errors.date}>
			<Field.Label>{t("entity_debt_payment_date")}</Field.Label>
			<DateSelect name="date" control={control}/>
			<Field.ErrorText>{errors.date?.message}</Field.ErrorText>
		</Field.Root>
		<Field.Root invalid={!!errors.isPercentagePayment} mt={4}>
			<CheckboxInput name="isPercentagePayment" control={control} title={t("entity_debt_payment_is_percentage_payment")}/>
			<Field.ErrorText>{errors.isPercentagePayment?.message}</Field.ErrorText>
		</Field.Root>
	</BaseFormModal>
}

export default DebtPaymentModal;