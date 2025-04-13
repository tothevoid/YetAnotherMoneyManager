import { useParams } from "react-router-dom";
import DataLayout from "../../components/data/DataLayout/DataLayout";
import BanksTable from "../../components/data/Tables/BanksTable/BanksTable";
import CurrenciesTable from "../../components/data/Tables/CurrenciesTable/CurrenciesTable";

const DataPage = () => {
    const { tab = "currencies" } = useParams(); // Получаем текущий таб из URL

    return (
        <DataLayout>
            {tab === "currencies" && <CurrenciesTable />}
            {tab === "banks" && <BanksTable />}
        </DataLayout>
    );
}

export default DataPage;