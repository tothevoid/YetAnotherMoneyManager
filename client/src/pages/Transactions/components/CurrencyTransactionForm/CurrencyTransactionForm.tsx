import React, { Fragment, useCallback, useEffect, useMemo, useState } from 'react'
import { Field, Input } from '@chakra-ui/react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { useTranslation } from 'react-i18next';
import { AccountEntity } from '../../../../models/accounts/AccountEntity';
import CollectionSelect from '../../../../shared/components/CollectionSelect/CollectionSelect';
import DateSelect from '../../../../shared/components/DateSelect/DateSelect';
import MoneyInput from '../../../../shared/components/MoneyInput/MoneyInput';
import { CurrencyTransactionFormInput, getCurrencyTransactionValidationSchema } from './CurrencyTransactionValidationSchema';
import { getAccounts } from '../../../../api/accounts/accountApi';
import { CurrencyTransactionEntity } from '../../../../models/transactions/CurrencyTransactionEntity';
import { generateGuid } from '../../../../shared/utilities/idUtilities';
import { SetSubmitHandler } from '../../modals/NewTransactionModal/NewTransactionModal';

interface Props {
    currencyTransaction?: CurrencyTransactionEntity | null
    setSubmitHandler: SetSubmitHandler,
    onCurrencyTransactionSaved: (currencyTransaction: CurrencyTransactionEntity) => Promise<void>
}

interface State {
    accounts: AccountEntity[]
}

const CurrencyTransactionForm: React.FC<Props> = (props: Props) => {
    const {t} = useTranslation();

    const getDefaultTransactionFormState = useCallback(() => {
        return {
            id: props.currencyTransaction?.id ?? generateGuid(),
            name: props.currencyTransaction?.name ?? "",
            date: props.currencyTransaction?.date ?? new Date(),
            amount: props.currencyTransaction?.amount ?? 0,
            rate: props.currencyTransaction?.rate ?? 0,
            sourceAccount: props.currencyTransaction?.sourceAccount,
            destinationAccount: props.currencyTransaction?.destinationAccount,
        }
    }, [props.currencyTransaction]);

    const validationSchema = useMemo(() => getCurrencyTransactionValidationSchema(t), [t]);

    const { register, handleSubmit, control, watch, formState: { errors }, reset} = useForm<CurrencyTransactionFormInput>({
        resolver: zodResolver(validationSchema),
        mode: "onBlur",
        defaultValues: getDefaultTransactionFormState()
    });

    useEffect(() => {
        reset(getDefaultTransactionFormState());
    }, [props.currencyTransaction, reset, getDefaultTransactionFormState])

    const [state, setState] = useState<State>({accounts: []});

    const initAccounts = async () => {
        const accounts = await getAccounts(true);
        setState((currentState) => {
            return {...currentState, accounts}
        })
    };
    
    const onCurrencyTransactionSaveClick = async (currencyTransaction: CurrencyTransactionFormInput) => {
        const formData = currencyTransaction as CurrencyTransactionEntity;
        await props.onCurrencyTransactionSaved(formData);
    };

    useEffect(() => {
        const initData = async () => {
            await initAccounts();
        }
        initData();
    }, []);

   useEffect(() => {
        props.setSubmitHandler(handleSubmit, onCurrencyTransactionSaveClick);
    }, [state]);

    const selectedSourceAccount = watch("sourceAccount");
    const sourceCurrency = state.accounts.find(a => a.id === selectedSourceAccount?.id)?.currency?.name ?? '';
    const selectedDestAccount = watch("destinationAccount");
    const destCurrency = state.accounts.find(a => a.id === selectedDestAccount?.id)?.currency?.name ?? '';

    return <Fragment>
        <Field.Root mt={4} invalid={!!errors.name}>
            <Field.Label>{t("entity_currency_transaction_name")}</Field.Label>
            <Input {...register("name")} placeholder="Enter transaction name"/>
            <Field.ErrorText>{errors.name?.message}</Field.ErrorText>
        </Field.Root>
        <Field.Root mt={4} invalid={!!errors.sourceAccount}>
            <Field.Label>{t("entity_currency_transaction_source_account")}</Field.Label>
            <CollectionSelect name="sourceAccount" control={control} placeholder="Select source account"
                collection={state.accounts} 
                labelSelector={(account => account.name)} 
                valueSelector={(account => account.id)}/>
            <Field.ErrorText>{errors.sourceAccount?.message}</Field.ErrorText>
        </Field.Root>
        <Field.Root mt={4} invalid={!!errors.destinationAccount}>
            <Field.Label>{t("entity_currency_transaction_destination_account")}</Field.Label>
            <CollectionSelect name="destinationAccount" control={control} placeholder="Select destination account"
                collection={state.accounts} 
                labelSelector={(account => account.name)} 
                valueSelector={(account => account.id)}/>
            <Field.ErrorText>{errors.destinationAccount?.message}</Field.ErrorText>
        </Field.Root>
        <Field.Root mt={4} invalid={!!errors.rate}>
            <Field.Label>{t("entity_currency_transaction_rate")}</Field.Label>
            <MoneyInput name="rate" control={control} currency={destCurrency || sourceCurrency} decimalScale={4} placeholder='1.00'/>
            <Field.ErrorText>{errors.rate?.message}</Field.ErrorText>
        </Field.Root>
        <Field.Root mt={4} invalid={!!errors.amount}>
            <Field.Label>{t("entity_currency_transaction_amount")}</Field.Label>
            <MoneyInput name="amount" control={control} currency={sourceCurrency} placeholder='500'/>
            <Field.ErrorText>{errors.amount?.message}</Field.ErrorText>
        </Field.Root>
        <Field.Root mt={4} invalid={!!errors.date}>
            <Field.Label>{t("entity_currency_transaction_date")}</Field.Label>
            <DateSelect name="date" control={control}/>
            <Field.ErrorText>{errors.date?.message}</Field.ErrorText>
        </Field.Root>
    </Fragment>
}

export default CurrencyTransactionForm;