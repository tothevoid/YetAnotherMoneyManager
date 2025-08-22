import { Field, Input} from "@chakra-ui/react"
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
import { DividendPaymentEntity } from "../../../../models/brokers/DividendPaymentEntity";
import { getAvailableDividends } from "../../../../api/securities/dividendApi";
import { DividendEntity } from "../../../../models/securities/DividendEntity";
import { formatDate } from "../../../../shared/utilities/formatters/dateFormatter";
import { formatMoneyByCurrencyCulture } from "../../../../shared/utilities/formatters/moneyFormatter";
import { SecurityEntity } from "../../../../models/securities/SecurityEntity";
import { generateGuid } from "../../../../shared/utilities/idUtilities";

export interface CreateDividendPaymentContext {
	brokerAccountId: string
}

export interface EditDividendPaymentContext {
	dividendPayment: DividendPaymentEntity
}

interface ModalProps {
	modalRef: RefObject<BaseModalRef | null>,
	context: CreateDividendPaymentContext | EditDividendPaymentContext,
	onSaved: (account: DividendPaymentEntity) => void;
};

const DividendPaymentModal: React.FC<ModalProps> = (props: ModalProps) => {
	const {t, i18n} = useTranslation();

	const [availableSecurities, setAvailableSecurities] = useState<SecurityEntity[]>([]);
	const [availableDividends, setAvailableDividends] = useState<DividendEntity[]>([]);
	const [dividendsBySecurity, setDividendsBySecurity] = useState<DividendEntity[]>([]);

	const defaultSecurity = "dividendPayment" in props.context ?
		props.context.dividendPayment?.dividend?.security:
		null;

	const [selectedSecurity, setSelectedSecurity] = useState<SecurityEntity | null>(defaultSecurity);
	const [payment, setPayment] = useState<number>(0);
	
	const fetchAvailableDividends = async () => {
		const brokerAccountId = "dividendPayment" in props.context ?
			props.context.dividendPayment.brokerAccount.id:
			props.context.brokerAccountId;

		const dividends = await getAvailableDividends(brokerAccountId);
		setAvailableDividends(dividends);

		const securities = new Map<string, SecurityEntity>();
		dividends.forEach((dividend) => {
			const securityId = dividend.security.id;
			if (securities.has(securityId)) {
				return;
			}

			securities.set(securityId, dividend.security);
		});
		
		setAvailableSecurities([...securities.values()])
	}

	useEffect(() => {
		fetchAvailableDividends();
	}, []);

	useEffect(() => {
		if (!selectedSecurity) {
			setDividendsBySecurity([]);
			return;
		}

		setDividendsBySecurity(availableDividends.filter((dividend) => dividend.security.id === selectedSecurity.id))
	}, [selectedSecurity, availableDividends])

	const dividendPayment = "dividendPayment" in props.context ? props.context.dividendPayment: null;
	const brokerAccount = "brokerAccountId" in props.context ? { id: props.context.brokerAccountId }: { id: undefined };

	const { register, reset, handleSubmit, watch, control, formState: { errors }} = useForm<DividendPaymentFormInput>({
		resolver: zodResolver(DividendPaymentValidationSchema),
		mode: "onBlur",
		defaultValues: {
			id: dividendPayment?.id ?? generateGuid(),
			brokerAccount: dividendPayment?.brokerAccount ?? brokerAccount,
			dividend: dividendPayment?.dividend,
			securitiesQuantity: dividendPayment?.securitiesQuantity ?? 0,
			tax: dividendPayment?.tax ?? 0,
			receivedAt: dividendPayment?.receivedAt ?? new Date(),
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
		props.onSaved(dividendPayment as DividendPaymentEntity);
		props.modalRef?.current?.closeModal();
	}

	const onModalVisibilityChanged = async (open: boolean) => {
		if (!open) {
			return;
		}

		reset();
		setSelectedSecurity(null);
		setPayment(0);
		await fetchAvailableDividends();
	}
  
	return <BaseFormModal ref={props.modalRef} title={t("entity_dividend_payment_form_title")} 
		submitHandler={handleSubmit(onSubmit)} visibilityChanged={onModalVisibilityChanged}>
		<Field.Root mt={4}>
			<Field.Label>{t("dividend_payment_form_security")}</Field.Label>
			<Select value={selectedSecurity} onChange={setSelectedSecurity} 
				getOptionLabel={(security: SecurityEntity) => `${security.name} (${security.ticker})`}
				getOptionValue={(security: SecurityEntity) => security.id}
				options={availableSecurities}/>
		</Field.Root>
		<Field.Root mt={4} invalid={!!errors.dividend}>
			<Field.Label>{t("entity_dividend_payment_dividend")}</Field.Label>
			<CollectionSelect name="dividend" control={control} placeholder="Select security"
				collection={dividendsBySecurity} 
				labelSelector={(dividend: DividendEntity) => 
					`${formatDate(dividend.snapshotDate, i18n)} (${formatMoneyByCurrencyCulture(dividend.amount, dividend.security.currency.name)})`} 
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
		{
			payment > 0 && <Field.Root mt={4}>
				<Field.Label>{t("dividend_payment_form_payment")}</Field.Label>
				<Input disabled value={selectedSecurity ? formatMoneyByCurrencyCulture(payment, selectedSecurity?.currency.name): ""}/>
			</Field.Root>
		}
	</BaseFormModal>
}

export default DividendPaymentModal