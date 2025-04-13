import { Fragment } from "react";
import { useTranslation } from "react-i18next";
import BrokerAccountsList from "../../components/brokers/BrokerAccountsList/BrokerAccountsList";

interface Props {}


const BrokerAccountsPage: React.FC<Props> = () => {
    const { t } = useTranslation();

    return (<Fragment>
        <BrokerAccountsList/>
    </Fragment>   
    )
}

export default BrokerAccountsPage;