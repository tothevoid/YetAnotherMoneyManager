import React, { forwardRef, useImperativeHandle, useState } from 'react'
import "./TransactionModal.scss"
import { TransactionEntity } from '../../models/TransactionEntity';
import { FundEntity } from '../../models/FundEntity';
// import { TransactionType } from '../../models/TransactionType';
import DatePicker from "react-datepicker";
import { Field, Button, Input, useDisclosure, Select, Dialog, Portal, CloseButton} from '@chakra-ui/react';
import { zodResolver } from '@hookform/resolvers/zod';
import { TransactionFormInput, TransactionValidationSchema } from './TransactionValidationSchema';
import { Controller, useForm } from 'react-hook-form';


type Props = {
	fundSources: FundEntity[],
	transaction?: TransactionEntity | null,
	// transactionTypes: TransactionType[],
	onSaved: (transaction: TransactionEntity) => void
	// onTypeAdded: (type: TransactionType) => void
}

export interface TransactionModalRef {
	openModal: () => void
}

type ModalState = {
	direction: TransactionDirection | null
}

enum TransactionDirection {
	Income = 0,
	Spent = 1
}

const transactionDirections = new Map<TransactionDirection, string>([
	[TransactionDirection.Income, "Income"],
	[TransactionDirection.Spent, "Spent"],
])

const TransactionModal = forwardRef<TransactionModalRef, Props>((props: Props, ref)=> {
	const { open, onOpen, onClose } = useDisclosure()

	const moneyQuantity = props.transaction?.moneyQuantity ? 
		Math.abs(props.transaction.moneyQuantity) :
		0;

	const direction: number | null = props.transaction && props.transaction.moneyQuantity > 0 ?
		TransactionDirection.Income:
		TransactionDirection.Spent;

	// const transactionType = (props.transactionTypes?.length > 0) ? 
		// 	props.transactionTypes[0] :
		// 	"";

	// transactionType: props.transaction?.transactionType ?? transactionType

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
			transactionType: ""
		}
	});

	const [modalState, setModalState] = useState<ModalState>({direction});

	// useEffect(() => {
	// 	setFormData(getInitialState());
	// 	// props.transactionTypes
	// }, [props.transaction, props.fundSources]);

	useImperativeHandle(ref, () => ({
		openModal: onOpen,
	}));


	const handleTransactionDirectionChange = ({ target: { value } }: React.ChangeEvent<HTMLSelectElement>) => {
		const newValue = value ? parseInt(value): null;

		setModalState((prev) => ({ ...prev, direction: newValue }));
	}

	// const handleSourceChange = ({ target: { name, value } }: React.ChangeEvent<HTMLSelectElement>) => {
	// 	const source = props.fundSources.find((entity: FundEntity) => entity.id === value);
	// 	if (source){
	// 		setFormData((prev) => ({ ...prev, [name]: source }));
	// 	}
	// }

	const onTransactionSaveClick = (transaction: TransactionFormInput) => {
		const multiplier = modalState.direction === TransactionDirection.Income ?
			1:
			-1;

		const formData = transaction as TransactionEntity;
		formData.moneyQuantity = multiplier * formData.moneyQuantity;
		props.onSaved(formData);
		onClose();
	};

	return (<Dialog.Root placement="center" open={open}>
		{/* //   initialFocusRef={initialRef}
		//   finalFocusRef={finalRef}
		// onClose={onClose} */}
		<Portal>
			<Dialog.Backdrop/>
			<Dialog.Positioner>
				<Dialog.Content as="form" onSubmit={handleSubmit(onTransactionSaveClick)}>
					<Dialog.Header>
						<Dialog.Title>New fund</Dialog.Title>
					</Dialog.Header>
					{/* <ModalCloseButton /> */}
					<Dialog.Body pb={6}>
					<Field.Root invalid={!!errors.name}>
						<Field.Label>Name</Field.Label>
						{/* ref={initialRef} */}
						<Input {...register("name")} autoComplete="off" placeholder='Grocery' />
					</Field.Root>
					<Field.Root mt={4}>
						<Field.Label>Direction</Field.Label>
						{/* <Select name="direction" onChange={handleTransactionDirectionChange} placeholder='Select direction'
							value={modalState.direction as number}>
							{[...transactionDirections.entries()].map(([key, value]) => {
								return <option key={key} value={key}>{value}</option>
							})}
						</Select> */}
					</Field.Root>
					<Field.Root mt={4} invalid={!!errors.moneyQuantity}>
						<Field.Label>Diff</Field.Label>
						<Input {...register("moneyQuantity", {valueAsNumber: true})} autoComplete="off" type='number' placeholder='500' />
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
								customInput={<Input/>}/>
							)}
							/>
						<Field.ErrorText>{errors.date?.message}</Field.ErrorText>
					</Field.Root>
					<Field.Root mt={4} invalid={!!errors.fundSource}>
						<Field.Label>Source</Field.Label>
						{/* <Controller
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
							/> */}
						<Field.ErrorText>{errors.fundSource?.message}</Field.ErrorText>
					</Field.Root>
					<Field.Root mt={4} invalid={!!errors.transactionType}>
						<Field.Label>Type</Field.Label>
						<Input {...register("transactionType")} placeholder='Describe type' />
						<Field.ErrorText>{errors.transactionType?.message}</Field.ErrorText>
					</Field.Root>
					</Dialog.Body>
					<Dialog.Footer>
						<Button type='submit' background='purple.600' mr={3}>Save</Button>
						<Button onClick={onClose}>Cancel</Button>
					</Dialog.Footer>
					<Dialog.CloseTrigger asChild>
						<CloseButton size="sm" />
					</Dialog.CloseTrigger>
				</Dialog.Content>
				<Dialog.CloseTrigger asChild>
					<CloseButton size="sm" />
				</Dialog.CloseTrigger>
			</Dialog.Positioner>
		</Portal>
	</Dialog.Root>)
})


export default TransactionModal;