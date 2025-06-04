import { Fragment, useState } from "react";
import { getSummary } from "../../api/accounts/accountApi";
import { AccountCurrencySummary } from "../../models/accounts/accountsSummary";
import { Flex, Text } from "@chakra-ui/react";
import { formatMoneyByCurrencyCulture } from "../../shared/utilities/formatters/moneyFormatter";
import { useTranslation } from "react-i18next";
import AccountsList from "./components/AccountsList/AccountsList";

interface Props {}

interface State {
    accountCurrencySummaries: AccountCurrencySummary[]
}

const AccountsPage: React.FC<Props> = () => {
    const [state, setState] = useState<State>({accountCurrencySummaries: []});

    const { t } = useTranslation();
    const requestAccountsData = async () => {
        const accountCurrencySummaries = await getSummary();
        setState((currentState) => {
            return {...currentState, accountCurrencySummaries}
        })
    };

    const onAccountsChanged = async () => {
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
        <AccountsList onAccountsChanged={onAccountsChanged}/>
    </Fragment>
    )
}

export default AccountsPage;