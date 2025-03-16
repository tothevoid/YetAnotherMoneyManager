import React, { useRef } from 'react';
import { TransactionEntity } from '../../models/TransactionEntity';
import { FundEntity } from '../../models/FundEntity';
import { MdOutlineArrowDownward, MdOutlineArrowUpward, MdEdit } from 'react-icons/md';
import { MdDelete } from "react-icons/md";
import { Flex, Stack, Card, CardBody, Text, Button, Icon } from '@chakra-ui/react';
import TransactionModal, { TransactionModalRef } from '../../modals/TransactionModal/TransactionModal';
import { formatMoney } from '../../formatters/moneyFormatter';
import { formatDate } from '../../formatters/dateFormatter';
import { ConfirmModal, ConfirmModalRef } from '../../modals/ConfirmModal/ConfirmModal';


type Props = { 
	onDelete: (transaction: TransactionEntity) => void,
	onUpdate: (updatedTransaction: TransactionEntity) => void,
	transaction: TransactionEntity,
	fundSources: FundEntity[],
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

	const onDeletionConfirmed = () => {
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

	return <Card.Root mt={5} mb={5} boxShadow={"sm"} _hover={{ boxShadow: "md" }}>
		<CardBody>
			<Flex justifyContent="space-between" alignItems="center">
				<Stack direction={'row'} alignItems="center">
					{getTransactionDirectionIcon()}
					<Stack ml={5}>
						<Text fontWeight={700}>{name}</Text>
						<Text>{formatDate(date, false)} • {fundSource.name}</Text>
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
			title="Delete transaction"
			message="Are you sure? You can't undo this action afterwards."
			confirmActionName="Delete"
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