import React, { Fragment } from 'react';
import Account from '../Account/Account'
import { AccountEntity } from '../../models/AccountEntity';
import { SimpleGrid } from '@chakra-ui/react/grid';
import { Flex, Text } from '@chakra-ui/react';
import AddAccountButton from '../AddAccountButton/AddAccountButton';
import { formatMoney } from '../../formatters/moneyFormatter';
import { useTranslation } from 'react-i18next';

const calculateTotal = (items: AccountEntity[]) => {
	return items.reduce((total: number, item: AccountEntity) => total += item.balance, 0);
};

type Props = {
	accounts: AccountEntity[],
	onReloadAccounts: () => void,
	onAddAccountCallback: (account: AccountEntity) => void,
	onDeleteAccountCallback: (account: AccountEntity) => void,
	onUpdateAccountCallback: (account: AccountEntity) => void
}

const AccountsList: React.FC<Props> = (props) => {
	// TODO: Check unnecessary calculations
	const total = calculateTotal(props.accounts);

	const {onAddAccountCallback, onUpdateAccountCallback, onDeleteAccountCallback } = props;
 
	const { t } = useTranslation();

	return (
		<Fragment>
			<Flex justifyContent="space-between" alignItems="center" pt={5} pb={5}>
				<Text fontSize='3xl'>{t("accounts_page_summary_title")}: {formatMoney(total)}</Text>
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