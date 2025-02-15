import React, { forwardRef, useImperativeHandle, useState } from 'react'
import { TransactionEntity } from '../../models/TransactionEntity';
import { FundEntity } from '../../models/FundEntity';
import { getCurrentDate } from '../../utils/DateUtils';
import { TransactionType } from '../../models/TransactionType';
// import TransactionTypeSelect from '../../TransactionTypeSelect/TransactionTypeSelect';
import { FormControl, Button, FormLabel, Input, Modal, ModalBody, ModalCloseButton, ModalContent, ModalFooter, ModalHeader, ModalOverlay, useDisclosure, Select } from '@chakra-ui/react';


type Props = {
	fundSources: FundEntity[],
	transaction: TransactionEntity | null,
	transactionTypes: TransactionType[],
	onSaved: (transaction: TransactionEntity) => void
	onTypeAdded: (type: TransactionType) => void
}

const TransactionModal: React.FC<Props> = forwardRef((props: Props, ref)=> {
	const { isOpen, onOpen, onClose } = useDisclosure()

	const source = (props.fundSources && props.fundSources.length !== 0) ? 
		props.fundSources[0] :
		{id: ""} as FundEntity

	const initialState: TransactionEntity = {
		id: props.transaction?.id ?? crypto.randomUUID(),
		name: props.transaction?.name ?? "",
		date: props.transaction?.date ?? getCurrentDate(),
		moneyQuantity: props.transaction?.moneyQuantity ?? 0,
		fundSource: props.transaction?.fundSource ?? source,
		transactionType: props.transaction?.transactionType ?? {id: ""} as TransactionType,
	};

	useImperativeHandle(ref, () => ({
		openModal: onOpen,
	}));

	const [formData, setFormData] = useState(initialState);

	const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		const { name, value, type } = e.target;
		const normalizedValue = (type === "number" || type === "price") ? parseFloat(value) : value;
		setFormData((prev) => ({ ...prev, [name]: normalizedValue }));
	};

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

	const onTransactionSaveClick = () => {
		props.onSaved(formData);
		setFormData(initialState)
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
				<FormLabel>Diff</FormLabel>
				<Input type="number" name="moneyQuantity" value={formData.moneyQuantity} onChange={handleChange} placeholder='500' />
			</FormControl>
			<FormControl mt={4}>
				<FormLabel>Diff</FormLabel>
				<Input type="date" name="date" value={formData.date} onChange={handleChange} placeholder='500' />
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