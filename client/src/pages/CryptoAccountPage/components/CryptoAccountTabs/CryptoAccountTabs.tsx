import React from "react";
import { useTranslation } from "react-i18next";
import { Tabs } from "@chakra-ui/react";
import { IoMdStats } from "react-icons/io";
import { MdQueryStats } from "react-icons/md";
import CryptoAccountsList from "../../../CryptoAccountsPage/components/CryptoAccountsList/CryptoAccountsList";
import CryptoAccountStats from "../CryptoAccountStats/CryptoAccountStats";

interface Props {
    cryptoAccountId?: string;
    onDataChanged: () => void;
}

const CryptoAccountTabs: React.FC<Props> = ({ cryptoAccountId, onDataChanged }) => {
    const { t } = useTranslation();

    const defaultTab = !cryptoAccountId ? "crypto_accounts" : "stats";

    return (
        <Tabs.Root
            lazyMount={true}
            unmountOnExit={true}
            variant="enclosed"
            defaultValue={defaultTab}
        >
            <Tabs.List>
                {!cryptoAccountId && (
                    <Tabs.Trigger value="crypto_accounts">
                        <IoMdStats />
                        {t("crypto_accounts_tab")}
                    </Tabs.Trigger>
                )}
                <Tabs.Trigger value="stats">
                    <MdQueryStats />
                    {t("crypto_account_page_account_stats_tab")}
                </Tabs.Trigger>
            </Tabs.List>

            {!cryptoAccountId && (
                <Tabs.Content value="crypto_accounts">
                    <CryptoAccountsList onDataChanged={onDataChanged} />
                </Tabs.Content>
            )}
            <Tabs.Content value="stats">
                <CryptoAccountStats cryptoAccountId={cryptoAccountId} />
            </Tabs.Content>
        </Tabs.Root>
    );
};

export default CryptoAccountTabs;
