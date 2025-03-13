import { forwardRef, useImperativeHandle } from 'react'
import "./TransactionModal.scss"
import { TransactionEntity } from '../../models/TransactionEntity';
import { FundEntity } from '../../models/FundEntity';
import DatePicker from "react-datepicker";
import { Field, Button, Input, useDisclosure, Dialog, Portal, CloseButton} from '@chakra-ui/react';
import { zodResolver } from '@hookform/resolvers/zod';
import { TransactionFormInput, TransactionValidationSchema } from './TransactionValidationSchema';
import { Controller, useForm } from 'react-hook-form';
import { Select } from "chakra-react-select"

type Props = {
	fundSources: FundEntity[],
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

const transactionOptions = {
	[TransactionDirection.Income]: { label: "Income", value: TransactionDirection.Income },
	[TransactionDirection.Spent]: { label: "Spent", value: TransactionDirection.Spent },
} as const;


const TransactionModal = forwardRef<TransactionModalRef, Props>((props: Props, ref)=> {
	const { open, onOpen, onClose } = useDisclosure()

	const moneyQuantity = props.transaction?.moneyQuantity ? 
		Math.abs(props.transaction.moneyQuantity) :
		0;

	const direction = props.transaction && props.transaction.moneyQuantity > 0 ?
		transactionOptions[TransactionDirection.Income]:
		transactionOptions[TransactionDirection.Spent];

	const source = (props.fundSources?.length > 0) ? 
		props.fundSources[0] :
		{id: ""} as FundEntity

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

	// useEffect(() => {
	// 	setFormData(getInitialState());
	// 	// props.transactionTypes
	// }, [props.transaction, props.fundSources]);

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

	return (<Dialog.Root placement="center" open={open}>
		<Portal>
			<Dialog.Backdrop/>
			<Dialog.Positioner>
				<Dialog.Content as="form" onSubmit={handleSubmit(onTransactionSaveClick)}>
					<Dialog.Header>
						<Dialog.Title>Transaction</Dialog.Title>
					</Dialog.Header>
					<Dialog.Body pb={6}>
					<Field.Root invalid={!!errors.name}>
						<Field.Label>Name</Field.Label>
						<Input {...register("name")} autoComplete="off" placeholder='Grocery' />
					</Field.Root>
					<Field.Root mt={4}>
						<Field.Label>Direction</Field.Label>
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
						<Field.Label>Diff</Field.Label>
						<Input {...register("moneyQuantity", {valueAsNumber: true})} min={0} autoComplete="off" type='number' placeholder='500' />
						<Field.ErrorText>{errors.moneyQuantity?.message}</Field.ErrorText>
					</Field.Root>
					<Field.Root mt={4} invalid={!!errors.date}>
						<Field.Label>Date</Field.Label>
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
						<Field.Label>Source</Field.Label>
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
						<Field.Label>Type</Field.Label>
						<Input {...register("transactionType")} autoComplete="off" placeholder='Describe type' />
						<Field.ErrorText>{errors.transactionType?.message}</Field.ErrorText>
					</Field.Root>
					</Dialog.Body>
					<Dialog.Footer>
						<Button type='submit' background='purple.600' mr={3}>Save</Button>
						<Button onClick={onClose}>Cancel</Button>
					</Dialog.Footer>
					<Dialog.CloseTrigger asChild>
						<CloseButton onClick={onClose} size="sm" />
					</Dialog.CloseTrigger>
				</Dialog.Content>
			</Dialog.Positioner>
		</Portal>
	</Dialog.Root>)
})

export default TransactionModal;