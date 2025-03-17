import React, { useRef } from 'react';
import { TransactionEntity } from '../../models/TransactionEntity';
import { AccountEntity } from '../../models/AccountEntity';
import { MdOutlineArrowDownward, MdOutlineArrowUpward, MdEdit } from 'react-icons/md';
import { MdDelete } from "react-icons/md";
import { Flex, Stack, Card, CardBody, Text, Button, Icon } from '@chakra-ui/react';
import TransactionModal, { TransactionModalRef } from '../../modals/TransactionModal/TransactionModal';
import { formatMoney } from '../../formatters/moneyFormatter';
import { formatDate } from '../../formatters/dateFormatter';
import { ConfirmModal, ConfirmModalRef } from '../../modals/ConfirmModal/ConfirmModal';
import { deleteTransaction } from '../../api/transactionApi';
import { useTranslation } from 'react-i18next';


type Props = { 
	onDelete: (transaction: TransactionEntity) => void,
	onUpdate: (updatedTransaction: TransactionEntity) => void,
	transaction: TransactionEntity,
	fundSources: AccountEntity[],
} 

const Transaction: React.FC<Props> = (props: Props) => {
	const {moneyQuantity, transactionType, name, date, fundSource} = props.transaction;

	const confirmModalRef = useRef<ConfirmModalRef>(null);
 	const editModalRef = useRef<TransactionModalRef>(null);

	const onEditClicked = () => {
		editModalRef.current?.openModal()
	};

	const onDeleteClicked = () => {
		confirmModalRef.current?.openModal();
	}

	const onDeletionConfirmed = async () => {
		const isDeleted = await deleteTransaction(props.transaction);
		if (!isDeleted) {
			return;
		}
		props.onDelete(props.transaction);
	}

	const getTransactionDirectionIcon = () => {
		return props.transaction.moneyQuantity > 0 ?
			<Icon rounded={16} fontSize="32"  background={'green.100'} color={'green.600'}>
				<MdOutlineArrowUpward/>
			</Icon>:
			<Icon rounded={16} fontSize="32"  background={'red.100'} color={'red.600'}>
				<MdOutlineArrowDownward/>
			</Icon>
	}

	const { i18n, t} = useTranslation()

	return <Card.Root mt={5} mb={5} boxShadow={"sm"} _hover={{ boxShadow: "md" }}>
		<CardBody>
			<Flex justifyContent="space-between" alignItems="center">
				<Stack direction={'row'} alignItems="center">
					{getTransactionDirectionIcon()}
					<Stack ml={5}>
						<Text fontWeight={700}>{name}</Text>
						<Text>{formatDate(date, i18n, false)} • {fundSource.name}</Text>
					</Stack>
				</Stack>
				<Flex gap={2} justifyContent="space-between" alignItems="center">
					<Text background={'green.100'} textAlign={'center'} w={150} rounded={10} padding={1} >{transactionType}</Text>
					<Text width={100}>{formatMoney(moneyQuantity)}</Text>
					<Button background={'white'} size={'sm'} onClick={onEditClicked}>
						<Icon color="blackAlpha.800">
							<MdEdit/>
						</Icon>
					</Button>
					<Button background={'white'} size={'sm'} onClick={onDeleteClicked}>
						<Icon color="red.600">
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
			fundSources={props.fundSources} 
			transaction={props.transaction} 
			ref={editModalRef} 
			onSaved={props.onUpdate}/>
	</Card.Root>
}

export default Transaction;