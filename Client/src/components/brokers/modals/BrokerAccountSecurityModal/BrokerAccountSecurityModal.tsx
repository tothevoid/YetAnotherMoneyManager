import { Button, CloseButton, Dialog, Field, Input, Portal, useDisclosure} from "@chakra-ui/react"
import { forwardRef, useEffect, useImperativeHandle, useState } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import { BrokerAccountSecurityEntity } from "../../../../models/brokers/BrokerAccountSecurityEntity";
import CollectionSelect from "../../../../controls/CollectionSelect/CollectionSelect";
import { getSecurities } from "../../../../api/securities/securityApi";
import { getBrokerAccounts } from "../../../../api/brokers/brokerAccountApi";
import { BrokerAccountEntity } from "../../../../models/brokers/BrokerAccountEntity";
import { SecurityEntity } from "../../../../models/securities/SecurityEntity";
import { BrokerAccountSecurityFormInput, BrokerAccountSecurityValidationSchema } from "./BrokerAccountSecurityValidationSchema";

interface BrokerAccountProps {
    brokerAccountSecurity?: BrokerAccountSecurityEntity | null,
    onSaved: (account: BrokerAccountSecurityEntity) => void;
};

interface State {
    brokerAccounts: BrokerAccountEntity[]
    securities: SecurityEntity[]
}

export interface BrokerAccountModalRef {
    openModal: () => void
}

const BrokerAccountSecurityModal = forwardRef<BrokerAccountModalRef, BrokerAccountProps>((props: BrokerAccountProps, ref)=> {
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

    const { register, handleSubmit, control, formState: { errors }} = useForm<BrokerAccountSecurityFormInput>({
        resolver: zodResolver(BrokerAccountSecurityValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.brokerAccountSecurity?.id ?? crypto.randomUUID(),
            brokerAccount: props.brokerAccountSecurity?.brokerAccount,
            security: props.brokerAccountSecurity?.security,
            initialPrice:  props.brokerAccountSecurity?.initialPrice ?? 0,
            quantity:  props.brokerAccountSecurity?.quantity ?? 0
        }
    });

    useImperativeHandle(ref, () => ({
        openModal: onOpen,
    }));


    const onSubmit = (brokerAccountSecurity: BrokerAccountSecurityFormInput) => {
        props.onSaved(brokerAccountSecurity as BrokerAccountSecurityEntity);
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
                        <Dialog.Title>{t("entity_broker_account_security_form_title")}</Dialog.Title>
                    </Dialog.Header>
                    <Dialog.Body pb={6}>
                        <Field.Root mt={4} invalid={!!errors.brokerAccount}>
                            <Field.Label>{t("entity_broker_account_broker_account")}</Field.Label>
                            <CollectionSelect name="type" control={control} placeholder="Select broker account"
                                collection={state.brokerAccounts} 
                                labelSelector={(accountType => accountType.name)} 
                                valueSelector={(accountType => accountType.id)}/>
                            <Field.ErrorText>{errors.brokerAccount?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root mt={4} invalid={!!errors.security}>
                            <Field.Label>{t("entity_broker_account_security")}</Field.Label>
                            <CollectionSelect name="currency" control={control} placeholder="Select security"
                                collection={state.securities} 
                                labelSelector={(currency => currency.name)} 
                                valueSelector={(currency => currency.id)}/>
                            <Field.ErrorText>{errors.security?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root mt={4} invalid={!!errors.initialPrice}>
                            <Field.Label>{t("entity_broker_initial_price")}</Field.Label>
                            <Input {...register("initialPrice", {valueAsNumber: true})} min={0} autoComplete="off" type='number' placeholder='500' />
                            <Field.ErrorText>{errors.initialPrice?.message}</Field.ErrorText>
					    </Field.Root>
                        <Field.Root mt={4} invalid={!!errors.quantity}>
                            <Field.Label>{t("entity_broker_quantity")}</Field.Label>
                            <Input {...register("quantity", {valueAsNumber: true})} min={0} autoComplete="off" type='number' placeholder='500' />
                            <Field.ErrorText>{errors.quantity?.message}</Field.ErrorText>
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
export default BrokerAccountSecurityModal