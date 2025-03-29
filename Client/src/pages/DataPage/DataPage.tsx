import DataLayout from "../../components/Data/DataLayout/DataLayout";
import CurrenciesTable from "../../components/Data/Tables/CurrenciesTable/CurrenciesTable";
import BanksTable from "../../components/Data/Tables/BanksTable/BanksTable";
import { useParams } from "react-router-dom";

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