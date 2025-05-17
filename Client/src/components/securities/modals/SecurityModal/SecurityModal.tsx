import { Button, CloseButton, Dialog, Field, Input, Portal, Image, useDisclosure, Stack, Text, Box} from "@chakra-ui/react"
import { ChangeEvent, forwardRef, Fragment, useEffect, useImperativeHandle, useRef, useState } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import { SecurityFormInput, SecurityValidationSchema } from "./SecurityValidationSchema";
import { SecurityEntity } from "../../../../models/securities/SecurityEntity";
import CollectionSelect from "../../../../controls/CollectionSelect/CollectionSelect";
import { getSecurityTypes } from "../../../../api/securities/securityTypeApi";
import { MdFileUpload } from "react-icons/md";
import { getIconUrl } from "../../../../api/securities/securityApi";
import { getCurrencies } from "../../../../api/currencies/currencyApi";
import { CurrencyEntity } from "../../../../models/currencies/CurrencyEntity";

interface SecuirtyModalProps {
    security?: SecurityEntity | null,
    onSaved: (security: SecurityEntity, icon: File | null) => void;
};

interface State {
    securityTypes: SecurityEntity[]
    currencies: CurrencyEntity[]
}

export interface SecurityModalRef {
    openModal: () => void
}

const SecurityModal = forwardRef<SecurityModalRef, SecuirtyModalProps>((props: SecuirtyModalProps, ref)=> {
    const { open, onOpen, onClose } = useDisclosure()

    const inputRef = useRef<HTMLInputElement | null>(null);

    const defaultIconUrl = getIconUrl(props.security?.iconKey);

    const [icon, setIcon] = useState<File | null>(null);
    const [iconUrl, setIconUrl] = useState<string | null>(defaultIconUrl);
    const [state, setState] = useState<State>({securityTypes: [], currencies: []})

    useEffect(() => {
        const initData = async () => {
            await requestData();
        }
        initData();
    }, []);

    const requestData = async () => {
        const securityTypes = await getSecurityTypes();
        const currencies = await getCurrencies();

        setState((currentState) => {
            return {...currentState, securityTypes, currencies }
        })
    };

    const { register, handleSubmit, control, formState: { errors }} = useForm<SecurityFormInput>({
        resolver: zodResolver(SecurityValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.security?.id ?? crypto.randomUUID(),
            name: props.security?.name ?? "",
            ticker: props.security?.ticker ?? "",
            type: props.security?.type,
            currency: props.security?.currency
        }
    });

    useImperativeHandle(ref, () => ({
        openModal: onOpen,
    }));


    const onSubmit = (security: SecurityFormInput) => {
        props.onSaved(security as SecurityEntity, icon);
        onClose();
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

    return (
        <Dialog.Root placement="center" open={open} onEscapeKeyDown={onClose}>
          <Portal>
            <Dialog.Backdrop/>
            <Dialog.Positioner>
                <Dialog.Content as="form" onSubmit={handleSubmit(onSubmit)}>
                    <Dialog.Header>
                        <Dialog.Title>{t("entity_security_from_title")}</Dialog.Title>
                    </Dialog.Header>
                    <Dialog.Body pb={6}>
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
                            <Field.Root invalid={!!errors.ticker}>
                                <Field.Label>{t("entity_security_ticker")}</Field.Label>
                                <Input {...register("ticker")} autoComplete="off" placeholder='NVDA' />
                                <Field.ErrorText>{errors.ticker?.message}</Field.ErrorText>
                            </Field.Root>
                        </Stack>
                        <Field.Root invalid={!!errors.name}>
                            <Field.Label>{t("entity_security_name")}</Field.Label>
                            <Input {...register("name")} autoComplete="off" placeholder='Debit card' />
                            <Field.ErrorText>{errors.name?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root mt={4} invalid={!!errors.type}>
                            <Field.Label>{t("entity_security_type")}</Field.Label>
                            <CollectionSelect name="type" control={control} placeholder="Select type"
                                collection={state.securityTypes} 
                                labelSelector={(currency => currency.name)} 
                                valueSelector={(currency => currency.id)}/>
                            <Field.ErrorText>{errors.type?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root mt={4} invalid={!!errors.currency}>
                            <Field.Label>{t("entity_security_currency")}</Field.Label>
                            <CollectionSelect name="currency" control={control} placeholder="Select currency"
                                collection={state.currencies} 
                                labelSelector={(currency => currency.name)} 
                                valueSelector={(currency => currency.id)}/>
                            <Field.ErrorText>{errors.currency?.message}</Field.ErrorText>
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
export default SecurityModal;