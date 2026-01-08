import { Field, Input} from "@chakra-ui/react"
import { RefObject, useCallback, useEffect, useState } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import CollectionSelect from "../../../../shared/components/CollectionSelect/CollectionSelect";
import { getSecurities } from "../../../../api/securities/securityApi";
import { getBrokerAccounts } from "../../../../api/brokers/brokerAccountApi";
import { BrokerAccountEntity } from "../../../../models/brokers/BrokerAccountEntity";
import { SecurityEntity } from "../../../../models/securities/SecurityEntity";
import { SecurityTransactionFormInput, SecurityTransactionValidationSchema } from "./SecurityTransactionValidationSchema";
import DateSelect from "../../../../shared/components/DateSelect/DateSelect";
import { SecurityTransactionEntity } from "../../../../models/securities/SecurityTransactionEntity";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import BaseFormModal from "../../../../shared/modals/BaseFormModal/BaseFormModal";
import { generateGuid } from "../../../../shared/utilities/idUtilities";

export interface CreateSecurityTransactionContext {
	brokerAccountId: string
}

export interface EditSecurityTransactionContext {
	securityTransaction: SecurityTransactionEntity
}

interface ModalProps {
	isGlobalBrokerAccount: boolean
	modalRef: RefObject<BaseModalRef | null>
	onSaved: (account: SecurityTransactionEntity) => void
	context: CreateSecurityTransactionContext | EditSecurityTransactionContext
};

interface State {
	brokerAccounts: BrokerAccountEntity[]
	securities: SecurityEntity[]
}

const SecurityTransactionModal: React.FC<ModalProps> = (props: ModalProps) => {
	const [state, setState] = useState<State>({ brokerAccounts: [], securities: []});
	const { t } = useTranslation()
	
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

	const getFormDefaultValues = useCallback(() => {
		const securityTransaction = "securityTransaction" in props.context ? props.context.securityTransaction: null;
		const brokerAccount = "brokerAccountId" in props.context ? { id: props.context.brokerAccountId }: { id: undefined};
		
		return {
			id: securityTransaction?.id ?? generateGuid(),
			brokerAccount: securityTransaction?.brokerAccount ?? brokerAccount,
			security: securityTransaction?.security,
			brokerCommission: securityTransaction?.brokerCommission ?? 0,
			stockExchangeCommission: securityTransaction?.stockExchangeCommission ?? 0,
			date: securityTransaction?.date ?? new Date(),
			price: securityTransaction?.price ?? 0,
			tax: securityTransaction?.tax ?? 0,
			quantity: securityTransaction?.quantity ?? 0
		}
	}, [props.context])

	const { register, handleSubmit, control, formState: { errors }, reset} = useForm<SecurityTransactionFormInput>({
		resolver: zodResolver(SecurityTransactionValidationSchema),
		mode: "onBlur",
		defaultValues: getFormDefaultValues()
	});

	useEffect(() => {
		reset(getFormDefaultValues());
	}, [reset, getFormDefaultValues, props.context]);

	const onSubmit = (securityTransaction: SecurityTransactionFormInput) => {
		props.onSaved(securityTransaction as SecurityTransactionEntity);
		props.modalRef?.current?.closeModal();
	}

	return <BaseFormModal ref={props.modalRef} title={t("entity_security_transaction_form_title")} submitHandler={handleSubmit(onSubmit)}>
		{
			props.isGlobalBrokerAccount && <Field.Root mt={4} invalid={!!errors.brokerAccount}>
				<Field.Label>{t("entity_security_transaction_broker_account")}</Field.Label>
				<CollectionSelect name="brokerAccount" control={control} placeholder="Select broker account"
					collection={state.brokerAccounts} 
					labelSelector={(brokerAccount => brokerAccount.name)} 
					valueSelector={(brokerAccount => brokerAccount.id)}/>
				<Field.ErrorText>{errors.brokerAccount?.message}</Field.ErrorText>
			</Field.Root>
		}
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
			<DateSelect name="date" control={control} isDateTime={true}/>
			<Field.ErrorText>{errors.date?.message}</Field.ErrorText>
		</Field.Root>
		<Field.Root mt={4} invalid={!!errors.price}>
			<Field.Label>{t("entity_security_transaction_price")}</Field.Label>
			<Input {...register("price", {valueAsNumber: true})} min={0} step="0.01" autoComplete="off" type='number' placeholder='100' />
			<Field.ErrorText>{errors.price?.message}</Field.ErrorText>
		</Field.Root>
		<Field.Root mt={4} invalid={!!errors.quantity}>
			<Field.Label>{t("entity_security_transaction_quantity")}</Field.Label>
			<Input {...register("quantity", {valueAsNumber: true})} min={0} autoComplete="off" type='number' placeholder='100' />
			<Field.ErrorText>{errors.quantity?.message}</Field.ErrorText>
		</Field.Root>
		<Field.Root mt={4} invalid={!!errors.brokerCommission}>
			<Field.Label>{t("entity_security_transaction_broker_commission")}</Field.Label>
			<Input {...register("brokerCommission", {valueAsNumber: true})} min={0} step="0.01" autoComplete="off" type='number' placeholder='500' />
			<Field.ErrorText>{errors.brokerCommission?.message}</Field.ErrorText>
		</Field.Root>
		<Field.Root mt={4} invalid={!!errors.stockExchangeCommission}>
			<Field.Label>{t("entity_security_transaction_stock_exchange_commission")}</Field.Label>
			<Input {...register("stockExchangeCommission", {valueAsNumber: true})} min={0} step="0.01" autoComplete="off" type='number' placeholder='500' />
			<Field.ErrorText>{errors.stockExchangeCommission?.message}</Field.ErrorText>
		</Field.Root>
		<Field.Root mt={4} invalid={!!errors.tax}>
			<Field.Label>{t("entity_security_transaction_tax")}</Field.Label>
			<Input {...register("tax", {valueAsNumber: true})} min={0} step="0.01" autoComplete="off" type='number' placeholder='500' />
			<Field.ErrorText>{errors.tax?.message}</Field.ErrorText>
		</Field.Root>
	</BaseFormModal>
}

export default SecurityTransactionModal