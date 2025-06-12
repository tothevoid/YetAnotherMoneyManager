import { Field, Input, Text} from "@chakra-ui/react"
import { Select } from "chakra-react-select";
import { RefObject, useEffect, useState } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import CollectionSelect from "../../../../shared/components/CollectionSelect/CollectionSelect";
import DateSelect from "../../../../shared/components/DateSelect/DateSelect";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import BaseFormModal from "../../../../shared/modals/BaseFormModal/BaseFormModal";
import { DividendPaymentFormInput, DividendPaymentValidationSchema } from "./DividendPaymentValidationSchema";
import { ClientDividendPaymentEntity } from "../../../../models/brokers/DividendPaymentEntity";
import { getAvailableDividends } from "../../../../api/securities/dividendApi";
import { DividendEntity } from "../../../../models/securities/DividendEntity";
import { formatDate } from "../../../../shared/utilities/formatters/dateFormatter";
import { formatMoneyByCurrencyCulture } from "../../../../shared/utilities/formatters/moneyFormatter";
import { useUserProfile } from "../../../../../features/UserProfileSettingsModal/hooks/UserProfileContext";
import { SecurityEntity } from "../../../../models/securities/SecurityEntity";

interface ModalProps {
    modalRef: RefObject<BaseModalRef | null>,
    dividendPayment: ClientDividendPaymentEntity,
    onSaved: (account: ClientDividendPaymentEntity) => void;
};

interface State {
    dividends: DividendEntity[]
}

const DividendPaymentModal: React.FC<ModalProps> = (props: ModalProps) => {
    const [state, setState] = useState<State>({ dividends: []});
    const {t, i18n} = useTranslation();
    const { user } = useUserProfile(); 

    const [securities, setSecurities] = useState<SecurityEntity[]>([]);
    const [selectedSecurity, setSelectedSecurity] = useState<SecurityEntity | null>(null);

    const [specificDividends, setSpecificDividends] = useState<DividendEntity[]>([]);

    const [payment, setPayment] = useState<number>(0);
    
    useEffect(() => {
        const initData = async () => {
            const dividends = await getAvailableDividends(props.dividendPayment?.brokerAccount?.id);
            setState({dividends});

            const securities = new Map<string, SecurityEntity>();
            dividends.forEach((dividend) => {
                const securityId = dividend.security.id;
                if (securities.has(securityId)) {
                    return;
                }

                securities.set(securityId, dividend.security);
            });
            
            setSecurities([...securities.values()])
        }
        initData();
    }, []);

    useEffect(() => {
        if (!selectedSecurity) {
            setSpecificDividends([]);
            return;
        }

        setSpecificDividends(state.dividends.filter((dividend) => dividend.security.id === selectedSecurity.id))
    }, [selectedSecurity, state.dividends])

    const { register, handleSubmit, watch, control, formState: { errors }} = useForm<DividendPaymentFormInput>({
        resolver: zodResolver(DividendPaymentValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.dividendPayment?.id ?? crypto.randomUUID(),
            brokerAccount: props.dividendPayment.brokerAccount,
            dividend: props.dividendPayment.dividend,
            securitiesQuantity: props.dividendPayment.securitiesQuantity ?? 0,
            tax: props.dividendPayment?.tax ?? 0,
            receivedAt: props.dividendPayment?.receivedAt ?? new Date(),
        }
    });

    const securitiesQuantity = watch('securitiesQuantity');
    const dividend = watch('dividend');
    const tax = watch('tax');

    useEffect(() => {
        if (!securitiesQuantity || !dividend) {
            return;
        }

        setPayment(securitiesQuantity * dividend.amount - tax)
    }, [securitiesQuantity, dividend, tax])

    const onSubmit = (dividendPayment: DividendPaymentFormInput) => {
        props.onSaved(dividendPayment as ClientDividendPaymentEntity);
        props.modalRef?.current?.closeModal();
    }
  
    return <BaseFormModal ref={props.modalRef} title={t("entity_dividend_payment_form_title")} submitHandler={handleSubmit(onSubmit)}>
         <Field.Root mt={4}>
            <Field.Label>{t("dividend_payment_form_security")}</Field.Label>
            <Select value={selectedSecurity} onChange={setSelectedSecurity} 
                getOptionLabel={(security: SecurityEntity) => `${security.name} (${security.ticker})`}
                getOptionValue={(security: SecurityEntity) => security.id}
                options={securities}/>
        </Field.Root>
        <Field.Root mt={4} invalid={!!errors.dividend}>
            <Field.Label>{t("entity_dividend_payment_dividend")}</Field.Label>
            <CollectionSelect name="dividend" control={control} placeholder="Select security"
                collection={specificDividends} 
                labelSelector={(dividend: DividendEntity) => 
                    `${formatDate(dividend.paymentDate, i18n)} (${formatMoneyByCurrencyCulture(dividend.amount, dividend.security.currency.name)})`} 
                valueSelector={(dividend => dividend.id)}/>
            <Field.ErrorText>{errors.dividend?.message}</Field.ErrorText>
        </Field.Root>
        <Field.Root mt={4} invalid={!!errors.securitiesQuantity}>
            <Field.Label>{t("entity_dividend_payment_securities_quantity")}</Field.Label>
            <Input {...register("securitiesQuantity", {valueAsNumber: true})} min={0} autoComplete="off" type='number' placeholder='100' />
            <Field.ErrorText>{errors.securitiesQuantity?.message}</Field.ErrorText>
        </Field.Root>
        <Field.Root mt={4} invalid={!!errors.tax}>
            <Field.Label>{t("entity_dividend_payment_tax")}</Field.Label>
            <Input {...register("tax", {valueAsNumber: true})} min={0} step="0.01" autoComplete="off" type='number' placeholder='500' />
            <Field.ErrorText>{errors.tax?.message}</Field.ErrorText>
        </Field.Root>
        <Field.Root mt={4} invalid={!!errors.receivedAt}>
            <Field.Label>{t("entity_dividend_received_at")}</Field.Label>
            <DateSelect name="receivedAt" control={control}/>
            <Field.ErrorText>{errors.receivedAt?.message}</Field.ErrorText>
        </Field.Root>
        <Field.Root hidden={!payment} mt={4}>
            <Field.Label>{t("dividend_payment_form_payment")}</Field.Label>
            <Input disabled value={formatMoneyByCurrencyCulture(payment, user?.currency.name)}/>
        </Field.Root>
    </BaseFormModal>
}

export default DividendPaymentModal