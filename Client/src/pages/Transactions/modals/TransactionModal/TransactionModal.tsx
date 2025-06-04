import React, { Fragment, RefObject, useEffect, useState } from 'react'
import { Field, Input} from '@chakra-ui/react';
import { zodResolver } from '@hookform/resolvers/zod';
import { TransactionFormInput, TransactionValidationSchema } from './TransactionValidationSchema';
import { useForm } from 'react-hook-form';
import { useTranslation } from 'react-i18next';
import { getTransactionTypes } from '../../../../api/transactions/transactionTypeApi';
import { AccountEntity } from '../../../../models/accounts/AccountEntity';
import { TransactionEntity } from '../../../../models/transactions/TransactionEntity';
import { TransactionTypeEntity } from '../../../../models/transactions/TransactionTypeEntity';
import CollectionSelect from '../../../../shared/components/CollectionSelect/CollectionSelect';
import DateSelect from '../../../../shared/components/DateSelect/DateSelect';
import BaseFormModal from '../../../../shared/modals/BaseFormModal/BaseFormModal';
import { BaseModalRef } from '../../../../shared/utilities/modalUtilities';

interface ModalProps {
	modalRef: RefObject<BaseModalRef | null>,
	transaction?: TransactionEntity | null,
	accounts: AccountEntity[],
	onSaved: (transaction: TransactionEntity) => void
}

interface State {
	transactionTypes: TransactionTypeEntity[]
}

enum TransactionDirection {
	Income = "income",
	Spent = "spent",
}

const TransactionModal: React.FC<ModalProps> = (props: ModalProps) => {
	const {t} = useTranslation();

	const [state, setState] = useState<State>({transactionTypes: []});

	const initTransactionTypes = async () => {
		const transactionTypes = await getTransactionTypes(true);
		setState((currentState) => {
			return {...currentState, transactionTypes}
		})
	};
	
	useEffect(() => {
		const initData = async () => {
			await initTransactionTypes();
		}
		initData();
	}, []);

	const transactionOptions = {
		[TransactionDirection.Income]: { label: t("entity_transaction_direction_income"), value: TransactionDirection.Income },
		[TransactionDirection.Spent]: { label: t("entity_transaction_direction_outcome"), value: TransactionDirection.Spent },
	} as const;

	const amount = props.transaction?.amount ? 
		Math.abs(props.transaction.amount) :
		0;

	const direction = props.transaction && props.transaction.amount > 0 ?
		transactionOptions[TransactionDirection.Income]:
		transactionOptions[TransactionDirection.Spent];

	const source = (props.accounts?.length > 0) ? 
		props.accounts[0] :
		{id: ""} as AccountEntity

	const { register, handleSubmit, watch, control, formState: { errors }} = useForm<TransactionFormInput>({
		resolver: zodResolver(TransactionValidationSchema),
		mode: "onBlur",
		defaultValues: {
			id: props.transaction?.id ?? crypto.randomUUID(),
			name: props.transaction?.name ?? "",
			date: props.transaction?.date ?? new Date(),
			amount,
			account: props.transaction?.account ?? source,
			direction: direction,
			cashback: props.transaction?.cashback ?? 0,
			isSystem: props.transaction?.isSystem ?? false,
			transactionType: props.transaction?.transactionType ?? ""
		}
	});

	const onTransactionSaveClick = (transaction: TransactionFormInput) => {
		const multiplier = transaction.direction.value == TransactionDirection.Income ?
			1:
			-1;

		const formData = transaction as TransactionEntity;
		formData.amount = multiplier * formData.amount;
		props.onSaved(formData);
		props.modalRef?.current?.closeModal();
	};

	const selectedDirection = watch("direction");

	return <BaseFormModal ref={props.modalRef} title={t("entity_transaction_name_form_title")} submitHandler={handleSubmit(onTransactionSaveClick)}>
		<Field.Root invalid={!!errors.name}>
			<Field.Label>{t("entity_transaction_name")}</Field.Label>
			<Input {...register("name")} autoComplete="off" placeholder='Grocery' />
		</Field.Root>
		<Field.Root mt={4}>
			<Field.Label>{t("entity_transaction_direction")}</Field.Label>
			<CollectionSelect name="direction" control={control} placeholder="Select direction"
				collection={Object.values(transactionOptions)} 
				labelSelector={(currency => currency.label)} 
				valueSelector={(currency => currency.value)}/>
		</Field.Root>
		<Field.Root mt={4} invalid={!!errors.amount}>
			<Field.Label>{t("entity_transaction_money_quantity")}</Field.Label>
			<Input {...register("amount", {valueAsNumber: true})} min={0} autoComplete="off" type='number' placeholder='500' />
			<Field.ErrorText>{errors.amount?.message}</Field.ErrorText>
		</Field.Root>
		{
			selectedDirection.value === TransactionDirection.Spent ?
				<Field.Root mt={4} invalid={!!errors.cashback}>
					<Field.Label>{t("entity_transaction_cashback")}</Field.Label>
					<Input {...register("cashback", {valueAsNumber: true})} min={0} autoComplete="off" type='number' placeholder='100' />
					<Field.ErrorText>{errors.cashback?.message}</Field.ErrorText>
				</Field.Root>:
				<Fragment/>
		}
		<Field.Root mt={4} invalid={!!errors.date}>
			<Field.Label>{t("entity_transaction_date")}</Field.Label>
			<DateSelect name="date" control={control}/>
			<Field.ErrorText>{errors.date?.message}</Field.ErrorText>
		</Field.Root>
		<Field.Root mt={4} invalid={!!errors.account}>
			<Field.Label>{t("entity_transaction_account_source")}</Field.Label>
			<CollectionSelect name="account" control={control} placeholder="Select account"
				collection={props.accounts} 
				labelSelector={(currency => currency.name)} 
				valueSelector={(currency => currency.id)}/>
			<Field.ErrorText>{errors.account?.message}</Field.ErrorText>
		</Field.Root>
		<Field.Root mt={4} invalid={!!errors.transactionType}>
			<Field.Label>{t("entity_transaction_transaction_type")}</Field.Label>
			<CollectionSelect name="transactionType" control={control} placeholder="Select type"
				collection={state.transactionTypes} 
				labelSelector={(transactionType => transactionType.name)} 
				valueSelector={(transactionType => transactionType.id)}/>
			<Field.ErrorText>{errors.transactionType?.message}</Field.ErrorText>
		</Field.Root>
	</BaseFormModal>
}

export default TransactionModal;