import { Field, Input } from "@chakra-ui/react";
import React, { RefObject, useCallback, useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { BrokerAccountEntity } from "../../../../models/brokers/BrokerAccountEntity";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import BaseFormModal from "../../../../shared/modals/BaseFormModal/BaseFormModal";
import { TopUpBrokerAccountFormInput, TopUpBrokerAccountValidationSchema } from "./TopUpBrokerAccountValidationSchema";
import { getAccounts } from "../../../../api/accounts/accountApi";
import { AccountEntity } from "../../../../models/accounts/AccountEntity";
import CollectionSelect from "../../../../shared/components/CollectionSelect/CollectionSelect";
import { useTranslation } from "react-i18next";
import { BrokerAccountFundTransferEntity } from "../../../../models/brokers/BrokerAccountFundTransfer";
import { createBrokerAccountFundsTransfer } from "../../../../api/brokers/BrokerAccountFundsTransferApi";
import { generateGuid } from "../../../../shared/utilities/idUtilities";
import DateSelect from "../../../../shared/components/DateSelect/DateSelect";
import { Nullable } from "../../../../shared/utilities/nullable";

interface ModalProps {
    brokerAccount: Nullable<BrokerAccountEntity>
    modalRef: Nullable<RefObject<BaseModalRef>>
    onDeposited: () => void;
}

const TopUpBrokerAccountModal: React.FC<ModalProps> = (props: ModalProps) => {
    const setDefaultValues = useCallback(() => {
        return {
            id: generateGuid(),
            account: {},
            brokerAccount: props.brokerAccount ?? {},
            income: true,
            date: new Date(),
            amount: 0
        }
    }, [props.brokerAccount]);

    const [accounts, setAccounts] = useState<AccountEntity[]>([]);

    useEffect(() => {
        const fetchData = async () => {
            // TODO: Filter by account type and currency
            const accounts = await getAccounts(true);
            setAccounts(accounts);
        }

        fetchData()
    }, [])
    
    
    const { register, handleSubmit, control, formState: { errors }, reset } = useForm<TopUpBrokerAccountFormInput>({
        resolver: zodResolver(TopUpBrokerAccountValidationSchema),
        defaultValues: setDefaultValues()
    });

    useEffect(() => {
        reset(setDefaultValues());
    }, [reset, setDefaultValues]);

    const onSubmit = async (data: TopUpBrokerAccountFormInput) => {
        await createBrokerAccountFundsTransfer(data as BrokerAccountFundTransferEntity)
        props.onDeposited();
    };

    const { t } = useTranslation();


    return (
        <BaseFormModal ref={props.modalRef} title={t("top_up_broker_account_modal_title")} 
            submitHandler={handleSubmit(onSubmit)} 
            saveButtonTitle={t("top_up_broker_account_modal_top_up_button")}>
            <Field.Root mt={4} invalid={!!errors.account}>
                <Field.Label>{t("top_up_broker_account_modal_account")}</Field.Label>
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

export default TopUpBrokerAccountModal;
