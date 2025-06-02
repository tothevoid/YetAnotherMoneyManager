import React, { useRef } from 'react';
import { AccountEntity } from '../../../models/accounts/AccountEntity';
import { MdOutlineArrowDownward, MdOutlineArrowUpward, MdEdit, MdOutlinePayment } from 'react-icons/md';
import { MdDelete } from "react-icons/md";
import { Flex, Stack, Card, CardBody, Text, Button, Icon, Image } from '@chakra-ui/react';
import TransactionModal from '../../../modals/TransactionModal/TransactionModal';
import { formatMoneyByCurrencyCulture } from '../../../formatters/moneyFormatter';
import { ConfirmModal } from '../../../modals/ConfirmModal/ConfirmModal';
import { deleteTransaction } from '../../../api/transactions/transactionApi';
import { useTranslation } from 'react-i18next';
import { TransactionEntity } from '../../../models/transactions/TransactionEntity';
import { getTransactionTypeIconUrl } from '../../../api/transactions/transactionTypeApi';
import { BaseModalRef } from '../../common/BaseFormModal';


type Props = { 
	onDelete: (transaction: TransactionEntity) => void,
	onUpdate: (updatedTransaction: TransactionEntity) => void,
	transaction: TransactionEntity,
	accounts: AccountEntity[],
} 

const Transaction: React.FC<Props> = (props: Props) => {
	const {amount, transactionType, name, date, account} = props.transaction;

	const confirmModalRef = useRef<BaseModalRef>(null);
 	const editModalRef = useRef<BaseModalRef>(null);

	const onEditClicked = () => {
		editModalRef.current?.openModal()
	};

	const onDeleteClicked = () => {
		confirmModalRef.current?.openModal();
	}

	const onDeletionConfirmed = async () => {
		const isDeleted = await deleteTransaction(props.transaction?.id);
		if (!isDeleted) {
			return;
		}
		props.onDelete(props.transaction);
	}

	const getTransactionDirectionIcon = () => {
		return props.transaction.amount > 0 ?
			<Icon rounded={16} size={"lg"} fontSize="32" background={'green.100'} color={'green.600'}>
				<MdOutlineArrowUpward/>
			</Icon>:
			<Icon rounded={16} size={"lg"} fontSize="32"  background={'red.100'} color={'red.600'}>
				<MdOutlineArrowDownward/>
			</Icon>
	}

	const { i18n, t} = useTranslation()

	const icon = transactionType.iconKey ?
		<Image title={transactionType.name} h={8} w={8} src={getTransactionTypeIconUrl(transactionType.iconKey)}/>:
		<MdOutlinePayment size={32} color="#aaa" />

	return <Card.Root borderColor="border_primary" color="text_primary" backgroundColor="background_primary" 
		mt={5} mb={5} boxShadow={"sm"} _hover={{ boxShadow: "md" }}>
		<CardBody>
			<Flex justifyContent="space-between" alignItems="center">
				<Stack direction={'row'} alignItems="center">
					{icon}
					<Stack direction={"row"} ml={5}>
						<Text fontWeight={700}>{name}</Text>
						<Text>{account.name}</Text>
					</Stack>
				</Stack>
				<Flex gap={2} justifyContent="space-between" alignItems="center">
					{getTransactionDirectionIcon()}
					<Text width={150}>{formatMoneyByCurrencyCulture(amount, account.currency.name)}</Text>
					<Button background={'background_secondary'} size={'sm'} onClick={onEditClicked}>
						<Icon color="card_action_icon_primary">
							<MdEdit/>
						</Icon>
					</Button>
					<Button background={'background_secondary'} size={'sm'} onClick={onDeleteClicked}>
						<Icon color="card_action_icon_danger">
							<MdDelete/>
						</Icon>
					</Button>
				</Flex>
			</Flex>
		</CardBody>	
		<ConfirmModal onConfirmed={onDeletionConfirmed}
			title={t("transaction_delete_title")}
			message={t("modals_delete_message")}
			confirmActionName={t("modals_delete_button")}
			ref={confirmModalRef}>
		</ConfirmModal>
		<TransactionModal
			accounts={props.accounts} 
			transaction={props.transaction} 
			ref={editModalRef} 
			onSaved={props.onUpdate}/>
	</Card.Root>
}

export default Transaction;