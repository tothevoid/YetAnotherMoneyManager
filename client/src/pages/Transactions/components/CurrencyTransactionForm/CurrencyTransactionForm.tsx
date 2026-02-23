import React, { Fragment, useCallback, useEffect, useState } from 'react'
import { Field, Input} from '@chakra-ui/react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { useTranslation } from 'react-i18next';
import { AccountEntity } from '../../../../models/accounts/AccountEntity';
import CollectionSelect from '../../../../shared/components/CollectionSelect/CollectionSelect';
import DateSelect from '../../../../shared/components/DateSelect/DateSelect';
import { CurrencyTransactionFormInput, CurrencyTransactionValidationSchema } from './CurrencyTransactionValidationSchema';
import { getAccounts } from '../../../../api/accounts/accountApi';
import { CurrencyTransactionEntity } from '../../../../models/transactions/CurrencyTransactionEntity';
import { generateGuid } from '../../../../shared/utilities/idUtilities';
import { SetSubmitHandler } from '../../modals/NewTransactionModal/NewTransactionModal';

interface Props {
    currencyTransaction?: CurrencyTransactionEntity
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
            date: props.currencyTransaction?.date ?? new Date(),
            amount: props.currencyTransaction?.amount ?? 0,
            rate: props.currencyTransaction?.rate ?? 0,
            sourceAccount: props.currencyTransaction?.sourceAccount,
            destinationAccount: props.currencyTransaction?.destinationAccount,
        }
    }, [props.currencyTransaction]);

    const { register, handleSubmit, control, formState: { errors }, reset} = useForm<CurrencyTransactionFormInput>({
        resolver: zodResolver(CurrencyTransactionValidationSchema),
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
            <Input {...register("rate", {valueAsNumber: true})} min={0} autoComplete="off" type='number' placeholder='500' step="0.01"/>
            <Field.ErrorText>{errors.rate?.message}</Field.ErrorText>
        </Field.Root>
        <Field.Root mt={4} invalid={!!errors.amount}>
            <Field.Label>{t("entity_currency_transaction_amount")}</Field.Label>
            <Input {...register("amount", {valueAsNumber: true})} min={0} autoComplete="off" type='number' placeholder='500' step="0.01"/>
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