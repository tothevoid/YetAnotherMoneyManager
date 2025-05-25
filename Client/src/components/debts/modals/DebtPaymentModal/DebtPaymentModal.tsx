import { Button, CloseButton, Dialog, Field, Input, Portal, useDisclosure} from "@chakra-ui/react"
import { forwardRef, useEffect, useImperativeHandle, useState } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import CollectionSelect from "../../../../controls/CollectionSelect/CollectionSelect";
import { ClientDebtEntity } from "../../../../models/debts/DebtEntity";
import DateSelect from "../../../../controls/DateSelect/DateSelect";
import { DebtPaymentFormInput, DebtPaymentValidationSchema } from "./DebtPaymentValidationSchema";
import { ClientDebtPaymentEntity } from "../../../../models/debts/DebtPaymentEntity";
import { AccountEntity } from "../../../../models/accounts/AccountEntity";
import { getDebts } from "../../../../api/debts/debtApi";
import { getAccounts } from "../../../../api/accounts/accountApi";

interface Props {
    debt?: ClientDebtPaymentEntity | null,
    onSaved: (debt: ClientDebtPaymentEntity) => void;
};

interface State {
    accounts: AccountEntity[],
    debts: ClientDebtEntity[]
}

export interface DebtModalPaymentRef {
    openModal: () => void
}

const DebtPaymentModal = forwardRef<DebtModalPaymentRef, Props>((props: Props, ref)=> {
    const { open, onOpen, onClose } = useDisclosure()  
    const [state, setState] = useState<State>({accounts: [], debts: []})

    useEffect(() => {
        const initData = async () => {
            await requestData();
        }
        initData();
    }, []);

    const requestData = async () => {
        const debts = await getDebts();
        const accounts = await getAccounts(true);

        setState((currentState) => {
            return {...currentState, debts, accounts }
        })
    };

    const { register, handleSubmit, control, formState: { errors }} = useForm<DebtPaymentFormInput>({
        resolver: zodResolver(DebtPaymentValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.debt?.id ?? crypto.randomUUID(),
            amount: props.debt?.amount ?? 0,
            date: props.debt?.date ?? new Date(),
            debt: props.debt?.debt,
            targetAccount: props.debt?.targetAccount
        }
    });

    useImperativeHandle(ref, () => ({
        openModal: onOpen,
    }));


    const onSubmit = (debt: DebtPaymentFormInput) => {
        props.onSaved(debt as ClientDebtPaymentEntity);
        onClose();
    }

    const {t} = useTranslation()


    return (
        <Dialog.Root placement="center" open={open} onEscapeKeyDown={onClose}>
          <Portal>
            <Dialog.Backdrop/>
            <Dialog.Positioner>
                <Dialog.Content as="form" onSubmit={handleSubmit(onSubmit)}>
                    <Dialog.Header>
                        <Dialog.Title>{t("entity_debt_payment_form_title")}</Dialog.Title>
                    </Dialog.Header>
                    <Dialog.Body pb={6}>
                         <Field.Root mt={4} invalid={!!errors.debt}>
                            <Field.Label>{t("entity_debt_payment_debt")}</Field.Label>
                            <CollectionSelect name="debt" control={control} placeholder="Select debt"
                                collection={state.debts} 
                                labelSelector={(debt => debt.name)} 
                                valueSelector={(debt => debt.id)}/>
                            <Field.ErrorText>{errors.debt?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root invalid={!!errors.amount}>
                            <Field.Label>{t("entity_debt_payment_amount")}</Field.Label>
                            <Input {...register("amount", {valueAsNumber: true})} min={0} step="0.01" autoComplete="off" type='number' placeholder='500' />
                            <Field.ErrorText>{errors.amount?.message}</Field.ErrorText>
                        </Field.Root>
                         <Field.Root mt={4} invalid={!!errors.targetAccount}>
                            <Field.Label>{t("entity_debt_payment_target_account")}</Field.Label>
                            <CollectionSelect name="targetAccount" control={control} placeholder="Select account"
                                collection={state.accounts} 
                                labelSelector={(account => account.name)} 
                                valueSelector={(account => account.id)}/>
                            <Field.ErrorText>{errors.targetAccount?.message}</Field.ErrorText>
                        </Field.Root>        
                        <Field.Root mt={4} invalid={!!errors.date}>
                            <Field.Label>{t("entity_debt_payment_date")}</Field.Label>
                            <DateSelect name="date" control={control}/>
                            <Field.ErrorText>{errors.date?.message}</Field.ErrorText>
                        </Field.Root>
                    </Dialog.Body>
                    <Dialog.Footer>
                        <Button type="submit" background='purple.600' mr={3}>{t("modals_save_button")}</Button>
                        <Button onClick={onClose}>{t("modals_cancel_button")}</Button>
                    </Dialog.Footer>
                    <Dialog.CloseTrigger asChild>
                        <CloseButton onClick={onClose} size="sm" />
                    </Dialog.CloseTrigger>
                </Dialog.Content>
            </Dialog.Positioner>
          </Portal>
        </Dialog.Root>
    )
})
export default DebtPaymentModal;