import { useParams } from "react-router-dom";
import DataLayout from "../../components/Data/DataLayout/DataLayout";
import BanksTable from "../../components/Data/Tables/BanksTable/BanksTable";
import BrokerAccountTypesTable from "../../components/Data/Tables/BrokerAccountTypesTable/BrokerAccountTypesTable";
import BrokersTable from "../../components/Data/Tables/BrokersTable/BrokersTable";
import CurrenciesTable from "../../components/Data/Tables/CurrenciesTable/CurrenciesTable";
import TransactionTypesTable from "../../components/Data/Tables/TransactionTypesTable/TransactionTypesTable";
import { FC, Fragment } from "react";

const DataPage = () => {
    const tabs = new Map<string, FC>(
        [
            ["transaction_types",  TransactionTypesTable],
            ["currencies", CurrenciesTable],
            ["banks", BanksTable],
            ["broker_account_types", BrokerAccountTypesTable],
            ["brokers", BrokersTable]
        ]
    );

    const { tab = [...tabs.keys()][0] } = useParams(); // Получаем текущий таб из URL
    const Component = tabs.get(tab);

    if (!Component){
        return <Fragment/>
    }

    return (
        <DataLayout>
            <Component/>
        </DataLayout>
    );
}

export default DataPage;