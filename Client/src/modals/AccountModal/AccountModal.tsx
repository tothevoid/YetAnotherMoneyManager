import { Field, Input} from "@chakra-ui/react"
import React, { RefObject, useEffect, useState } from "react"
import { AccountEntity } from "../../models/accounts/AccountEntity";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { AccountFormInput, AccountValidationSchema } from "./AccountValidationSchema";
import { useTranslation } from "react-i18next";
import { CurrencyEntity } from "../../models/currencies/CurrencyEntity";
import DateSelect from "../../controls/DateSelect/DateSelect";
import CollectionSelect from "../../controls/CollectionSelect/CollectionSelect";
import CheckboxInput from "../../controls/CheckboxInput/CheckboxInput";
import { getAccountTypes } from "../../api/accounts/accountTypeApi";
import { AccountTypeEntity } from "../../models/accounts/AccountTypeEntity";
import { getCurrencies } from "../../api/currencies/currencyApi";
import { BaseModalRef } from "../../common/ModalUtilities";
import BaseFormModal from "../../components/common/BaseFormModal";

interface ModalProps {
	modalRef: RefObject<BaseModalRef | null>,
	account?: AccountEntity | null,
	onSaved: (account: AccountEntity) => void;
};

interface State {
	currencies: CurrencyEntity[]
	accountTypes: AccountTypeEntity[]
}

const AccountModal: React.FC<ModalProps> = (props: ModalProps) => {
  	const [state, setState] = useState<State>({currencies: [], accountTypes: []})
	
	useEffect(() => {
		const initData = async () => {
			await initCurrencies();
			await initAccountTypes();
		}
		initData();
	}, []);

	const initCurrencies = async () => {
		const currencies = await getCurrencies();
		setState((currentState) => {
			return {...currentState, currencies}
		})
	};

	const initAccountTypes = async () => {
		const accountTypes = await getAccountTypes();
		setState((currentState) => {
			return {...currentState, accountTypes}
		})
	}

	const { register, handleSubmit, control, formState: { errors }} = useForm<AccountFormInput>({
		resolver: zodResolver(AccountValidationSchema),
		mode: "onBlur",
		defaultValues: {
			id: props.account?.id ?? crypto.randomUUID(),
			name: props.account?.name ?? "",
			balance: props.account?.balance ?? 0,
			currency: props.account?.currency,
			accountType: props.account?.accountType,
			active: props.account?.active ?? true,
			createdOn: props.account?.createdOn ?? new Date()
		}
	});

	const onSubmit = (account: AccountFormInput) => {
		props.onSaved(account as AccountEntity);
		props.modalRef?.current?.closeModal();
	}

	const {t} = useTranslation()

	return <BaseFormModal ref={props.modalRef} title={t("entity_account_name_form_title")} submitHandler={handleSubmit(onSubmit)}>
		<Field.Root invalid={!!errors.name}>
			<Field.Label>{t("entity_account_name")}</Field.Label>
			<Input {...register("name")} autoComplete="off" placeholder='Debit card' />
			<Field.ErrorText>{errors.name?.message}</Field.ErrorText>
		</Field.Root>
		<Field.Root mt={4} invalid={!!errors.accountType}>
			<Field.Label>{t("entity_transaction_type")}</Field.Label>
			<CollectionSelect name="accountType" control={control} placeholder="Select account type"
				collection={state.accountTypes} 
				labelSelector={(accountType => accountType.name)} 
				valueSelector={(accountType => accountType.id)}/>
			<Field.ErrorText>{errors.accountType?.message}</Field.ErrorText>
		</Field.Root>
		<Field.Root mt={4} invalid={!!errors.currency}>
			<Field.Label>{t("entity_transaction_currency")}</Field.Label>
			<CollectionSelect name="currency" control={control} placeholder="Select currency"
				collection={state.currencies} 
				labelSelector={(currency => currency.name)} 
				valueSelector={(currency => currency.id)}/>
			<Field.ErrorText>{errors.currency?.message}</Field.ErrorText>
		</Field.Root>
		<Field.Root invalid={!!errors.balance} mt={4}>
			<Field.Label>{t("entity_account_balance")}</Field.Label>
			<Input {...register("balance", { valueAsNumber: true })} name="balance" type="number" placeholder='10000' />
			<Field.ErrorText>{errors.balance?.message}</Field.ErrorText>
		</Field.Root>
		<Field.Root invalid={!!errors.createdOn} mt={4}>
			<Field.Label>{t("entity_account_created_on")}</Field.Label>
			<DateSelect name="createdOn" control={control}/>
			<Field.ErrorText>{errors.createdOn?.message}</Field.ErrorText>
		</Field.Root>
		<Field.Root invalid={!!errors.active} mt={4}>
			<CheckboxInput name="active" control={control} title={t("entity_account_active")}/>
			<Field.ErrorText>{errors.active?.message}</Field.ErrorText>
		</Field.Root>
	</BaseFormModal>
}

export default AccountModal