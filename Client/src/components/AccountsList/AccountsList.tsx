import React, { Fragment, useEffect, useState } from 'react';
import Account from '../Account/Account'
import { AccountEntity } from '../../models/accounts/AccountEntity';
import { SimpleGrid } from '@chakra-ui/react/grid';
import { Checkbox, Flex } from '@chakra-ui/react';
import AddAccountButton from '../AddAccountButton/AddAccountButton';
import { useTranslation } from 'react-i18next';
import { getAccounts } from '../../api/accounts/accountApi';

interface Props {
	onAccountsChanged: () => void
}

interface State {
	onlyActive: boolean
	accounts: AccountEntity[]
}

const AccountsList: React.FC<Props> = (props) => {
	const { t } = useTranslation()

	const [state, setState] = useState<State>({ onlyActive: true, accounts: [] })

 	useEffect(() => {
		const initData = async () => {
			await requestAccountsData();
		}
		initData();
	}, []);

	useEffect(() => {
		const reloadData = async () => {
			await requestAccountsData();
		}
		reloadData();
	}, [state.onlyActive]);

	const requestAccountsData = async () => {
		const accounts = await getAccounts(state.onlyActive);
		setState((currentState) => {
			return {...currentState, accounts}
		})
	};

	const onAccountCreated = async (addedAccount: AccountEntity) => {
        if (!addedAccount) {
            return
        }

        setState((currentState: State) => {
            const accounts = state.accounts.concat(addedAccount);
            return {...currentState, accounts};
        });
    };

    const onAccountUpdated = async (updatedAccount: AccountEntity) => {
        if (!updatedAccount) {
            return
        }

        let accountNameChanged = false;

        setState((currentState: State) => {
            const accounts = currentState.accounts.map((account: AccountEntity) => {
                if (account.id === updatedAccount.id) {
                    accountNameChanged = account.name !== updatedAccount.name;
                    return {...updatedAccount}
                } 
                return account
            });

            if (!accountNameChanged) {
                return {...currentState, accounts};
            }
            
            return {...currentState, accounts}
        });
    };

    const onAccountDeleted = async (deletedAccount: AccountEntity) => {
        if (!deletedAccount) {
            return;
        }

        setState((currentState: State) => {
            const accounts = currentState.accounts.filter((account: AccountEntity) => account.id !== deletedAccount.id)
            return { ...currentState, accounts }
        });
    };

    const onReloadAccounts = async () => {
        await requestAccountsData();
    }

	const onCheckboxChanged = async (checkboxChange: any) => {
		setState((currentState) => {
			return {...currentState, onlyActive: !!checkboxChange.checked};
		});
	}

	return (
		<Fragment>
			<Flex justifyContent="space-between" alignItems="center" pt={5} pb={5}>
				<div>
					<Checkbox.Root checked={state.onlyActive} onCheckedChange={onCheckboxChanged} variant="solid">
						<Checkbox.HiddenInput />
						<Checkbox.Control />
						<Checkbox.Label color="text_primary">{t("accounts_list_only_active")}</Checkbox.Label>
					</Checkbox.Root>
				</div>
				<AddAccountButton onAdded={onAccountCreated}></AddAccountButton>
			</Flex>
			<SimpleGrid pt={5} pb={5} gap={4} templateColumns='repeat(auto-fill, minmax(300px, 3fr))'>
				{
				state.accounts.map((account: AccountEntity) => {
					return <Account onReloadAccounts={onReloadAccounts} account={account} onEditCallback={onAccountUpdated} 
						onDeleteCallback={onAccountDeleted} key={account.id}/>
				})
				}
			</SimpleGrid>
		</Fragment>
	);
}

export default AccountsList;