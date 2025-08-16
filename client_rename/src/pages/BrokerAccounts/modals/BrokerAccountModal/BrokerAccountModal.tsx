import { Field, Input} from "@chakra-ui/react"
import { RefObject, useEffect, useState } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import { getBrokers } from "../../../../api/brokers/brokerApi";
import { BrokerAccountEntity } from "../../../../models/brokers/BrokerAccountEntity";
import { CurrencyEntity } from "../../../../models/currencies/CurrencyEntity";
import { BrokerAccountTypeEntity } from "../../../../models/brokers/BrokerAccountTypeEntity";
import { BrokerEntity } from "../../../../models/brokers/BrokerEntity";
import { getCurrencies } from "../../../../api/currencies/currencyApi";
import CollectionSelect from "../../../../shared/components/CollectionSelect/CollectionSelect";
import { BrokerAccountFormInput, BrokerAccountValidationSchema } from "./BrokerAccountValidationSchema";
import { getBrokerAccountTypes } from "../../../../api/brokers/brokerAccountTypeApi";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import BaseFormModal from "../../../../shared/modals/BaseFormModal/BaseFormModal";
import { generateGuid } from "../../../../shared/utilities/idUtilities";

interface ModalProps {
    modalRef: RefObject<BaseModalRef | null>
    brokerAccount?: BrokerAccountEntity | null,
    onSaved: (account: BrokerAccountEntity) => void;
};

interface State {
    currencies: CurrencyEntity[]
    accountTypes: BrokerAccountTypeEntity[]
    brokers: BrokerEntity[]
}

const BrokerAccountModal: React.FC<ModalProps> = (props: ModalProps) => {
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

    const { register, handleSubmit, control, formState: { errors }} = useForm<BrokerAccountFormInput>({
        resolver: zodResolver(BrokerAccountValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.brokerAccount?.id ?? generateGuid(),
            name: props.brokerAccount?.name ?? "",
            type: props.brokerAccount?.type,
            currency: props.brokerAccount?.currency,
            broker: props.brokerAccount?.broker,
            initialValue: props.brokerAccount?.initialValue,
            currentValue: props.brokerAccount?.currentValue,
            mainCurrencyAmount: props.brokerAccount?.mainCurrencyAmount ?? 0
        }
    });

    const onSubmit = (brokerAccount: BrokerAccountFormInput) => {
        props.onSaved(brokerAccount as BrokerAccountEntity);
        props.modalRef?.current?.closeModal();
    }

    const {t} = useTranslation()

    return <BaseFormModal ref={props.modalRef} title={t("entity_broker_name_form_title")} submitHandler={handleSubmit(onSubmit)}>
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
        <Field.Root mt={4} invalid={!!errors.mainCurrencyAmount}>
            <Field.Label>{t("entity_broker_account_main_currency_amount")}</Field.Label>
            <Input {...register("mainCurrencyAmount", { valueAsNumber: true })} type='number' step="0.01" placeholder='10' />
            <Field.ErrorText>{errors.mainCurrencyAmount?.message}</Field.ErrorText>
        </Field.Root>
        <Field.Root mt={4} invalid={!!errors.broker}>
            <Field.Label>{t("entity_broker_account_broker")}</Field.Label>
            <CollectionSelect name="broker" control={control} placeholder="Select broker"
                collection={state.brokers} 
                labelSelector={(broker => broker.name)} 
                valueSelector={(broker => broker.id)}/>
            <Field.ErrorText>{errors.broker?.message}</Field.ErrorText>
        </Field.Root>
    </BaseFormModal>
}
export default BrokerAccountModal