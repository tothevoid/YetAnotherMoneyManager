import { Fragment, useEffect, useState } from "react";
import { getAccounts, getSummary } from "../../api/accountApi";
import { AccountEntity } from "../../models/AccountEntity";
import AccountsList from "../../components/AccountsList/AccountsList";
import { AccountCurrencySummary } from "../../api/models/accountsSummary";
import { Flex, Text } from "@chakra-ui/react";
import { formatMoneyByCurrencyCulture } from "../../formatters/moneyFormatter";
import { useTranslation } from "react-i18next";

interface Props {}

interface State {
    accounts: AccountEntity[]
    accountCurrencySummaries: AccountCurrencySummary[]
}

const AccountsPage: React.FC<Props> = () => {
    const [state, setState] = useState<State>({accounts: [], accountCurrencySummaries: []});

    const { t } = useTranslation();

    useEffect(() => {
        const initData = async () => {
            await requestAccountsData();
        }
        initData();
    }, []);

    const requestAccountsData = async () => {
        const accounts = await getAccounts();
        const accountCurrencySummaries = await getSummary() ?? null;
        setState((currentState) => {
            return {...currentState, accounts, accountCurrencySummaries}
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

    return (<Fragment>
        {
            state.accountCurrencySummaries.length > 0 ?
                <Flex justifyContent="space-between" alignItems="center" pt={5} pb={5}>
                    <Text fontSize='3xl'>{t("accounts_page_summary_title")}: {state.accountCurrencySummaries.map(currency => 
                        formatMoneyByCurrencyCulture(currency.summary, currency.name)).join(" | ") }
                    </Text>
                </Flex>:
                <Fragment/>
        }
        <AccountsList
            onReloadAccounts={onReloadAccounts}
            onAddAccountCallback = {onAccountCreated}
            onUpdateAccountCallback = {onAccountUpdated}
            onDeleteAccountCallback = {onAccountDeleted} 
            accounts = {state.accounts}>
        </AccountsList>
    </Fragment>   
    )
}

export default AccountsPage;