import { useParams } from "react-router-dom";
import DataLayout from "../../components/Data/DataLayout/DataLayout";
import BanksTable from "../../components/Data/Tables/BanksTable/BanksTable";
import BrokerAccountTypesTable from "../../components/Data/Tables/BrokerAccountTypesTable/BrokerAccountTypesTable";
import BrokersTable from "../../components/Data/Tables/BrokersTable/BrokersTable";
import CurrenciesTable from "../../components/Data/Tables/CurrenciesTable/CurrenciesTable";
import TransactionTypesTable from "../../components/Data/Tables/TransactionTypesTable/TransactionTypesTable";

const DataPage = () => {
    const { tab = "currencies" } = useParams(); // Получаем текущий таб из URL

    return (
        <DataLayout>
            {tab === "transaction_types" && <TransactionTypesTable />}
            {tab === "currencies" && <CurrenciesTable />}
            {tab === "banks" && <BanksTable />}
            {tab === "broker_account_types" && <BrokerAccountTypesTable />}
            {tab === "brokers" && <BrokersTable />}
        </DataLayout>
    );
}

export default DataPage;