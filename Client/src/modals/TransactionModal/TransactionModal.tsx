import { forwardRef, useImperativeHandle } from 'react'
import "./TransactionModal.scss"
import { TransactionEntity } from '../../models/TransactionEntity';
import { AccountEntity } from '../../models/AccountEntity';
import DatePicker from "react-datepicker";
import { Field, Button, Input, useDisclosure, Dialog, Portal, CloseButton} from '@chakra-ui/react';
import { zodResolver } from '@hookform/resolvers/zod';
import { TransactionFormInput, TransactionValidationSchema } from './TransactionValidationSchema';
import { Controller, useForm } from 'react-hook-form';
import { Select } from "chakra-react-select"
import { useTranslation } from 'react-i18next';

type Props = {
	fundSources: AccountEntity[],
	transaction?: TransactionEntity | null,
	onSaved: (transaction: TransactionEntity) => void
}

export interface TransactionModalRef {
	openModal: () => void
}

enum TransactionDirection {
	Income = "income",
	Spent = "spent",
}

const TransactionModal = forwardRef<TransactionModalRef, Props>((props: Props, ref)=> {
	const { open, onOpen, onClose } = useDisclosure();
	const {t} = useTranslation();

	const transactionOptions = {
		[TransactionDirection.Income]: { label: t("entity_transaction_direction_income"), value: TransactionDirection.Income },
		[TransactionDirection.Spent]: { label: t("entity_transaction_direction_outcome"), value: TransactionDirection.Spent },
	} as const;

	const moneyQuantity = props.transaction?.moneyQuantity ? 
		Math.abs(props.transaction.moneyQuantity) :
		0;

	const direction = props.transaction && props.transaction.moneyQuantity > 0 ?
		transactionOptions[TransactionDirection.Income]:
		transactionOptions[TransactionDirection.Spent];

	const source = (props.fundSources?.length > 0) ? 
		props.fundSources[0] :
		{id: ""} as AccountEntity

	const { register, handleSubmit, control, formState: { errors }} = useForm<TransactionFormInput>({
		resolver: zodResolver(TransactionValidationSchema),
		mode: "onBlur",
		defaultValues: {
			id: props.transaction?.id ?? crypto.randomUUID(),
			name: props.transaction?.name ?? "",
			date: props.transaction?.date ?? new Date(),
			moneyQuantity: moneyQuantity,
			fundSource: props.transaction?.fundSource ?? source,
			direction: direction,
			transactionType: props.transaction?.transactionType ?? ""
		}
	});

	useImperativeHandle(ref, () => ({
		openModal: onOpen,
	}));

	const onTransactionSaveClick = (transaction: TransactionFormInput) => {
		const multiplier = transaction.direction.value == TransactionDirection.Income ?
			1:
			-1;

		const formData = transaction as TransactionEntity;
		formData.moneyQuantity = multiplier * formData.moneyQuantity;
		props.onSaved(formData);
		onClose();
	};

	return <Dialog.Root placement="center" open={open} onEscapeKeyDown={onClose}>
		<Portal>
			<Dialog.Backdrop/>
			<Dialog.Positioner>
				<Dialog.Content as="form" onSubmit={handleSubmit(onTransactionSaveClick)}>
					<Dialog.Header>
						<Dialog.Title>{t("entity_transaction_name_form_title")}</Dialog.Title>
					</Dialog.Header>
					<Dialog.Body pb={6}>
					<Field.Root invalid={!!errors.name}>
						<Field.Label>{t("entity_transaction_name")}</Field.Label>
						<Input {...register("name")} autoComplete="off" placeholder='Grocery' />
					</Field.Root>
					<Field.Root mt={4}>
						<Field.Label>{t("entity_transaction_direction")}</Field.Label>
						<Controller
							name="direction"
							control={control}
							render={({ field }) => (
									<Select
										{...field}
										getOptionLabel={(e) => e.label}
										getOptionValue={(e) => e.value}
										options={Object.values(transactionOptions)}
										isClearable
										placeholder='Select source'>
									</Select>
								)}
							/>
					</Field.Root>
					<Field.Root mt={4} invalid={!!errors.moneyQuantity}>
						<Field.Label>{t("entity_transaction_money_quantity")}</Field.Label>
						<Input {...register("moneyQuantity", {valueAsNumber: true})} min={0} autoComplete="off" type='number' placeholder='500' />
						<Field.ErrorText>{errors.moneyQuantity?.message}</Field.ErrorText>
					</Field.Root>
					<Field.Root mt={4} invalid={!!errors.date}>
						<Field.Label>{t("entity_transaction_date")}</Field.Label>
						<Controller
							name="date"
							control={control}
							render={({ field: {onChange, value} }) => (
								<DatePicker
								wrapperClassName="transaction-datepicker"
								selected={value}
								onChange={onChange}
								dateFormat="dd.MM.yyyy"
								placeholderText="Select date"
								customInput={<Input className='transaction-datepicker'/>}/>
							)}
							/>
						<Field.ErrorText>{errors.date?.message}</Field.ErrorText>
					</Field.Root>
					<Field.Root mt={4} invalid={!!errors.fundSource}>
						<Field.Label>{t("entity_transaction_fund_source")}</Field.Label>
						<Controller
							name="fundSource"
							control={control}
							render={({ field }) => (
									<Select
										{...field}
										getOptionLabel={(e) => e.name}
										getOptionValue={(e) => e.id}
										options={props.fundSources}
										isClearable
										placeholder='Select source'>
									</Select>
								)}
							/>
						<Field.ErrorText>{errors.fundSource?.message}</Field.ErrorText>
					</Field.Root>
					<Field.Root mt={4} invalid={!!errors.transactionType}>
						<Field.Label>{t("entity_transaction_transaction_type")}</Field.Label>
						<Input {...register("transactionType")} autoComplete="off" placeholder='Describe type' />
						<Field.ErrorText>{errors.transactionType?.message}</Field.ErrorText>
					</Field.Root>
					</Dialog.Body>
					<Dialog.Footer>
						<Button type='submit' background='purple.600' mr={3}>{t("modals_save_button")}</Button>
						<Button onClick={onClose}>{t("modals_cancel_button")}</Button>
					</Dialog.Footer>
					<Dialog.CloseTrigger asChild>
						<CloseButton onClick={onClose} size="sm" />
					</Dialog.CloseTrigger>
				</Dialog.Content>
			</Dialog.Positioner>
		</Portal>
	</Dialog.Root>
})

export default TransactionModal;