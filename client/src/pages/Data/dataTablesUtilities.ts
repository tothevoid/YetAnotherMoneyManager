import { TFunction } from "i18next";

export interface DataTable {
    id: string;
    href: string;
    label: string;
}

export const getDataTablesConfig = (t: TFunction): DataTable[] => {
    const basicPath = "data";

    return [
        { id: "transaction_types", href: `/${basicPath}/transaction_types`, label: t("data_tab_transaction_types") },
        { id: "currencies", href: `/${basicPath}/currencies`, label: t("data_tab_currencies") },
        { id: "banks", href: `/${basicPath}/banks`, label: t("data_tab_banks") },
        { id: "broker_account_types", href: `/${basicPath}/broker_account_types`, label: t("data_tab_broker_account_types") },
        { id: "brokers", href: `/${basicPath}/brokers`, label: t("data_tab_brokers") },
        { id: "crypto_providers", href: `/${basicPath}/crypto_providers`, label: t("data_tab_crypto_providers") }
    ];
};