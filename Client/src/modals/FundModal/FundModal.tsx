import { Button, FormControl, FormErrorMessage, FormLabel, Input, Modal, ModalBody, ModalCloseButton, ModalContent, 
	ModalFooter, ModalHeader, ModalOverlay, 
	useDisclosure} from "@chakra-ui/react"
import { forwardRef, useImperativeHandle } from "react"
import { FundEntity } from "../../models/FundEntity";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { FundFormInput, fundValidationSchema } from "./FundValidationSchema";

type FundProps = {
	fund?: FundEntity | null,
	onSaved: (fund: FundEntity) => void;
};

export interface FundModalRef {
	openModal: () => void
}

const FundModal = forwardRef<FundModalRef, FundProps>((props: FundProps, ref)=> {
	const { isOpen, onOpen, onClose } = useDisclosure()

	const { register, handleSubmit, formState: { errors }} = useForm<FundFormInput>({
        resolver: zodResolver(fundValidationSchema),
        mode: "onBlur",
        defaultValues: {
			id: props.fund?.id ?? crypto.randomUUID(),
			name: props.fund?.name ?? "",
			balance: props.fund?.balance ?? 0,
        }
    });

	useImperativeHandle(ref, () => ({
		openModal: onOpen,
	}));


	const onSubmit = (fund: FundFormInput) => {
		props.onSaved(fund as FundEntity);
		onClose();
	}

	return (
		<Modal
		//   initialFocusRef={initialRef}
		//   finalFocusRef={finalRef}
			isOpen={isOpen}
			onClose={onClose}
		>
		  <ModalOverlay />
		  <ModalContent as="form" onSubmit={handleSubmit(onSubmit)}>
			<ModalHeader>New fund</ModalHeader>
			<ModalCloseButton />
			<ModalBody pb={6}>
			  <FormControl  isInvalid={!!errors.name}>
				<FormLabel>Name</FormLabel>
				{/* ref={initialRef} */}
				<Input {...register("name")} placeholder='Debit card' />
				<FormErrorMessage>{errors.name?.message}</FormErrorMessage>
			  </FormControl>
  
			  <FormControl isInvalid={!!errors.balance} mt={4}>
				<FormLabel>Balance</FormLabel>
				<Input {...register("balance", { valueAsNumber: true })}  name="balance" placeholder='10000' />
				<FormErrorMessage>{errors.balance?.message}</FormErrorMessage>
			  </FormControl>

			  {/* <FormControl mt={4}>
				<FormLabel>Currency</FormLabel>
				<Input placeholder='10000' />
			  </FormControl> */}
			</ModalBody>
			<ModalFooter>
			  <Button type="submit" colorScheme='purple' mr={3}>Save</Button>
			  <Button onClick={onClose}>Cancel</Button>
			</ModalFooter>
		  </ModalContent>
		</Modal>
	)
})

export default FundModal