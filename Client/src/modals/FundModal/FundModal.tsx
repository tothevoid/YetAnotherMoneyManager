import { Button, CloseButton, Dialog, Field, Input, Portal, useDisclosure} from "@chakra-ui/react"
import { forwardRef, useImperativeHandle } from "react"
import { FundEntity } from "../../models/FundEntity";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { FundFormInput, FundValidationSchema } from "./FundValidationSchema";

type FundProps = {
	fund?: FundEntity | null,
	onSaved: (fund: FundEntity) => void;
};

export interface FundModalRef {
	openModal: () => void
}

const FundModal = forwardRef<FundModalRef, FundProps>((props: FundProps, ref)=> {
	const { open, onOpen, onClose } = useDisclosure()

	const { register, handleSubmit, formState: { errors }} = useForm<FundFormInput>({
        resolver: zodResolver(FundValidationSchema),
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
		<Dialog.Root placement="center" open={open}>
		  <Portal>
			<Dialog.Backdrop/>
			<Dialog.Positioner>
				<Dialog.Content as="form" onSubmit={handleSubmit(onSubmit)}>
					<Dialog.Header>
						<Dialog.Title>Fund</Dialog.Title>
					</Dialog.Header>
					<Dialog.Body pb={6}>
					<Field.Root invalid={!!errors.name}>
						<Field.Label>Name</Field.Label>
						<Input {...register("name")} placeholder='Debit card' />
						<Field.ErrorText>{errors.name?.message}</Field.ErrorText>
					</Field.Root>
		
					<Field.Root invalid={!!errors.balance} mt={4}>
						<Field.Label>Balance</Field.Label>
						<Input {...register("balance", { valueAsNumber: true })}  name="balance" placeholder='10000' />
						<Field.ErrorText>{errors.balance?.message}</Field.ErrorText>
					</Field.Root>

					{/* <Field.Root mt={4}>
						<Field.Label>Currency</Field.Label>
						<Input placeholder='10000' />
					</Field.Root> */}
					</Dialog.Body>
					<Dialog.Footer>
					<Button type="submit" background='purple.600' mr={3}>Save</Button>
					<Button onClick={onClose}>Cancel</Button>
					</Dialog.Footer>
				</Dialog.Content>
				<Dialog.CloseTrigger asChild>
					<CloseButton size="sm" />
				</Dialog.CloseTrigger>
			</Dialog.Positioner>
		  </Portal>
		</Dialog.Root>
	)
})

export default FundModal