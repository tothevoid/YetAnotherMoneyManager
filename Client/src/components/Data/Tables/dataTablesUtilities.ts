interface DataTable {
    id: string,
    href: string,
    label: string
}

export const getDataTablesConfig = (): DataTable[] => {
    const basicPath = "data";

    return [
        { id: "currencies", href: `/${basicPath}/currencies`, label: "Валюты" },
        { id: "banks", href: `/${basicPath}/banks`, label: "Банки" }
    ]
}