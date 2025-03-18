import { useEffect, useState } from "react";
import { getAccounts } from "../../api/accountApi";
import { AccountEntity } from "../../models/AccountEntity";
import AccountsList from "../../components/AccountsList/AccountsList";

interface Props {}

interface State {
    accounts: AccountEntity[]
}

const AccountsPage: React.FC<Props> = () => {
    const [state, setState] = useState<State>({accounts: []});

    useEffect(() => {
        const initData = async () => {
            await initAccounts();
        }
        initData();
    }, []);

    const initAccounts = async () => {
        const accounts = await getAccounts();
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

    return (
        <AccountsList
            onAddAccountCallback = {onAccountCreated}
            onUpdateAccountCallback = {onAccountUpdated}
            onDeleteAccountCallback = {onAccountDeleted} 
            accounts = {state.accounts}>
        </AccountsList>
    )
}

export default AccountsPage;