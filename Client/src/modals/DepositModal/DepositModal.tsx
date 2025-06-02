import { RefObject, useEffect, useState } from 'react'
import { Button, Field, Input, Dialog, Flex, Portal, CloseButton} from '@chakra-ui/react';
import { DepositEntity } from '../../models/deposits/DepositEntity';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from "react-hook-form";
import { DepositValidationSchema, DepositFormInput } from "./DepositValidationSchema";
import { useTranslation } from "react-i18next";
import DateSelect from "../../controls/DateSelect/DateSelect";
import { CurrencyEntity } from '../../models/currencies/CurrencyEntity';
import CollectionSelect from '../../controls/CollectionSelect/CollectionSelect';
import { getCurrencies } from '../../api/currencies/currencyApi';
import { BaseModalRef } from '../../common/ModalUtilities';
import BaseFormModal from '../../components/common/BaseFormModal';

interface ModalProps {
	modalRef: RefObject<BaseModalRef | null>,
	deposit?: DepositEntity | null,
	onSaved: (deposit: DepositEntity) => void
}

interface State {
	currencies: CurrencyEntity[]
}

const DepositModal: React.FC<ModalProps> = (props: ModalProps) => {

	const [state, setState] = useState<State>({ currencies: [] });

	const initCurrencies = async () => {
		const currencies = await getCurrencies();
		setState((currentState) => {
			return {...currentState, currencies}
		})
	};
	
	useEffect(() => {
		const initData = async () => {
			await initCurrencies();
		}
		initData();
	}, []);

	const { register, control, handleSubmit, formState: { errors }} = useForm<DepositFormInput>({
		resolver: zodResolver(DepositValidationSchema),
		mode: "onBlur",
		defaultValues: {
			id: props.deposit?.id ?? crypto.randomUUID(),
			name: props.deposit?.name ?? "",
			from: props.deposit?.from ?? new Date(),
			to: props.deposit?.to ?? new Date(),
			percentage: props.deposit?.percentage ?? 0,
			initialAmount: props.deposit?.initialAmount ?? 0,
			estimatedEarn: props.deposit?.estimatedEarn ?? 0,
			currency: props.deposit?.account?.currency
		}
	});

	const onSubmit = (deposit: DepositFormInput) => {
		props.onSaved(deposit as DepositEntity);
		props.modalRef?.current?.closeModal();
	}

	const {t} = useTranslation()

	return <BaseFormModal ref={props.modalRef} title={t("entity_deposit_name_form_title")} submitHandler={handleSubmit(onSubmit)}>
		<Field.Root invalid={!!errors.name}>
			<Field.Label>{t("entity_deposit_name")}</Field.Label>
			<Input {...register("name")} autoComplete="off" placeholder='Deposit name' />
			<Field.ErrorText>{errors.name?.message}</Field.ErrorText>
		</Field.Root>
		<Field.Root invalid={!!errors.percentage} mt={4}>
			<Field.Label>{t("entity_deposit_percentage")}</Field.Label>
			<Input {...register("percentage", { valueAsNumber: true })} type="number" step="0.01" placeholder='10' />
			<Field.ErrorText>{errors.percentage?.message}</Field.ErrorText>
		</Field.Root>
		<Field.Root mt={4} invalid={!!errors.currency}>
			<Field.Label>{t("entity_transaction_currency")}</Field.Label>
			<CollectionSelect name="currency" control={control} placeholder="Select currency"
				collection={state.currencies} 
				labelSelector={(currency => currency.name)} 
				valueSelector={(currency => currency.id)}/>
			<Field.ErrorText>{errors.currency?.message}</Field.ErrorText>
		</Field.Root>
		<Field.Root invalid={!!errors.initialAmount} mt={4}>
			<Field.Label>{t("entity_deposit_initial_amount")}</Field.Label>
			<Input {...register("initialAmount", { valueAsNumber: true })} type='number' step="0.01" placeholder='10' />
			<Field.ErrorText>{errors.initialAmount?.message}</Field.ErrorText>
		</Field.Root>
		<Field.Root invalid={!!errors.estimatedEarn} mt={4}>
			<Field.Label>{t("entity_deposit_estimated_earn")}</Field.Label>
			<Input {...register("estimatedEarn", { valueAsNumber: true })} type='number' step="0.01" placeholder='10' />
			<Field.ErrorText>{errors.estimatedEarn?.message}</Field.ErrorText>
		</Field.Root>
		<Flex gap={4} direction="row">
			<Field.Root invalid={!!errors.from} mt={4}>
				<Field.Label>{t("entity_deposit_from")}</Field.Label>
				<DateSelect name="from" control={control}/>
				<Field.ErrorText>{errors.from?.message}</Field.ErrorText>
			</Field.Root>
			<Field.Root invalid={!!errors.to} mt={4}>
				<Field.Label>{t("entity_deposit_to")}</Field.Label>
				{/* TODO Fix date field duplication */}
				<DateSelect name="to" control={control}/>
				<Field.ErrorText>{errors.to?.message}</Field.ErrorText>
			</Field.Root>
		</Flex>
	</BaseFormModal>	
}

export default DepositModal;