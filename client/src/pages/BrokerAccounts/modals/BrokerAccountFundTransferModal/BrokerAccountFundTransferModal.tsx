import { Field, Input } from "@chakra-ui/react";
import React, { RefObject, useCallback, useEffect, useMemo, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { BrokerAccountEntity } from "../../../../models/brokers/BrokerAccountEntity";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import BaseFormModal from "../../../../shared/modals/BaseFormModal/BaseFormModal";
import { getAccounts } from "../../../../api/accounts/accountApi";
import { AccountEntity } from "../../../../models/accounts/AccountEntity";
import CollectionSelect from "../../../../shared/components/CollectionSelect/CollectionSelect";
import { useTranslation } from "react-i18next";
import { BrokerAccountFundTransferEntity } from "../../../../models/brokers/BrokerAccountFundTransfer";
import { createBrokerAccountFundsTransfer } from "../../../../api/brokers/BrokerAccountFundsTransferApi";
import { generateGuid } from "../../../../shared/utilities/idUtilities";
import DateSelect from "../../../../shared/components/DateSelect/DateSelect";
import { Nullable } from "../../../../shared/utilities/nullable";
import { BrokerAccountFundTransferFormInput, BrokerAccountFundTransferValidationSchema } from "./BrokerAccountFundTransferValidationSchema";

interface ModalProps {
    brokerAccount: Nullable<BrokerAccountEntity>
    modalRef: RefObject<BaseModalRef | null>;
    onDeposited: () => void;
}

interface TransferType {
    label: string;
    value: boolean;
}

const BrokerAccountFundTransferModal: React.FC<ModalProps> = (props: ModalProps) => {
    const [accounts, setAccounts] = useState<AccountEntity[]>([]);

    const { i18n, t } = useTranslation();

    const transferTypes: TransferType[] = useMemo(
        () => [
            { label: t("top_up_broker_account_modal_operation_deposit"), value: true },
            { label: t("top_up_broker_account_modal_operation_withdraw"), value: false }
        ],
        [i18n.language, t]
    );
    
    const setDefaultValues = useCallback(() => {
        return {
            id: generateGuid(),
            account: {},
            brokerAccount: props.brokerAccount ?? {},
            income: transferTypes[0],
            date: new Date(),
            amount: 0
        }
    }, [props.brokerAccount, transferTypes]);

    useEffect(() => {
        const fetchData = async () => {
            // TODO: Filter by account type and currency
            const accounts = await getAccounts(true);
            setAccounts(accounts);
        }
        fetchData()
    }, [])

    const { register, handleSubmit, control, formState: { errors }, reset, watch } = useForm<BrokerAccountFundTransferFormInput>({
        resolver: zodResolver(BrokerAccountFundTransferValidationSchema),
        defaultValues: setDefaultValues()
    });

    const incomeValue = watch("income"); 
    const [accountLabel, setAccountLabel] = useState<string>("");
    useEffect(() => {
        setAccountLabel(incomeValue.value ? t("top_up_broker_account_modal_account_from") : t("top_up_broker_account_modal_account_to"));
    }, [t, incomeValue]);

    useEffect(() => {
        reset(setDefaultValues());
    }, [reset, setDefaultValues]);

    const onSubmit = async (formData: BrokerAccountFundTransferFormInput) => {
        const transfer = {...formData, income: formData.income.value} as BrokerAccountFundTransferEntity
        await createBrokerAccountFundsTransfer(transfer)
        props.onDeposited();
        props.modalRef.current?.closeModal();
    };

    return (
        <BaseFormModal ref={props.modalRef} title={t("top_up_broker_account_modal_title")} 
            submitHandler={handleSubmit(onSubmit)} 
            saveButtonTitle={t("top_up_broker_account_modal_transfer_button")}>
            <Field.Root mt={4} invalid={!!errors.income}>
                <Field.Label>{t("top_up_broker_account_modal_operation")}</Field.Label>
                <CollectionSelect name="income" control={control} placeholder="Select type"
                    collection={transferTypes}
                    labelSelector={(transferType => transferType.label)}
                    valueSelector={(transferType => transferType.value)} />
                <Field.ErrorText>{errors.income?.message}</Field.ErrorText>
            </Field.Root>
            <Field.Root mt={4} invalid={!!errors.account}>
                <Field.Label>{accountLabel}</Field.Label>
                <CollectionSelect name="account" control={control} placeholder="Select account"
                    collection={accounts}
                    labelSelector={(account => account.name)}
                    valueSelector={(account => account.id)} />
                <Field.ErrorText>{errors.account?.message}</Field.ErrorText>
            </Field.Root>
            <Field.Root invalid={!!errors.date} mt={4}>
                <Field.Label>{t("top_up_broker_account_modal_date")}</Field.Label>
                <DateSelect name="date" control={control}/>
                <Field.ErrorText>{errors.date?.message}</Field.ErrorText>
            </Field.Root>
            <Field.Root mt={4} invalid={!!errors.amount}>
                <Field.Label>{t("top_up_broker_account_modal_amount")} ({props.brokerAccount?.currency.name})</Field.Label>
                <Input {...register("amount", { valueAsNumber: true })} name="amount" type="number" placeholder="10000" />
                <Field.ErrorText>{errors.amount?.message}</Field.ErrorText>
            </Field.Root>
        </BaseFormModal>
    );
};

export default BrokerAccountFundTransferModal;
