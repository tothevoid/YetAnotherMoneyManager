import React, { Fragment } from 'react';
import Account from '../Account/Account'
import { AccountEntity } from '../../models/AccountEntity';
import { SimpleGrid } from '@chakra-ui/react/grid';
import { Flex } from '@chakra-ui/react';
import AddAccountButton from '../AddAccountButton/AddAccountButton';

type Props = {
	accounts: AccountEntity[],
	onReloadAccounts: () => void,
	onAddAccountCallback: (account: AccountEntity) => void,
	onDeleteAccountCallback: (account: AccountEntity) => void,
	onUpdateAccountCallback: (account: AccountEntity) => void
}

const AccountsList: React.FC<Props> = (props) => {
	const {onAddAccountCallback, onUpdateAccountCallback, onDeleteAccountCallback } = props;
	return (
		<Fragment>
			<Flex justifyContent="space-between" alignItems="center" pt={5} pb={5}>
				<AddAccountButton onAdded={onAddAccountCallback}></AddAccountButton>
			</Flex>
			<SimpleGrid pt={5} pb={5} gap={4} templateColumns='repeat(auto-fill, minmax(300px, 3fr))'>
				{
				props.accounts.map((account: AccountEntity) => {
					return <Account onReloadAccounts={props.onReloadAccounts} account={account} onEditCallback={onUpdateAccountCallback} 
						onDeleteCallback={onDeleteAccountCallback} key={account.id}/>
				})
				}
			</SimpleGrid>
		</Fragment>
	);
}

export default AccountsList;