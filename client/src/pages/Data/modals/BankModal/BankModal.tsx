import { Box, Button, Field, Flex, Input, Image} from "@chakra-ui/react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import { ChangeEvent, Fragment, RefObject, useEffect, useRef, useState } from "react";
import BaseFormModal from "../../../../shared/modals/BaseFormModal/BaseFormModal";
import { generateGuid } from "../../../../shared/utilities/idUtilities";
import { BankFormInput, BankValidationSchema } from "./BankValidationSchema";
import { BankEntity } from "../../../../models/banks/BankEntity";
import { Nullable } from "../../../../shared/utilities/nullable";
import { getBankIconUrl } from "../../../../api/banks/bankApi";
import { MdFileUpload } from "react-icons/md";


interface ModalProps {
    modalRef: RefObject<BaseModalRef | null>,
    bank?: BankEntity | null,
    onSaved: (account: BankEntity, icon: Nullable<File>) => void;
};

const BankModal: React.FC<ModalProps> = (props: ModalProps) => {
    const { register, handleSubmit, formState: { errors }, reset} = useForm<BankFormInput>({
        resolver: zodResolver(BankValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.bank?.id ?? generateGuid(),
            name: props.bank?.name ?? ""
        }
    });

    const inputRef = useRef<HTMLInputElement | null>(null);
    const [icon, setIcon] = useState<File | null>(null);
    const [iconUrl, setIconUrl] = useState<string | null>(null);

    useEffect(() => {
        if (props.bank) {
            reset(props.bank);
        }
    }, [props.bank]);

    useEffect(() => {
        const url = getBankIconUrl(props.bank?.iconKey);
        setIconUrl(url);
    }, [props.bank]);

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

    const onSubmit = (transactionType: BankFormInput) => {
        props.onSaved({...transactionType, iconKey: props.bank?.iconKey } as BankEntity, icon);
        props.modalRef?.current?.closeModal();
    }
    
    const {t} = useTranslation();

    return <BaseFormModal ref={props.modalRef} title={t("entity_bank_from_title")} submitHandler={handleSubmit(onSubmit)}>
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
            <Field.Label>{t("entity_bank_name")}</Field.Label>
            <Input {...register("name")} autoComplete="off" placeholder='Sample bank' />
            <Field.ErrorText>{errors.name?.message}</Field.ErrorText>
        </Field.Root>
    </BaseFormModal>
}

export default BankModal;