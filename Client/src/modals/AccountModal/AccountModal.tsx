import { Button, CloseButton, Dialog, Field, Input, Portal, useDisclosure} from "@chakra-ui/react"
import { forwardRef, useEffect, useImperativeHandle, useState } from "react"
import { AccountEntity } from "../../models/AccountEntity";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { AccountFormInput, AccountValidationSchema } from "./AccountValidationSchema";
import { useTranslation } from "react-i18next";
import { getCurrencies } from "../../api/currencyApi";
import { CurrencyEntity } from "../../models/CurrencyEntity";
import DateSelect from "../../controls/DateSelect/DateSelect";
import CollectionSelect from "../../controls/CollectionSelect/CollectionSelect";
import CheckboxInput from "../../controls/CheckboxInput/CheckboxInput";
import { getAccountTypes } from "../../api/accountTypeApi";
import { AccountTypeEntity } from "../../models/AccountTypeEntity";

type AccountProps = {
	account?: AccountEntity | null,
	onSaved: (account: AccountEntity) => void;
};

type State = {
	currencies: CurrencyEntity[]
	accountTypes: AccountTypeEntity[]
}

export interface AccountModalRef {
	openModal: () => void
}

const AccountModal = forwardRef<AccountModalRef, AccountProps>((props: AccountProps, ref)=> {
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

	const { open, onOpen, onClose } = useDisclosure()

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

	useImperativeHandle(ref, () => ({
		openModal: onOpen,
	}));


	const onSubmit = (account: AccountFormInput) => {
		props.onSaved(account as AccountEntity);
		onClose();
	}

	const {t} = useTranslation()

	return (
		<Dialog.Root placement="center" open={open} onEscapeKeyDown={onClose}>
		  <Portal>
			<Dialog.Backdrop/>
			<Dialog.Positioner>
				<Dialog.Content as="form" onSubmit={handleSubmit(onSubmit)}>
					<Dialog.Header>
						<Dialog.Title>{t("entity_account_name_form_title")}</Dialog.Title>
					</Dialog.Header>
					<Dialog.Body pb={6}>
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
					</Dialog.Body>
					<Dialog.Footer>
						<Button type="submit" background='purple.600' mr={3}>{t("modals_save_button")}</Button>
						<Button onClick={onClose}>{t("modals_cancel_button")}</Button>
					</Dialog.Footer>
					<Dialog.CloseTrigger asChild>
						<CloseButton onClick={onClose} size="sm" />
					</Dialog.CloseTrigger>
				</Dialog.Content>
			</Dialog.Positioner>
		  </Portal>
		</Dialog.Root>
	)
})
export default AccountModal