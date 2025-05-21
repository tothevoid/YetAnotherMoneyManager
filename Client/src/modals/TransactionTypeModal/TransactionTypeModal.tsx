import { Box, Button, CloseButton, Dialog, Field, Input, Portal, useDisclosure, Image, Flex } from "@chakra-ui/react"
import { forwardRef, Fragment, useEffect, useImperativeHandle, useRef, useState } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import CheckboxInput from "../../controls/CheckboxInput/CheckboxInput";
import { TransactionTypeFormInput, TransactionTypeValidationSchema } from "./TransactionTypeValidationSchema";
import { TransactionTypeEntity } from "../../models/transactions/TransactionTypeEntity";
import { getTransactionTypeIconUrl } from "../../api/transactions/transactionTypeApi";
import { MdFileUpload } from "react-icons/md";

type Props = {
    transactionType?: TransactionTypeEntity | null,
    onSaved: (account: TransactionTypeEntity, icon: File | null) => void;
};

export interface TransactionTypeModalRef {
    openModal: () => void
}

const TransactionTypeModal = forwardRef<TransactionTypeModalRef, Props>((props: Props, ref)=> {
    const { open, onOpen, onClose } = useDisclosure()

    const { register, reset, handleSubmit, control, formState: { errors }} = useForm<TransactionTypeFormInput>({
        resolver: zodResolver(TransactionTypeValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.transactionType?.id ?? crypto.randomUUID(),
            name: props.transactionType?.name ?? "",
            active: props.transactionType?.active ?? true
        }
    });

    useEffect(() => {
        if (props.transactionType) {
            reset(props.transactionType);
        }
    }, [props.transactionType]);

    useImperativeHandle(ref, () => ({
        openModal: onOpen,
    }));

    const onSubmit = (transactionType: TransactionTypeFormInput) => {
        props.onSaved(transactionType as TransactionTypeFormInput, icon);
        onClose();
    }

    const defaultIconUrl = getTransactionTypeIconUrl(props.transactionType?.iconKey);

    const inputRef = useRef<HTMLInputElement | null>(null);
    const [icon, setIcon] = useState<File | null>(null);
    const [iconUrl, setIconUrl] = useState<string | null>(defaultIconUrl);

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

    const {t} = useTranslation()

    return (
        <Dialog.Root placement="center" open={open} onEscapeKeyDown={onClose}>
          <Portal>
            <Dialog.Backdrop/>
            <Dialog.Positioner>
                <Dialog.Content as="form" onSubmit={handleSubmit(onSubmit)}>
                    <Dialog.Header>
                        <Dialog.Title>{t("entity_transaction_type_name_form_title")}</Dialog.Title>
                    </Dialog.Header>
                    <Dialog.Body pb={6}>
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
                                    color="purple.600" 
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
                    </Dialog.Body>
                    <Dialog.Footer>
                        <Button type="submit" background='purple.600' mr={3}>{t("modals_save_button")}</Button>
                        <Button onClick={onClose}>{t("modals_cancel_button")}</Button>
                    </Dialog.Footer>
                    <Dialog.CloseTrigger asChild>
                        <CloseButton onClick={onClose} size="sm" />
                    </Dialog.CloseTrigger>
                </Dialog.Content>
            </Dialog.Positioner>
          </Portal>
        </Dialog.Root>
    )
})
export default TransactionTypeModal