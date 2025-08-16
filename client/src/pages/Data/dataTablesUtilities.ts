interface DataTable {
    id: string,
    href: string,
    label: string
}

export const getDataTablesConfig = (): DataTable[] => {
    const basicPath = "data";

    //TODO add i18n

    return [
        { id: "transaction_types", href: `/${basicPath}/transaction_types`, label: "Типы транзакций" },
        { id: "currencies", href: `/${basicPath}/currencies`, label: "Валюты" },
        { id: "banks", href: `/${basicPath}/banks`, label: "Банки" },
        { id: "broker_account_types", href: `/${basicPath}/broker_account_types`, label: "Типы брокерских счетов" },
        { id: "brokers", href: `/${basicPath}/brokers`, label: "Брокеры" },
        { id: "crypto_providers", href: `/${basicPath}/crypto_providers`, label: "Крипто провайдеры" }
    ]
}