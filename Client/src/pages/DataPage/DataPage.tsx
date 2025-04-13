import { useParams } from "react-router-dom";
import DataLayout from "../../components/data/DataLayout/DataLayout";
import BanksTable from "../../components/data/Tables/BanksTable/BanksTable";
import CurrenciesTable from "../../components/data/Tables/CurrenciesTable/CurrenciesTable";
import BrokerAccountTypesTable from "../../components/data/Tables/BrokerAccountTypesTable/BrokerAccountTypesTable";
import BrokersTable from "../../components/data/Tables/BrokersTable/BrokersTable";

const DataPage = () => {
    const { tab = "currencies" } = useParams(); // Получаем текущий таб из URL

    return (
        <DataLayout>
            {tab === "currencies" && <CurrenciesTable />}
            {tab === "banks" && <BanksTable />}
            {tab === "broker_account_types" && <BrokerAccountTypesTable />}
            {tab === "brokers" && <BrokersTable />}
        </DataLayout>
    );
}

export default DataPage;