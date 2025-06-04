import "./TransactionsPage.scss"

import React, { Fragment, useEffect, useRef, useState } from 'react';
import Transaction from './components/Transaction/Transaction';
import { AccountEntity } from '../../models/accounts/AccountEntity';
import Pagination from './components/Pagination/Pagination';
import TransactionStats from './components/TransactionStats/TransactionStats';
import { Box, Checkbox, Flex, SimpleGrid, Spinner, Text, VStack } from '@chakra-ui/react';
import { getAccountsByTypes } from '../../api/accounts/accountApi';
import { useTranslation } from 'react-i18next';
import { TransactionEntity } from "../../models/transactions/TransactionEntity";
import { formatDate } from "../../shared/utilities/formatters/dateFormatter";
import { formatMoneyByCurrencyCulture } from "../../shared/utilities/formatters/moneyFormatter";
import { useUserProfile } from "../../../features/UserProfileSettingsModal/hooks/UserProfileContext";
import ShowModalButton from "../../shared/components/ShowModalButton/ShowModalButton";
import { BaseModalRef } from "../../shared/utilities/modalUtilities";
import TransactionModal from "./modals/TransactionModal/TransactionModal";
import { useTransactions } from "./hooks/useTransactions";
import { groupByKey, sumEntities } from "../../shared/utilities/arrayUtilities";

interface Props {}

interface State {
    accounts: AccountEntity[]
}

const TransactionsPage: React.FC<Props> = () => {
    const { t, i18n } = useTranslation();
    const { user } = useUserProfile();
    const [state, setState] = useState<State>({accounts: [] as AccountEntity[]});

    const {
        transactions,
		createTransactionEntity,
		updateTransactionEntity,
		deleteTransactionEntity,
		setParams,
		params,
        isTransactionsLoading
    } = useTransactions({ month: new Date().getMonth() + 1, year: new Date().getFullYear(), showSystem: false })

    useEffect(() => {
        const initData = async () => {
            await initAccounts();
        }
        initData();
    }, []);

    const initAccounts = async () => {
        const accounts = await getAccountsByTypes([
            "6ea1867f-c067-412c-b443-8b9bc2467202",	// Credit card
            "a08f5553-379e-4294-a2e5-75e88219433c",	// Cash
            "cda2ce07-551e-48cf-988d-270c0d022866"	// Debit card
        ], true);
        setState((currentState) => {
            return {...currentState, accounts}
        })
    };

    const onPageSwitched = (month: number, year: number) => {
        setParams({month: month, year: year, showSystem: params.showSystem});
    }

    const onShowSystemSwitched = (showSystem: boolean) => {
		setParams({...params, showSystem: showSystem})
	}
  
    const groupedTransactions = groupByKey(transactions, (transaction) => transaction.date.getDate())

    const calculateSummary = (transactions: TransactionEntity[]) => {
        const sum = sumEntities(transactions, (transaction) => transaction.amount * transaction.account.currency.rate)
        return formatMoneyByCurrencyCulture(sum, user?.currency.name);
    }

    const addTransactionModalRef = useRef<BaseModalRef>(null);
            
    const onAddTransactionClick = () => {
        addTransactionModalRef.current?.openModal()
    }

    return (
        <Box color="text_primary" paddingTop={4} paddingBottom={4}>
            <SimpleGrid columns={2} gap={16}>
                <Box>
                    <Flex justifyContent={"space-between"}>
                        <Text fontSize="2xl" fontWeight={600}>{t("manager_transactions_title")}</Text>
                        <ShowModalButton buttonTitle={t("manager_transactions_add_transaction")} onClick={onAddTransactionClick}>
                            <TransactionModal modalRef={addTransactionModalRef} accounts={state.accounts} onSaved={createTransactionEntity}/>
                        </ShowModalButton>
                    </Flex>
                    <Box marginBlock={"10px"}>
                        <Checkbox.Root checked={params.showSystem} onCheckedChange={(details) => onShowSystemSwitched(!!details.checked)} variant="solid">
                            <Checkbox.HiddenInput />
                            <Checkbox.Control />
                            <Checkbox.Label color="text_primary">{t("manager_transactions_show_system")}</Checkbox.Label>
                        </Checkbox.Root>
                    </Box>
                    <Pagination year={params.year} month={params.month} onPageSwitched={onPageSwitched}></Pagination>
                    <Box>
                        { isTransactionsLoading && <VStack marginBlock={"50px"}> <Spinner size={"lg"}/> </VStack>}
                        {
                            !isTransactionsLoading && [...groupedTransactions.entries()].map(([transactionDay, transactions]) => 
                                <Box key={transactionDay}>
                                    <Flex justifyContent="space-between">
                                        <Text>{formatDate(new Date(params.year, params.month - 1, transactionDay), i18n, false)}</Text>
                                        <Text>{calculateSummary(transactions)}</Text>
                                    </Flex>
                                    
                                    {
                                        transactions.map((transaction: TransactionEntity) => {       
                                            return <Transaction key={transaction.id} transaction={transaction} 
                                                onDelete={deleteTransactionEntity} onUpdate={updateTransactionEntity}
                                                accounts={state.accounts}>
                                            </Transaction>
                                        })
                                    }
                                </Box>
                            )
                        }
                        { !isTransactionsLoading && !transactions.length && <Box className="empty-transactions">{t("manager_transactions_no_transactions")}</Box>}
                    </Box>
                </Box>
                <Box>
                    {
                        transactions.length > 0 && <TransactionStats accounts={state.accounts} transactions={transactions}/>
                    }
                </Box>
            </SimpleGrid>
        </Box>
    );
}

export default TransactionsPage;