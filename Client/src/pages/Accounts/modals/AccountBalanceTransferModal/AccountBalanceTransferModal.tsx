import { Field, Input} from "@chakra-ui/react"
import React, { RefObject, useEffect, useState } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { AccountBalanceTransferFormInput, AccountBalanceTransferModalValidationSchema } from "./AccountBalanceTransferModalValidationSchema";
import { useTranslation } from "react-i18next";
import { getAccounts, transferBalance } from "../../../../api/accounts/accountApi";
import { AccountEntity } from "../../../../models/accounts/AccountEntity";
import CollectionSelect from "../../../../shared/components/CollectionSelect/CollectionSelect";
import BaseFormModal from "../../../../shared/modals/BaseFormModal/BaseFormModal";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";

interface ModalProps {
	modalRef: RefObject<BaseModalRef | null>,
	from?: AccountEntity | null,
	onTransfered: () => void;
};

type State = {
	accounts: AccountEntity[]
}

export type Transfer = {
	from: AccountEntity,
	to: AccountEntity,
	balance: number
	fee: number
}

const AccountBalanceTransferModal: React.FC<ModalProps> = (props: ModalProps) => {
	const [state, setState] = useState<State>({accounts: []})
	
	useEffect(() => {
		const initData = async () => {
			await initAccounts();
		}
		initData();
	}, []);

	const initAccounts = async () => {
		const accounts = await getAccounts();
		setState((currentState) => {
			return {...currentState, accounts}
		})
	};

	const { register, handleSubmit, control, formState: { errors }} = useForm<AccountBalanceTransferFormInput>({
		resolver: zodResolver(AccountBalanceTransferModalValidationSchema),
		mode: "onBlur",
		defaultValues: {
			from: props.from,
			to: null,
			balance: 0,
			fee: 0
		}
	});


	const onSubmit = async (transfer: AccountBalanceTransferFormInput) => {
		const isTransferred = await transferBalance(transfer as Transfer)

		if (!isTransferred) {
			return;
		}
		
		props.onTransfered();
		props.modalRef?.current?.closeModal();
	}

	const {t} = useTranslation()

	return <BaseFormModal ref={props.modalRef} title={t("account_balance_transfer_modal_title")} submitHandler={handleSubmit(onSubmit)}>
		<Field.Root mt={4} invalid={!!errors.from}>
			<Field.Label>{t("account_balance_transfer_modal_from")}</Field.Label>
			<CollectionSelect name="from" control={control} placeholder="Select sender account"
				collection={state.accounts} 
				labelSelector={(account => account.name)} 
				valueSelector={(account => account.id)}/>
			<Field.ErrorText>{errors.from?.message}</Field.ErrorText>
		</Field.Root>
		<Field.Root mt={4} invalid={!!errors.to}>
			<Field.Label>{t("account_balance_transfer_modal_to")}</Field.Label>
			<CollectionSelect name="to" control={control} placeholder="Select recepient account"
				collection={state.accounts} 
				labelSelector={(account => account.name)} 
				valueSelector={(account => account.id)}/>
			<Field.ErrorText>{errors.to?.message}</Field.ErrorText>
		</Field.Root>
		<Field.Root invalid={!!errors.balance} mt={4}>
			<Field.Label>{t("account_balance_transfer_modal_balance")}</Field.Label>
			<Input {...register("balance", { valueAsNumber: true })} name="balance" type="number" placeholder='10000' />
			<Field.ErrorText>{errors.balance?.message}</Field.ErrorText>
		</Field.Root>
		<Field.Root invalid={!!errors.fee} mt={4}>
			<Field.Label>{t("account_balance_transfer_modal_fee")}</Field.Label>
			<Input {...register("fee", { valueAsNumber: true })} name="fee" type="number" placeholder='0' />
			<Field.ErrorText>{errors.fee?.message}</Field.ErrorText>
		</Field.Root>
	</BaseFormModal>
}

export default AccountBalanceTransferModal