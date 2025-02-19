import { Button, FormControl, FormLabel, Input, Modal, ModalBody, ModalCloseButton, ModalContent, 
	ModalFooter, ModalHeader, ModalOverlay, 
	useDisclosure} from "@chakra-ui/react"
import { forwardRef, useImperativeHandle, useState } from "react"
import { FundEntity } from "../../models/FundEntity";

type FundProps = {
	fund?: FundEntity | null,
	onSaved: (fund: FundEntity) => void;
};

export interface FundModalRef {
	openModal: () => void
}

const FundModal = forwardRef<FundModalRef, FundProps>((props: FundProps, ref)=> {
	const { isOpen, onOpen, onClose } = useDisclosure()

	const initialState = {
		id: props.fund?.id ?? crypto.randomUUID(),
		name: props.fund?.name ?? "",
		balance: props.fund?.balance ?? 0,
	}

	useImperativeHandle(ref, () => ({
		openModal: onOpen,
	}));

	const [formData, setFormData] = useState(initialState);

	const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
	  const { name, value } = e.target;
	  setFormData((prev) => ({ ...prev, [name]: value }));
	};

	const onFundSaveClick = () => {
		props.onSaved({id: formData.id, name: formData.name, balance: Number(formData.balance)});
		setFormData(initialState)
		onClose();
	};

	return (
		<Modal
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
				<Input name="name" value={formData.name} onChange={handleChange} placeholder='Debit card' />
			  </FormControl>
  
			  <FormControl mt={4}>
				<FormLabel>Balance</FormLabel>
				<Input type="number" name="balance" value={formData.balance} onChange={handleChange} placeholder='10000' />
			  </FormControl>

			  {/* <FormControl mt={4}>
				<FormLabel>Currency</FormLabel>
				<Input placeholder='10000' />
			  </FormControl> */}
			</ModalBody>
			<ModalFooter>
			  <Button onClick={onFundSaveClick} colorScheme='blue' mr={3}>Save</Button>
			  <Button onClick={onClose}>Cancel</Button>
			</ModalFooter>
		  </ModalContent>
		</Modal>
	)
})

export default FundModal