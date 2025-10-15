import { Button, Field, Input, Image, Stack, Box} from "@chakra-ui/react"
import React, { ChangeEvent, Fragment, RefObject, useEffect, useRef, useState } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import { CryptocurrencyFormInput, CryptocurrencyValidationSchema } from "./CryptocurrencyValidationSchema";
import { MdFileUpload } from "react-icons/md";
import BaseFormModal from "../../../shared/modals/BaseFormModal/BaseFormModal";
import { BaseModalRef } from "../../../shared/utilities/modalUtilities";
import { CryptocurrencyEntity } from "../../../models/crypto/CryptocurrencyEntity";
import { getIconUrl } from "../../../api/crypto/cryptocurrencyApi";
import { generateGuid } from "../../../shared/utilities/idUtilities";

interface ModalProps {
    modalRef: RefObject<BaseModalRef | null>,
    cryptocurrency?: CryptocurrencyEntity | null,
    onSaved: (cryptocurrency: CryptocurrencyEntity, icon: File | null) => void;
};

const CryptocurrencyModal: React.FC<ModalProps> = (props: ModalProps) => {
    const inputRef = useRef<HTMLInputElement | null>(null);

    const [icon, setIcon] = useState<File | null>(null);
    const [iconUrl, setIconUrl] = useState<string | null>(null);

    const { register, handleSubmit, formState: { errors }, reset} = useForm<CryptocurrencyFormInput>({
        resolver: zodResolver(CryptocurrencyValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.cryptocurrency?.id ?? generateGuid(),
            name: props.cryptocurrency?.name ?? "",
            symbol: props.cryptocurrency?.symbol ?? "",
            price: props.cryptocurrency?.price ?? 0
        }
    });

    useEffect(() => {
        if (props.cryptocurrency) {
            reset(props.cryptocurrency);
        }
    }, [props.cryptocurrency, reset]);

    useEffect(() => {
       const url = getIconUrl(props.cryptocurrency?.iconKey);
       setIconUrl(url);
    }, [props.cryptocurrency]);

    const onSubmit = (cryptocurrency: CryptocurrencyFormInput) => {
        props.onSaved({ ...cryptocurrency, iconKey: props.cryptocurrency?.iconKey } as CryptocurrencyEntity, icon);
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

    return <BaseFormModal ref={props.modalRef} title={t("cryptocurrency_form_title")} submitHandler={handleSubmit(onSubmit)}>
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
            <Field.Root invalid={!!errors.symbol}>
                <Field.Label>{t("cryptocurrency_form_symbol")}</Field.Label>
                <Input {...register("symbol")} autoComplete="off" placeholder='BTC' />
                <Field.ErrorText>{errors.symbol?.message}</Field.ErrorText>
            </Field.Root>
        </Stack>
        <Field.Root invalid={!!errors.name}>
            <Field.Label>{t("cryptocurrency_form_name")}</Field.Label>
            <Input {...register("name")} autoComplete="off" placeholder='Bitcoin' />
            <Field.ErrorText>{errors.name?.message}</Field.ErrorText>
        </Field.Root>
        <Field.Root invalid={!!errors.price} mt={4}>
            <Field.Label>{t("cryptocurrency_form_price")}</Field.Label>
            <Input {...register("price", { valueAsNumber: true })} type="number" step="0.01" placeholder='10' />
            <Field.ErrorText>{errors.price?.message}</Field.ErrorText>
        </Field.Root>
    </BaseFormModal>
}

export default CryptocurrencyModal;