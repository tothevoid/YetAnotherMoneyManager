import React, { RefObject, useState } from 'react'
import { useTranslation } from 'react-i18next';
import BaseFormModal from '../../../../shared/modals/BaseFormModal/BaseFormModal';
import { BaseModalRef } from '../../../../shared/utilities/modalUtilities';
import TransactionForm from '../../components/TransactionForm/TransactionForm';
import { TransactionEntity } from '../../../../models/transactions/TransactionEntity';
import { Nullable } from '../../../../shared/utilities/nullable';

interface ModalProps {
	modalRef: RefObject<BaseModalRef | null>,
	transaction: Nullable<TransactionEntity>,
	onSaved: (transaction: TransactionEntity) => Promise<void>
}

interface State {
    formHandler?: React.FormEventHandler
}

const TransactionModal: React.FC<ModalProps> = (props: ModalProps) => {
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
        <TransactionForm transaction={props.transaction} setSubmitHandler={setSubmitHandler} onTransactionSaved={props.onSaved} />
    </BaseFormModal>
}

export default TransactionModal;