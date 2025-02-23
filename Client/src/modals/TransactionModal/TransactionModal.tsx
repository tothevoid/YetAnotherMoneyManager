import React, { forwardRef, useEffect, useImperativeHandle, useState } from 'react'
import "./TransactionModal.scss"
import { TransactionEntity } from '../../models/TransactionEntity';
import { FundEntity } from '../../models/FundEntity';
import { TransactionType } from '../../models/TransactionType';
import DatePicker from "react-datepicker";
import { FormControl, Button, FormLabel, Input, Modal, ModalBody, ModalCloseButton, 
	ModalContent, ModalFooter, ModalHeader, ModalOverlay, useDisclosure, Select } from '@chakra-ui/react';

type Props = {
	fundSources: FundEntity[],
	transaction?: TransactionEntity | null,
	transactionTypes: TransactionType[],
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
	const { isOpen, onOpen, onClose } = useDisclosure()

	const moneyQuantity = props.transaction?.moneyQuantity ? 
		Math.abs(props.transaction.moneyQuantity) :
		0;

	const direction: number | null = props.transaction && props.transaction.moneyQuantity > 0 ?
		TransactionDirection.Income:
		TransactionDirection.Spent;

	const getInitialState = (): TransactionEntity => {
		const source = (props.fundSources?.length > 0) ? 
			props.fundSources[0] :
			{id: ""} as FundEntity
	
		const transactionType = (props.transactionTypes?.length > 0) ? 
			props.transactionTypes[0] :
			{id: ""} as TransactionType

		return {
			id: props.transaction?.id ?? crypto.randomUUID(),
			name: props.transaction?.name ?? "",
			date: props.transaction?.date ?? new Date(),
			moneyQuantity: moneyQuantity,
			fundSource: props.transaction?.fundSource ?? source,
			transactionType: props.transaction?.transactionType ?? transactionType
		};
	}

	const [formData, setFormData] = useState<TransactionEntity>(getInitialState);
	const [modalState, setModalState] = useState<ModalState>({direction});

	useEffect(() => {
		setFormData(getInitialState());
	}, [props.transaction, props.fundSources, props.transactionTypes]);


	useImperativeHandle(ref, () => ({
		openModal: onOpen,
	}));

	const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		const { name, value, type } = e.target;
		const normalizedValue = (type === "number" || type === "price") ? parseFloat(value) : value;
		setFormData((prev) => ({ ...prev, [name]: normalizedValue }));
	};

	const handleTransactionDirectionChange = ({ target: { value } }: React.ChangeEvent<HTMLSelectElement>) => {
		const newValue = value ? parseInt(value): null;

		setModalState((prev) => ({ ...prev, direction: newValue }));
	}

	const handleSourceChange = ({ target: { name, value } }: React.ChangeEvent<HTMLSelectElement>) => {
		const source = props.fundSources.find((entity: FundEntity) => entity.id === value);
		if (source){
			setFormData((prev) => ({ ...prev, [name]: source }));
		}
	}

	const handleTypeChange = ({ target: { name, value } }: React.ChangeEvent<HTMLSelectElement>) => {
		const transactionType = props.transactionTypes.find((transactionType: TransactionType) => transactionType.id === value);
		if (transactionType){
			setFormData((prev) => ({ ...prev, [name]: transactionType }));
		}
	}

	const onDateChanged = (date: Date | null) => {
		if (!date) {
			return;
		}

		setFormData((prev: TransactionEntity) => ({ ...prev, date: date }));
	}

	const onTransactionSaveClick = () => {
		const multiplier = modalState.direction === TransactionDirection.Income ?
			1:
			-1;

		formData.moneyQuantity = multiplier * formData.moneyQuantity;
		props.onSaved(formData);
		onClose();
	};

	return (<Modal
		//   initialFocusRef={initialRef}
		//   finalFocusRef={finalRef}
			isOpen={isOpen}
			onClose={onClose}
		>
		<ModalOverlay />
		<ModalContent>
			<ModalHeader>New fund</ModalHeader>
			<ModalCloseButton />
			<ModalBody pb={6}>
			<FormControl>
				<FormLabel>Name</FormLabel>
				{/* ref={initialRef} */}
				<Input name="name" value={formData.name} onChange={handleChange} placeholder='Grocery' />
			</FormControl>
			<FormControl mt={4}>
				<FormLabel>Direction</FormLabel>
				<Select name="direction" onChange={handleTransactionDirectionChange} placeholder='Select direction'
					value={modalState.direction as number}>
					{[...transactionDirections.entries()].map(([key, value]) => {
						return <option key={key} value={key}>{value}</option>
					})}
				</Select>
			</FormControl>
			<FormControl mt={4}>
				<FormLabel>Diff</FormLabel>
				<Input type='number' name="moneyQuantity" value={formData.moneyQuantity} onChange={handleChange} placeholder='500' />
			</FormControl>
			<FormControl mt={4}>
				<FormLabel>Date</FormLabel>
				<DatePicker wrapperClassName="transaction-datepicker"
					selected={formData.date}
					onChange={onDateChanged}
					dateFormat="dd.MM.yyyy"
					customInput={<Input/>}
				/>
			</FormControl>
			<FormControl mt={4}>
				<FormLabel>Source</FormLabel>
				<Select name="fundSource" value={formData.fundSource.id} onChange={handleSourceChange} placeholder='Select source'>
					{props.fundSources.map(({id, name}) => {
						return <option key={id} value={id}>{name}</option>
					})}
				</Select>
			</FormControl>
			<FormControl mt={4}>
				<FormLabel>Type</FormLabel>
				<Select name="transactionType" value={formData.transactionType.id} onChange={handleTypeChange} placeholder='Select type'>
					{props.transactionTypes.map(({id, name}) => {
						return <option key={id} value={id}>{name}</option>
					})}
				</Select>
			</FormControl>
			</ModalBody>
			<ModalFooter>
				<Button onClick={onTransactionSaveClick} colorScheme='blue' mr={3}>Save</Button>
				<Button onClick={onClose}>Cancel</Button>
			</ModalFooter>
		</ModalContent>
	</Modal>)
})


export default TransactionModal;