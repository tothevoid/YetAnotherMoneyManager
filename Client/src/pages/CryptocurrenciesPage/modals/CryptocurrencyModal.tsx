import { Button, Field, Input, Image, Stack, Box} from "@chakra-ui/react"
import React, { ChangeEvent, Fragment, RefObject, useRef, useState } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import { CryptocurrencyFormInput, CryptocurrencyValidationSchema } from "./CryptocurrencyValidationSchema";
import { MdFileUpload } from "react-icons/md";
import BaseFormModal from "../../../shared/modals/BaseFormModal/BaseFormModal";
import { BaseModalRef } from "../../../shared/utilities/modalUtilities";
import { CryptocurrencyEntity } from "../../../models/crypto/CryptocurrencyEntity";
import { getIconUrl } from "../../../api/crypto/cryptocurrencyApi";

interface ModalProps {
    modalRef: RefObject<BaseModalRef | null>,
    cryptocurrency?: CryptocurrencyEntity | null,
    onSaved: (cryptocurrency: CryptocurrencyEntity, icon: File | null) => void;
};

const CryptocurrencyModal: React.FC<ModalProps> = (props: ModalProps) => {
    const inputRef = useRef<HTMLInputElement | null>(null);

    const defaultIconUrl = getIconUrl(props.cryptocurrency?.iconKey);

    const [icon, setIcon] = useState<File | null>(null);
    const [iconUrl, setIconUrl] = useState<string | null>(defaultIconUrl);

    const { register, handleSubmit, formState: { errors }} = useForm<CryptocurrencyFormInput>({
        resolver: zodResolver(CryptocurrencyValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.cryptocurrency?.id ?? crypto.randomUUID(),
            name: props.cryptocurrency?.name ?? "",
            symbol: props.cryptocurrency?.symbol ?? "",
            price: props.cryptocurrency?.price ?? 0
        }
    });

    const onSubmit = (cryptocurrency: CryptocurrencyFormInput) => {
        props.onSaved(cryptocurrency as CryptocurrencyEntity, icon);
        props.modalRef?.current?.closeModal();
    }

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

    return <BaseFormModal ref={props.modalRef} title={t("entity_security_from_title")} submitHandler={handleSubmit(onSubmit)}>
        <Stack marginBlock={2} gapX={4} alignItems={"center"} direction={"row"}>
            <Box justifyContent={"center"} role="group" position="relative" boxSize="50px">
                <Input type="file" accept="image/*" onChange={handleFileChange} ref={inputRef} display="none" />
                {
                    iconUrl ?
                        <Image src={iconUrl} boxSize="50px"
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
            <Field.Root invalid={!!errors.symbol}>
                <Field.Label>{t("entity_security_ticker")}</Field.Label>
                <Input {...register("symbol")} autoComplete="off" placeholder='BTC' />
                <Field.ErrorText>{errors.symbol?.message}</Field.ErrorText>
            </Field.Root>
        </Stack>
        <Field.Root invalid={!!errors.name}>
            <Field.Label>{t("entity_security_name")}</Field.Label>
            <Input {...register("name")} autoComplete="off" placeholder='Bitcoin' />
            <Field.ErrorText>{errors.name?.message}</Field.ErrorText>
        </Field.Root>
        <Field.Root invalid={!!errors.price} mt={4}>
            <Field.Label>{t("entity_deposit_percentage")}</Field.Label>
            <Input {...register("price", { valueAsNumber: true })} type="number" step="0.01" placeholder='10' />
            <Field.ErrorText>{errors.price?.message}</Field.ErrorText>
        </Field.Root>
    </BaseFormModal>
}

export default CryptocurrencyModal;