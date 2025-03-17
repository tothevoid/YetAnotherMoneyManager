import { Button, CloseButton, Dialog, Field, Input, Portal, useDisclosure} from "@chakra-ui/react"
import { forwardRef, useImperativeHandle } from "react"
import { AccountEntity } from "../../models/AccountEntity";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { AccountFormInput, AccountValidationSchema } from "./AccountValidationSchema";
import { useTranslation } from "react-i18next";

type AccountProps = {
	account?: AccountEntity | null,
	onSaved: (account: AccountEntity) => void;
};

export interface AccountModalRef {
	openModal: () => void
}

const AccountModal = forwardRef<AccountModalRef, AccountProps>((props: AccountProps, ref)=> {
	const { open, onOpen, onClose } = useDisclosure()

	const { register, handleSubmit, formState: { errors }} = useForm<AccountFormInput>({
        resolver: zodResolver(AccountValidationSchema),
        mode: "onBlur",
        defaultValues: {
			id: props.account?.id ?? crypto.randomUUID(),
			name: props.account?.name ?? "",
			balance: props.account?.balance ?? 0,
        }
    });

	useImperativeHandle(ref, () => ({
		openModal: onOpen,
	}));


	const onSubmit = (account: AccountFormInput) => {
		props.onSaved(account as AccountEntity);
		onClose();
	}

	const {t} = useTranslation()

	return (
		<Dialog.Root placement="center" open={open} onEscapeKeyDown={onClose}>
		  <Portal>
			<Dialog.Backdrop/>
			<Dialog.Positioner>
				<Dialog.Content as="form" onSubmit={handleSubmit(onSubmit)}>
					<Dialog.Header>
						<Dialog.Title>{t("entity_account_name_form_title")}</Dialog.Title>
					</Dialog.Header>
					<Dialog.Body pb={6}>
					<Field.Root invalid={!!errors.name}>
						<Field.Label>{t("entity_account_name")}</Field.Label>
						<Input {...register("name")} autoComplete="off" placeholder='Debit card' />
						<Field.ErrorText>{errors.name?.message}</Field.ErrorText>
					</Field.Root>
		
					<Field.Root invalid={!!errors.balance} mt={4}>
						<Field.Label>{t("entity_account_balance")}</Field.Label>
						<Input {...register("balance", { valueAsNumber: true })} name="balance" type="number" placeholder='10000' />
						<Field.ErrorText>{errors.balance?.message}</Field.ErrorText>
					</Field.Root>

					{/* <Field.Root mt={4}>
						<Field.Label>Currency</Field.Label>
						<Input placeholder='10000' />
					</Field.Root> */}
					</Dialog.Body>
					<Dialog.Footer>
					<Button type="submit" background='purple.600' mr={3}>{t("modals_save_button")}</Button>
					<Button onClick={onClose}>{t("modals_cancel_button")}</Button>
					</Dialog.Footer>
					<Dialog.CloseTrigger asChild>
						<CloseButton size="sm" />
					</Dialog.CloseTrigger>
				</Dialog.Content>
			</Dialog.Positioner>
		  </Portal>
		</Dialog.Root>
	)
})

export default AccountModal