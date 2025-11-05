import { Field, Input, Stack} from "@chakra-ui/react"
import React, { RefObject, useEffect, useState } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import { SecurityFormInput, SecurityValidationSchema } from "./SecurityValidationSchema";
import { SecurityEntity } from "../../../../models/securities/SecurityEntity";
import CollectionSelect from "../../../../shared/components/CollectionSelect/CollectionSelect";
import { getSecurityTypes } from "../../../../api/securities/securityTypeApi";
import { getIconUrl } from "../../../../api/securities/securityApi";
import { getCurrencies } from "../../../../api/currencies/currencyApi";
import { CurrencyEntity } from "../../../../models/currencies/CurrencyEntity";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import { SecurityTypeEntity } from "../../../../models/securities/SecurityTypeEntity";
import BaseFormModal from "../../../../shared/modals/BaseFormModal/BaseFormModal";
import { generateGuid } from "../../../../shared/utilities/idUtilities";
import ImageInput from "../../../../shared/components/Form/ImageInput/ImageInput";

interface ModalProps {
    modalRef: RefObject<BaseModalRef | null>,
    security?: SecurityEntity | null,
    onSaved: (security: SecurityEntity, icon: File | null) => void;
};

interface State {
    securityTypes: SecurityTypeEntity[]
    currencies: CurrencyEntity[]
}

const SecurityModal: React.FC<ModalProps> = (props: ModalProps) => {
    const [icon, setIcon] = useState<File | null>(null);
    const [iconUrl, setIconUrl] = useState<string | null>(null);
    const [state, setState] = useState<State>({securityTypes: [], currencies: []})

    useEffect(() => {
        const initData = async () => {
            await requestData();
        }
        initData();
    }, []);

    useEffect(() => {
        const url = getIconUrl(props.security?.iconKey);
        setIconUrl(url);
    }, [props.security]);

    const requestData = async () => {
        const securityTypes = await getSecurityTypes();
        const currencies = await getCurrencies();

        setState((currentState) => {
            return {...currentState, securityTypes, currencies }
        })
    };

    const { register, handleSubmit, control, formState: { errors }, reset} = useForm<SecurityFormInput>({
        resolver: zodResolver(SecurityValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.security?.id ?? generateGuid(),
            name: props.security?.name ?? "",
            ticker: props.security?.ticker ?? "",
            type: props.security?.type,
            currency: props.security?.currency
        }
    });

    useEffect(() => {
        if (props.security) {
            reset(props.security);
        }
    }, [props.security, reset])

    const onSubmit = (security: SecurityFormInput) => {
        props.onSaved({ ...security, 
            // TODO: Remove system fields
            iconKey: props.security?.iconKey, 
            priceFetchedAt: props.security?.priceFetchedAt, 
            actualPrice: props.security?.actualPrice } as SecurityEntity, icon);
        props.modalRef?.current?.closeModal();
    }

    const onImageSelected = (url: string, image: File) => {
        setIcon(image);
        setIconUrl(url);
    }
    
    const onVisibilityChanged = (open: boolean) => {
        if (!open) {
            setIcon(null);
            setIconUrl(null);
        }
    }

    const {t} = useTranslation()

    return <BaseFormModal visibilityChanged={onVisibilityChanged} ref={props.modalRef} title={t("entity_security_from_title")} submitHandler={handleSubmit(onSubmit)}>
        <Stack marginBlock={2} gapX={4} alignItems={"center"} direction={"row"}>
            <ImageInput imageUrl={iconUrl} onImageSelected={onImageSelected} />
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
    </BaseFormModal>
}

export default SecurityModal;