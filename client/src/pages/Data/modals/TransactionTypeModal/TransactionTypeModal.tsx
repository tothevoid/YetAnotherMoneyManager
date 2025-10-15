import { Box, Button, Field, Input, Image, Flex } from "@chakra-ui/react"
import React, { ChangeEvent, Fragment, RefObject, useEffect, useRef, useState } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import { TransactionTypeFormInput, TransactionTypeValidationSchema } from "./TransactionTypeValidationSchema";
import { MdFileUpload } from "react-icons/md";
import { getTransactionTypeIconUrl } from "../../../../api/transactions/transactionTypeApi";
import { TransactionTypeEntity } from "../../../../models/transactions/TransactionTypeEntity";
import CheckboxInput from "../../../../shared/components/CheckboxInput/CheckboxInput";
import BaseFormModal from "../../../../shared/modals/BaseFormModal/BaseFormModal";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import { generateGuid } from "../../../../shared/utilities/idUtilities";

interface ModalProps {
	modalRef: RefObject<BaseModalRef | null>,
	transactionType?: TransactionTypeEntity | null,
	onSaved: (account: TransactionTypeEntity, icon: File | null) => void;
};

const TransactionTypeModal: React.FC<ModalProps> = (props: ModalProps) => {
	
	const {t} = useTranslation();

	const { register, reset, handleSubmit, control, formState: { errors }} = useForm<TransactionTypeFormInput>({
		resolver: zodResolver(TransactionTypeValidationSchema),
		mode: "onBlur",
		defaultValues: {
			id: props.transactionType?.id ?? generateGuid(),
			name: props.transactionType?.name ?? "",
			active: props.transactionType?.active ?? true
		}
	});

	useEffect(() => {
		if (props.transactionType) {
			reset(props.transactionType);
		}
	}, [props.transactionType]);

	const onSubmit = (transactionType: TransactionTypeFormInput) => {
		props.onSaved({...transactionType, iconKey: props.transactionType?.iconKey } as TransactionTypeEntity, icon);
		props.modalRef?.current?.closeModal();
	}

	const inputRef = useRef<HTMLInputElement | null>(null);
	const [icon, setIcon] = useState<File | null>(null);
	const [iconUrl, setIconUrl] = useState<string | null>(null);

	useEffect(() => {
		const url = getTransactionTypeIconUrl(props.transactionType?.iconKey);
		setIconUrl(url);
	}, [props.transactionType]);

	const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
		const selectedFile = e.target.files?.[0] || null;
		if (selectedFile) {
			const reader = new FileReader();
			reader.onloadend = () => {
				setIcon(selectedFile);
				setIconUrl(reader.result as string);
			};
			reader.readAsDataURL(selectedFile);
		}
	};

	return <BaseFormModal ref={props.modalRef} title={t("entity_transaction_type_name_form_title")} submitHandler={handleSubmit(onSubmit)}>
		<Flex marginBlock={5} alignItems={"center"} justifyContent={"center"}>
			<Box justifyContent={"center"} role="group" position="relative" boxSize="50px">
				<Input type="file" accept="image/*" onChange={handleFileChange} ref={inputRef} display="none" />
				{
					iconUrl ?
						<Image src={iconUrl} boxSize="50px"
							backgroundColor="black"
							objectFit="contain"
							borderColor="gray.200"
							borderRadius="md"></Image>:
						<Fragment/>
				}
				{/* Fix group hover */}
				<Button 
					background={"transparent"} 
					color="action_primary" 
					position="absolute"
					top="0"
					left="0"
					boxSize="50px"
					borderRadius="md"
					bg="whiteAlpha.400"
					opacity={iconUrl ? 0: 1}
					_groupHover={{ opacity: 1, bg: 'whiteAlpha.900' }}
					transition="opacity 0.2s"
					boxShadow="sm"
					onClick={() => inputRef.current?.click()} size="xl">
					<MdFileUpload/>
				</Button>
			</Box>
		</Flex>
		<Field.Root invalid={!!errors.name}>
			<Field.Label>{t("entity_transaction_type_name")}</Field.Label>
			<Input {...register("name")} autoComplete="off" placeholder='grocery' />
			<Field.ErrorText>{errors.name?.message}</Field.ErrorText>
		</Field.Root>
		<Field.Root invalid={!!errors.active} mt={4}>
			<CheckboxInput name="active" control={control} title={t("entity_transaction_type_active")}/>
			<Field.ErrorText>{errors.active?.message}</Field.ErrorText>
		</Field.Root>
	</BaseFormModal>
}

export default TransactionTypeModal