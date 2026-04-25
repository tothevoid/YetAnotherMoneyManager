import { Field, Input, Stack } from "@chakra-ui/react";
import React, { RefObject, useCallback, useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import CollectionSelect from "../../../shared/components/CollectionSelect/CollectionSelect";
import { BaseModalRef } from "../../../shared/utilities/modalUtilities";
import BaseFormModal from "../../../shared/modals/BaseFormModal/BaseFormModal";
import { CryptoAccountCryptocurrencyEntity } from "../../../models/crypto/CryptoAccountCryptocurrencyEntity";
import { CryptoAccountCryptocurrencyFormInput, CryptoAccountCryptocurrencyValidationSchema,  } from "./CryptoAccountCryptocurrencyValidationSchema";
import { getCryptocurrencies } from "../../../api/crypto/cryptocurrencyApi";
import { CryptocurrencyEntity } from "../../../models/crypto/CryptocurrencyEntity";

interface ModalProps {
    modalRef: RefObject<BaseModalRef | null>;
    cryptoAccountCryptocurrency?: CryptoAccountCryptocurrencyEntity | null;
    onSaved: (cryptoAccountCryptocurrency: CryptoAccountCryptocurrencyEntity) => void;
}

interface State {
    cryptocurrencies: CryptocurrencyEntity[]
}

const CryptoAccountCryptocurrencyModal: React.FC<ModalProps> = (props: ModalProps) => {
    const { t } = useTranslation();
   
    const getDefaultValues = useCallback(() => {
        return {
            cryptocurrency: props.cryptoAccountCryptocurrency?.cryptocurrency,
            cryptoAccount: props.cryptoAccountCryptocurrency?.cryptoAccount,
            quantity: props.cryptoAccountCryptocurrency?.quantity ?? 0
        };
    }, [props.cryptoAccountCryptocurrency]);

    const [state, setState] = useState<State>({ cryptocurrencies: []});
   
    const initCryptocurrencies = async () => {
        const cryptocurrencies = await getCryptocurrencies();
        setState((currentState: State) => {
            return {...currentState, cryptocurrencies}
        })
    };

    useEffect(() => {
        initCryptocurrencies();
    }, []);

    const { register, handleSubmit, formState: { errors }, reset, control } = useForm<CryptoAccountCryptocurrencyFormInput>({
        resolver: zodResolver(CryptoAccountCryptocurrencyValidationSchema),
        mode: "onBlur",
        defaultValues: getDefaultValues()
    });

    useEffect(() => {
        reset(getDefaultValues());
    }, [props.cryptoAccountCryptocurrency, reset, getDefaultValues]);

    const onSubmit = (data: CryptoAccountCryptocurrencyFormInput) => {
        props.onSaved(data as CryptoAccountCryptocurrencyEntity);
    };

    return (
        <BaseFormModal ref={props.modalRef} title={t("crypto_account_cryptocurrency_modal_title")} submitHandler={handleSubmit(onSubmit)}>
            <Stack>
                <Field.Root mt={4} invalid={!!errors.cryptocurrency}>
                    <Field.Label>{t("crypto_account_cryptocurrency_cryptocurrency")}</Field.Label>
                    <CollectionSelect name="cryptocurrency" control={control} placeholder="Select cryptocurrency"
                        collection={state.cryptocurrencies} 
                        labelSelector={(cryptocurrency: CryptocurrencyEntity) => cryptocurrency.name} 
                        valueSelector={(cryptocurrency: CryptocurrencyEntity) => cryptocurrency.id}/>
                    <Field.ErrorText>{errors.cryptocurrency?.message}</Field.ErrorText>
                </Field.Root>
                <Field.Root invalid={!!errors.quantity} mt={4}>
                    <Field.Label>{t("crypto_account_cryptocurrency_quantity")}</Field.Label>
                    <Input {...register("quantity", { valueAsNumber: true })} type='number' step="0.01" placeholder='10' />
                    <Field.ErrorText>{errors.quantity?.message}</Field.ErrorText>
                </Field.Root>
            </Stack>
        </BaseFormModal>
    );
};

export default CryptoAccountCryptocurrencyModal;
