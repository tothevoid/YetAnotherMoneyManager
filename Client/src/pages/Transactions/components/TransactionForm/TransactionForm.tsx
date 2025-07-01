import React, { Fragment, useEffect, useState } from 'react'
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
import { getAccounts } from '../../../../api/accounts/accountApi';

interface ModalProps {
	setSubmitHandler: (handler: React.FormEventHandler) => void,
	transaction?: TransactionEntity,
	onTransactionSaved: (transaction: TransactionEntity) => Promise<void>
}

interface State {
	transactionTypes: TransactionTypeEntity[],
	accounts: AccountEntity[]
}

enum TransactionDirection {
	Income = "income",
	Spent = "spent",
}

const TransactionForm: React.FC<ModalProps> = (props: ModalProps) => {
	const {t} = useTranslation();

	const [state, setState] = useState<State>({transactionTypes: [], accounts: []});

	const amount = props.transaction?.amount ? 
		Math.abs(props.transaction.amount) :
		0;

	const transactionOptions = {
		[TransactionDirection.Income]: { label: t("entity_transaction_direction_income"), value: TransactionDirection.Income },
		[TransactionDirection.Spent]: { label: t("entity_transaction_direction_outcome"), value: TransactionDirection.Spent },
	} as const;

	const direction = props.transaction && props.transaction.amount > 0 ?
		transactionOptions[TransactionDirection.Income]:
		transactionOptions[TransactionDirection.Spent];

	const source = (state.accounts?.length > 0) ? 
		state.accounts[0] :
		{id: ""} as AccountEntity;

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

	const initTransactionTypes = async () => {
		const transactionTypes = await getTransactionTypes(true);
		const accounts = await getAccounts(true);

		setState((currentState) => {
			return {...currentState, transactionTypes, accounts}
		})
	};

	const onTransactionSaveClick = (transaction: TransactionFormInput) => {
		const multiplier = transaction.direction.value == TransactionDirection.Income ?
			1:
			-1;

		const formData = transaction as TransactionEntity;
		formData.amount = multiplier * formData.amount;
		props.onTransactionSaved(formData);
	};
	
	useEffect(() => {
		const initData = async () => {
			await initTransactionTypes();
		}

		const handler = handleSubmit(onTransactionSaveClick)
		props.setSubmitHandler(handler);
		initData();
	}, []);

	const selectedDirection = watch("direction");

	return <Fragment>
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
			<Field.Label>{t("entity_transaction_account")}</Field.Label>
			<CollectionSelect name="account" control={control} placeholder="Select account"
				collection={state.accounts} 
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
	</Fragment>
}

export default TransactionForm;