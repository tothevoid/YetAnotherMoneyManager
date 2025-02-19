import React, { useRef } from 'react';
import { TransactionEntity } from '../../models/TransactionEntity';
import { FundEntity } from '../../models/FundEntity';
import { ArrowDownIcon, ArrowUpIcon, DeleteIcon, EditIcon } from '@chakra-ui/icons';
import { Flex, Stack, Card, CardBody, Text, Button, AlertDialog, AlertDialogOverlay, 
	AlertDialogContent, AlertDialogBody, AlertDialogFooter, AlertDialogHeader, useDisclosure } from '@chakra-ui/react';
import TransactionModal, { TransactionModalRef } from '../../modals/TransactionModal/TransactionModal';
import { TransactionType } from '../../models/TransactionType';
import { formatMoney } from '../../formatters/moneyFormatter';
import { formatDate } from '../../formatters/dateFormatter';


type Props = { 
	onDelete: (transaction: TransactionEntity) => void,
	onUpdate: (updatedTransaction: TransactionEntity) => void,
	transaction: TransactionEntity,
	fundSources: FundEntity[],
	transactionTypes: TransactionType[]
} 


const Transaction: React.FC<Props> = (props: Props) => {
	// const getURL = (transactionType: TransactionType) => 
	// 	`${config.api.URL}/images/${transactionType.id}.${transactionType.extension}`
	const {moneyQuantity, transactionType, name, date, fundSource} = props.transaction;

	const { isOpen, onOpen, onClose } = useDisclosure();

	const cancelRef = React.useRef<HTMLButtonElement>(null!);

 	const editModalRef = useRef<TransactionModalRef>(null);

	const onEditClicked = () => {
		editModalRef.current?.openModal()
	};

	const onDeleteClicked = () => {
		props.onDelete(props.transaction);
	}

	const getTransactionDirectionIcon = () => {
		return props.transaction.moneyQuantity > 0 ?
			<ArrowUpIcon rounded={16} fontSize="32"  background={'green.100'} color={'green.600'}/>:
			<ArrowDownIcon rounded={16} fontSize="32"  background={'red.100'} color={'red.600'}/>
	}

	return <Card mt={5} mb={5} boxShadow={"sm"} _hover={{ boxShadow: "md" }}>
		<CardBody>
			<Flex justifyContent="space-between" alignItems="center">
				{/* {
					transactionType ? 
						<img src={getURL(transactionType)} alt={name} className="transaction-icon"></img>:
						<div className="transaction-icon-placeholder"></div>
				} */}
				<Stack direction={'row'} alignItems="center">
					{getTransactionDirectionIcon()}
					<Stack ml={5}>
						<Text fontWeight={700}>{name}</Text>
						<Text>{formatDate(date, false)} • {fundSource.name}</Text>
					</Stack>
				</Stack>
				<Flex gap={2} justifyContent="space-between" alignItems="center">
					<Text background={'green.100'} textAlign={'center'} w={150} rounded={10} padding={1} >{transactionType?.name}</Text>
					<Text width={100} align={"right"} >{formatMoney(moneyQuantity)}</Text>
					<Button background={'white'} size={'sm'} onClick={onEditClicked}>
						<EditIcon/>
					</Button>
					<Button background={'white'} size={'sm'} onClick={onOpen}>
						<DeleteIcon color={"red.600"}/>
					</Button>
				</Flex>
			</Flex>
		</CardBody>
		{/* TODO: Remove duplication. (Fund.tsx) */}
		<AlertDialog
			isOpen={isOpen}
			leastDestructiveRef={cancelRef}
			onClose={onClose}>
			<AlertDialogOverlay>
				<AlertDialogContent>
					<AlertDialogHeader fontSize='lg' fontWeight='bold'>
						Delete transaction
					</AlertDialogHeader>
					<AlertDialogBody>
						Are you sure? You can't undo this action afterwards.
					</AlertDialogBody>
					
					<AlertDialogFooter>
					<Button ref={cancelRef} onClick={onClose}>
						Cancel
					</Button>
					<Button colorScheme='red' onClick={onDeleteClicked} ml={3}>
						Delete
					</Button>
					</AlertDialogFooter>
				</AlertDialogContent>
			</AlertDialogOverlay>
		</AlertDialog>
		<TransactionModal transactionTypes={props.transactionTypes} 
			fundSources={props.fundSources} 
			transaction={props.transaction} 
			ref={editModalRef} 
			onSaved={props.onUpdate}/>
	</Card>
}

export default Transaction;