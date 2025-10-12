import { Field, Input } from "@chakra-ui/react";
import React, { RefObject, useCallback, useEffect, useMemo, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import BaseFormModal from "../../../../shared/modals/BaseFormModal/BaseFormModal";
import { getAccounts } from "../../../../api/accounts/accountApi";
import { AccountEntity } from "../../../../models/accounts/AccountEntity";
import CollectionSelect from "../../../../shared/components/CollectionSelect/CollectionSelect";
import { useTranslation } from "react-i18next";
import { BrokerAccountFundTransferEntity } from "../../../../models/brokers/BrokerAccountFundTransfer";
import { generateGuid } from "../../../../shared/utilities/idUtilities";
import DateSelect from "../../../../shared/components/DateSelect/DateSelect";
import { BrokerAccountFundTransferFormInput, BrokerAccountFundTransferValidationSchema } from "./BrokerAccountFundTransferValidationSchema";

export interface CreateBrokerAccountFundTransferContext {
    brokerAccountId: string
}

export interface EditBrokerAccountFundTransferContext {
    brokerAccountFundTransfer: BrokerAccountFundTransferEntity
}

interface ModalProps {
    onSaved: (transfer: BrokerAccountFundTransferEntity) => void
    context: CreateBrokerAccountFundTransferContext | EditBrokerAccountFundTransferContext
    modalRef: RefObject<BaseModalRef | null>}

interface TransferType {
    label: string;
    value: boolean;
}

const BrokerAccountFundTransferModal: React.FC<ModalProps> = (props: ModalProps) => {
    const [accounts, setAccounts] = useState<AccountEntity[]>([]);

    const { i18n, t } = useTranslation();

    const transferTypes: TransferType[] = useMemo(
        () => [
            { label: t("broker_account_transfer_modal_operation_deposit"), value: true },
            { label: t("broker_account_transfer_modal_operation_withdraw"), value: false }
        ],
        [i18n.language, t]
    );
    
    const getDefaultValues = useCallback(() => {
        const brokerAccountFundTransfer = "brokerAccountFundTransfer" in props.context ? props.context.brokerAccountFundTransfer: null;
		
        const brokerAccount = "brokerAccountId" in props.context ? 
            { id: props.context.brokerAccountId }: 
            { id: props.context.brokerAccountFundTransfer.brokerAccount.id};
        
        const transferType = transferTypes.find(tt => tt.value === brokerAccountFundTransfer?.income) ?? transferTypes[0]
        
        return {
            id: brokerAccountFundTransfer?.id ?? generateGuid(),
            account: brokerAccountFundTransfer?.account,
            brokerAccount: brokerAccountFundTransfer?.brokerAccount ?? brokerAccount,
            income: transferType,
            date: brokerAccountFundTransfer?.date ?? new Date(),
            amount: brokerAccountFundTransfer?.amount ?? 0
        }
    }, [props.context, transferTypes]);

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
        defaultValues: getDefaultValues()
    });

    const incomeValue = watch("income"); 
    const [accountLabel, setAccountLabel] = useState<string>("");
    useEffect(() => {
        setAccountLabel(incomeValue.value ? t("broker_account_transfer_modal_account_from") : t("broker_account_transfer_modal_account_to"));
    }, [t, incomeValue]);

    useEffect(() => {
        reset(getDefaultValues());
    }, [reset, getDefaultValues, props.context]);

    const onSubmit = async (formData: BrokerAccountFundTransferFormInput) => {
        const transfer = {...formData, income: formData.income.value} as BrokerAccountFundTransferEntity;
        props.onSaved(transfer);
        props.modalRef.current?.closeModal();
    };

    const onModalVisibilityChanged = async (open: boolean) => {
        if (!open) {
            return;
        }

        reset(getDefaultValues());
    }

    return (
        <BaseFormModal ref={props.modalRef} title={t("broker_account_transfer_modal_title")}
            visibilityChanged={onModalVisibilityChanged}
            submitHandler={handleSubmit(onSubmit)} 
            saveButtonTitle={t("broker_account_transfer_modal_transfer_button")}>
            <Field.Root mt={4} invalid={!!errors.income}>
                <Field.Label>{t("broker_account_transfer_modal_operation")}</Field.Label>
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
                <Field.Label>{t("broker_account_transfer_modal_date")}</Field.Label>
                <DateSelect name="date" control={control} isDateTime={true}/>
                <Field.ErrorText>{errors.date?.message}</Field.ErrorText>
            </Field.Root>
            <Field.Root mt={4} invalid={!!errors.amount}>
                <Field.Label>{t("broker_account_transfer_modal_amount")}</Field.Label>
                <Input {...register("amount", { valueAsNumber: true })} name="amount" type="number" placeholder="10000" />
                <Field.ErrorText>{errors.amount?.message}</Field.ErrorText>
            </Field.Root>
        </BaseFormModal>
    );
};

export default BrokerAccountFundTransferModal;
