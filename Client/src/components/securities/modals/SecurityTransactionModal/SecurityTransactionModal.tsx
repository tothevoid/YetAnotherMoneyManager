import { Button, CloseButton, Dialog, Field, Input, Portal, useDisclosure} from "@chakra-ui/react"
import { forwardRef, useEffect, useImperativeHandle, useState } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import CollectionSelect from "../../../../controls/CollectionSelect/CollectionSelect";
import { getSecurities } from "../../../../api/securities/securityApi";
import { getBrokerAccounts } from "../../../../api/brokers/brokerAccountApi";
import { BrokerAccountEntity } from "../../../../models/brokers/BrokerAccountEntity";
import { SecurityEntity } from "../../../../models/securities/SecurityEntity";
import { SecurityTransactionFormInput, SecurityTransactionValidationSchema } from "./SecurityTransactionValidationSchema";
import DateSelect from "../../../../controls/DateSelect/DateSelect";
import { SecurityTransactionEntity } from "../../../../models/securities/SecurityTransactionEntity";

interface BrokerAccountProps {
    securityTransaction?: SecurityTransactionEntity | null,
    onSaved: (account: SecurityTransactionEntity) => void;
};

interface State {
    brokerAccounts: BrokerAccountEntity[]
    securities: SecurityEntity[]
}

export interface BrokerAccountModalRef {
    openModal: () => void
}

const SecurityTransactionModal = forwardRef<BrokerAccountModalRef, BrokerAccountProps>((props: BrokerAccountProps, ref)=> {
    const [state, setState] = useState<State>({ brokerAccounts: [], securities: []})
    
    useEffect(() => {
        const initData = async () => {
            await requestData();
        }
        initData();
    }, []);

    const requestData = async () => {
        const brokerAccounts = await getBrokerAccounts();
        const securities = await getSecurities();
        setState((currentState) => {
            return {...currentState, brokerAccounts, securities}
        })
    };

    const { open, onOpen, onClose } = useDisclosure()

    const { register, handleSubmit, control, formState: { errors }} = useForm<SecurityTransactionFormInput>({
        resolver: zodResolver(SecurityTransactionValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.securityTransaction?.id ?? crypto.randomUUID(),
            brokerAccount: props.securityTransaction?.brokerAccount,
            security: props.securityTransaction?.security,
            commission: props.securityTransaction?.commission ?? 0,
            date: props.securityTransaction?.date ?? new Date(),
            price: props.securityTransaction?.price ?? 0,
            tax: props.securityTransaction?.tax ?? 0,
        }
    });

    useImperativeHandle(ref, () => ({
        openModal: onOpen,
    }));


    const onSubmit = (securityTransaction: SecurityTransactionFormInput) => {
        props.onSaved(securityTransaction as SecurityTransactionEntity);
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
                        <Dialog.Title>{t("entity_security_transaction_form_title")}</Dialog.Title>
                    </Dialog.Header>
                    <Dialog.Body pb={6}>
                        <Field.Root mt={4} invalid={!!errors.brokerAccount}>
                            <Field.Label>{t("entity_security_transaction_broker_account")}</Field.Label>
                            <CollectionSelect name="brokerAccount" control={control} placeholder="Select broker account"
                                collection={state.brokerAccounts} 
                                labelSelector={(brokerAccount => brokerAccount.name)} 
                                valueSelector={(brokerAccount => brokerAccount.id)}/>
                            <Field.ErrorText>{errors.brokerAccount?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root mt={4} invalid={!!errors.security}>
                            <Field.Label>{t("entity_security_transaction_security")}</Field.Label>
                            <CollectionSelect name="security" control={control} placeholder="Select security"
                                collection={state.securities} 
                                labelSelector={(security => security.name)} 
                                valueSelector={(security => security.id)}/>
                            <Field.ErrorText>{errors.security?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root mt={4} invalid={!!errors.date}>
						    <Field.Label>{t("entity_security_transaction_date")}</Field.Label>
                            <DateSelect name="date" control={control}/>
                            <Field.ErrorText>{errors.date?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root mt={4} invalid={!!errors.quantity}>
                            <Field.Label>{t("entity_security_transaction_quantity")}</Field.Label>
                            <Input {...register("quantity", {valueAsNumber: true})} min={0} autoComplete="off" type='number' placeholder='100' />
                            <Field.ErrorText>{errors.quantity?.message}</Field.ErrorText>
					    </Field.Root>
                        <Field.Root mt={4} invalid={!!errors.price}>
                            <Field.Label>{t("entity_security_transaction_price")}</Field.Label>
                            <Input {...register("price", {valueAsNumber: true})} min={0} autoComplete="off" type='number' placeholder='100' />
                            <Field.ErrorText>{errors.price?.message}</Field.ErrorText>
					    </Field.Root>
                        <Field.Root mt={4} invalid={!!errors.commission}>
                            <Field.Label>{t("entity_security_transaction_commission")}</Field.Label>
                            <Input {...register("commission", {valueAsNumber: true})} min={0} autoComplete="off" type='number' placeholder='500' />
                            <Field.ErrorText>{errors.commission?.message}</Field.ErrorText>
					    </Field.Root>
                        <Field.Root mt={4} invalid={!!errors.tax}>
                            <Field.Label>{t("entity_security_transaction_tax")}</Field.Label>
                            <Input {...register("tax", {valueAsNumber: true})} min={0} autoComplete="off" type='number' placeholder='500' />
                            <Field.ErrorText>{errors.tax?.message}</Field.ErrorText>
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
export default SecurityTransactionModal