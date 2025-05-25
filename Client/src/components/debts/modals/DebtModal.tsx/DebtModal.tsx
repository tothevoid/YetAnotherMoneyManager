import { Button, CloseButton, Dialog, Field, Input, Portal, useDisclosure} from "@chakra-ui/react"
import { forwardRef, useEffect, useImperativeHandle, useState } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import CollectionSelect from "../../../../controls/CollectionSelect/CollectionSelect";
import { getCurrencies } from "../../../../api/currencies/currencyApi";
import { CurrencyEntity } from "../../../../models/currencies/CurrencyEntity";
import { DebtFormInput, DebtValidationSchema } from "./DebtValidationSchema";
import { ClientDebtEntity } from "../../../../models/debts/DebtEntity";
import DateSelect from "../../../../controls/DateSelect/DateSelect";

interface Props {
    debt?: ClientDebtEntity | null,
    onSaved: (debt: ClientDebtEntity) => void;
};

interface State {
    currencies: CurrencyEntity[]
}

export interface DebtModalRef {
    openModal: () => void
}

const DebtModal = forwardRef<DebtModalRef, Props>((props: Props, ref)=> {
    const { open, onOpen, onClose } = useDisclosure()  
    const [state, setState] = useState<State>({currencies: []})

    useEffect(() => {
        const initData = async () => {
            await requestData();
        }
        initData();
    }, []);

    const requestData = async () => {
        const currencies = await getCurrencies();

        setState((currentState) => {
            return {...currentState, currencies }
        })
    };

    const { register, handleSubmit, control, formState: { errors }} = useForm<DebtFormInput>({
        resolver: zodResolver(DebtValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.debt?.id ?? crypto.randomUUID(),
            name: props.debt?.name ?? "",
            amount: props.debt?.amount ?? 0,
            currency: props.debt?.currency,
            date: props.debt?.date
        }
    });

    useImperativeHandle(ref, () => ({
        openModal: onOpen,
    }));


    const onSubmit = (debt: DebtFormInput) => {
        props.onSaved(debt as ClientDebtEntity);
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
                        <Dialog.Title>{t("entity_debt_form_title")}</Dialog.Title>
                    </Dialog.Header>
                    <Dialog.Body pb={6}>
                        <Field.Root invalid={!!errors.name}>
                            <Field.Label>{t("entity_debt_name")}</Field.Label>
                            <Input {...register("name")} autoComplete="off" placeholder='Debit card' />
                            <Field.ErrorText>{errors.name?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root invalid={!!errors.amount}>
                            <Field.Label>{t("entity_debt_amount")}</Field.Label>
                            <Input {...register("amount", {valueAsNumber: true})} min={0} step="0.01" autoComplete="off" type='number' placeholder='500' />
                            <Field.ErrorText>{errors.amount?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root mt={4} invalid={!!errors.currency}>
                            <Field.Label>{t("entity_debt_currency")}</Field.Label>
                            <CollectionSelect name="currency" control={control} placeholder="Select type"
                                collection={state.currencies} 
                                labelSelector={(currency => currency.name)} 
                                valueSelector={(currency => currency.id)}/>
                            <Field.ErrorText>{errors.currency?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root mt={4} invalid={!!errors.date}>
						    <Field.Label>{t("entity_debt_date")}</Field.Label>
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
export default DebtModal;