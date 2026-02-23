import React, { RefObject, useState } from 'react'
import { useTranslation } from 'react-i18next';
import BaseFormModal from '../../../../shared/modals/BaseFormModal/BaseFormModal';
import { BaseModalRef } from '../../../../shared/utilities/modalUtilities';
import CurrencyTransactionForm from '../../components/CurrencyTransactionForm/CurrencyTransactionForm';
import { CurrencyTransactionEntity } from '../../../../models/transactions/CurrencyTransactionEntity';
import { FieldValues, UseFormHandleSubmit } from 'react-hook-form';
import { SetSubmitHandler } from '../NewTransactionModal/NewTransactionModal';

interface ModalProps {
    modalRef: RefObject<BaseModalRef | null>,
    currencyTransaction?: CurrencyTransactionEntity | null,
    onSaved: (transaction: CurrencyTransactionEntity) => Promise<void>
}

interface State {
    formHandler?: React.FormEventHandler
}

const CurrencyTransactionModal: React.FC<ModalProps> = (props: ModalProps) => {
    const {t} = useTranslation();

    const [state, setState] = useState<State>({});

    const setSubmitHandler: SetSubmitHandler = async <T extends FieldValues>(submit: UseFormHandleSubmit<T>, handler: (data: T) => Promise<void>) => {
        const wrappedHandler = async (data: T) => {
            await handler(data);
            props.modalRef?.current?.closeModal();
        }


        setState((currentState) => {
            return {...currentState, formHandler: submit(wrappedHandler)}
        })
    }
    
    const onSubmit = (event: React.FormEvent) => {
        if (!state.formHandler) {
            return;
        }

        state.formHandler(event);
        props.modalRef?.current?.closeModal();
    }

    return <BaseFormModal ref={props.modalRef} title={t("entity_transaction_name_form_title")} submitHandler={onSubmit}>
        <CurrencyTransactionForm currencyTransaction={props.currencyTransaction} setSubmitHandler={setSubmitHandler} onCurrencyTransactionSaved={props.onSaved} />
    </BaseFormModal>
}

export default CurrencyTransactionModal;