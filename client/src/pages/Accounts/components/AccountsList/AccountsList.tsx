import React, { useEffect, useRef } from 'react';
import Account from '../Account/Account';
import { SimpleGrid } from '@chakra-ui/react/grid';
import { Box, Checkbox, Flex, Text } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';
import { AccountEntity } from '../../../../models/accounts/AccountEntity';
import ShowModalButton from '../../../../shared/components/ShowModalButton/ShowModalButton';
import { BaseModalRef } from '../../../../shared/utilities/modalUtilities';
import AccountModal from '../../modals/AccountModal/AccountModal';
import { useAccounts } from '../../hooks/useAccounts';
import Placeholder from '../../../../shared/components/Placeholder/Placeholder';

interface Props {
	onAccountsChanged: () => void
}

const AccountsList: React.FC<Props> = ({onAccountsChanged}) => {
	const { t } = useTranslation();

	const { 
		accounts, 
		accountQueryParameters,
		createAccountEntity,
		updateAccountEntity,
		deleteAccountEntity,
		setAccountQueryParameters,
		reloadAccounts
	} = useAccounts({ onlyActive: true })

	useEffect(() => {
		onAccountsChanged();
	}, [accounts, onAccountsChanged])

	const onCheckboxChanged = async (checkboxChange: any) => {	
		setAccountQueryParameters({onlyActive: !!checkboxChange.checked})
	}

	const modalRef = useRef<BaseModalRef>(null);
	
	const onAdd = () => {
		modalRef.current?.openModal()
	};

	const addButton = () => {
		return <ShowModalButton buttonTitle={t("accounts_page_summary_add")} onClick={onAdd}>
			<AccountModal modalRef={modalRef} onSaved={createAccountEntity}/>
		</ShowModalButton>
	}

	return <Box>
		<Flex justifyContent="space-between" alignItems="center" pt={5} pb={5}>
			<Box>
				<Checkbox.Root checked={accountQueryParameters.onlyActive} onCheckedChange={onCheckboxChanged} variant="solid">
					<Checkbox.HiddenInput />
					<Checkbox.Control />
					<Checkbox.Label color="text_primary">{t("accounts_list_only_active")}</Checkbox.Label>
				</Checkbox.Root>
			</Box>
			{accounts.length > 0 && addButton()}
		</Flex>
		{
			accounts.length > 0 ?
				<SimpleGrid pt={5} pb={5} gap={4} templateColumns='repeat(auto-fill, minmax(350px, 3fr))'>
				{
					accounts.map((account: AccountEntity) => {
						return <Account key={account.id} onReloadAccounts={reloadAccounts} account={account} onEditCallback={updateAccountEntity} 
							onDeleteCallback={deleteAccountEntity}/>
					})
				}
				</SimpleGrid>:
				<Placeholder text={t("accounts_list_no_accounts")}>
					{addButton()}
				</Placeholder>
		}
	</Box>
}

export default AccountsList;