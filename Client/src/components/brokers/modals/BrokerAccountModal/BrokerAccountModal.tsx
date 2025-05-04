import { Button, CloseButton, Dialog, Field, Input, Portal, useDisclosure} from "@chakra-ui/react"
import { forwardRef, useEffect, useImperativeHandle, useState } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import { getBrokers } from "../../../../api/brokers/brokerApi";
import { BrokerAccountEntity } from "../../../../models/brokers/BrokerAccountEntity";
import { CurrencyEntity } from "../../../../models/currencies/CurrencyEntity";
import { BrokerAccountTypeEntity } from "../../../../models/brokers/BrokerAccountTypeEntity";
import { BrokerEntity } from "../../../../models/brokers/BrokerEntity";
import { getCurrencies } from "../../../../api/currencies/currencyApi";
import CollectionSelect from "../../../../controls/CollectionSelect/CollectionSelect";
import { BrokerAccountFormInput, BrokerAccountValidationSchema } from "./BrokerAccountValidationSchema";
import { getBrokerAccountTypes } from "../../../../api/brokers/brokerAccountTypeApi";

interface BrokerAccountProps {
    brokerAccount?: BrokerAccountEntity | null,
    onSaved: (account: BrokerAccountEntity) => void;
};

interface State {
    currencies: CurrencyEntity[]
    accountTypes: BrokerAccountTypeEntity[]
    brokers: BrokerEntity[]
}

export interface BrokerAccountModalRef {
    openModal: () => void
}

const BrokerAccountModal = forwardRef<BrokerAccountModalRef, BrokerAccountProps>((props: BrokerAccountProps, ref)=> {
    const [state, setState] = useState<State>({currencies: [], accountTypes: [], brokers: []})
    
    useEffect(() => {
        const initData = async () => {
            await requestData();
        }
        initData();
    }, []);

    const requestData = async () => {
        const currencies = await getCurrencies();
        const accountTypes = await getBrokerAccountTypes();
        const brokers = await getBrokers();
        setState((currentState) => {
            return {...currentState, currencies, accountTypes, brokers}
        })
    };

    const { open, onOpen, onClose } = useDisclosure()

    const { register, handleSubmit, control, formState: { errors }} = useForm<BrokerAccountFormInput>({
        resolver: zodResolver(BrokerAccountValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.brokerAccount?.id ?? crypto.randomUUID(),
            name: props.brokerAccount?.name ?? "",
            type: props.brokerAccount?.type,
            currency: props.brokerAccount?.currency,
            broker: props.brokerAccount?.broker,
            assetsValue: props.brokerAccount?.assetsValue,
        }
    });

    useImperativeHandle(ref, () => ({
        openModal: onOpen,
    }));


    const onSubmit = (brokerAccount: BrokerAccountFormInput) => {
        props.onSaved(brokerAccount as BrokerAccountEntity);
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
                        <Dialog.Title>{t("entity_broker_name_form_title")}</Dialog.Title>
                    </Dialog.Header>
                    <Dialog.Body pb={6}>
                        <Field.Root invalid={!!errors.name}>
                            <Field.Label>{t("entity_broker_account_name")}</Field.Label>
                            <Input {...register("name")} autoComplete="off" placeholder='Debit card' />
                            <Field.ErrorText>{errors.name?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root mt={4} invalid={!!errors.type}>
                            <Field.Label>{t("entity_broker_account_type")}</Field.Label>
                            <CollectionSelect name="type" control={control} placeholder="Select broker account type"
                                collection={state.accountTypes} 
                                labelSelector={(accountType => accountType.name)} 
                                valueSelector={(accountType => accountType.id)}/>
                            <Field.ErrorText>{errors.type?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root mt={4} invalid={!!errors.currency}>
                            <Field.Label>{t("entity_broker_account_currency")}</Field.Label>
                            <CollectionSelect name="currency" control={control} placeholder="Select currency"
                                collection={state.currencies} 
                                labelSelector={(currency => currency.name)} 
                                valueSelector={(currency => currency.id)}/>
                            <Field.ErrorText>{errors.currency?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root mt={4} invalid={!!errors.broker}>
                            <Field.Label>{t("entity_broker_account_broker")}</Field.Label>
                            <CollectionSelect name="broker" control={control} placeholder="Select broker"
                                collection={state.brokers} 
                                labelSelector={(broker => broker.name)} 
                                valueSelector={(broker => broker.id)}/>
                            <Field.ErrorText>{errors.broker?.message}</Field.ErrorText>
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
export default BrokerAccountModal