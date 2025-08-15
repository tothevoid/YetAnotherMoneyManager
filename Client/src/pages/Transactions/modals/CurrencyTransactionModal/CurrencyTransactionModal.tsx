import React, { RefObject, useState } from 'react'
import { useTranslation } from 'react-i18next';
import BaseFormModal from '../../../../shared/modals/BaseFormModal/BaseFormModal';
import { BaseModalRef } from '../../../../shared/utilities/modalUtilities';
import CurrencyTransactionForm from '../../components/CurrencyTransactionForm/CurrencyTransactionForm';
import { CurrencyTransactionEntity } from '../../../../models/transactions/CurrencyTransactionEntity';

interface ModalProps {
    modalRef: RefObject<BaseModalRef | null>,
    currencyTransaction?: CurrencyTransactionEntity,
    onSaved: (transaction: CurrencyTransactionEntity) => Promise<void>
}

interface State {
    formHandler?: React.FormEventHandler
}

const CurrencyTransactionModal: React.FC<ModalProps> = (props: ModalProps) => {
    const {t} = useTranslation();

    const [state, setState] = useState<State>({});

    const setSubmitHandler = (handler: React.FormEventHandler) => {
        setState((currentState) => {
            return {...currentState, formHandler: handler}
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