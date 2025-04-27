import { Fragment } from "react";
import { useTranslation } from "react-i18next";
import BrokerAccountSecuritiesList from "../../components/brokers/BrokerAccountSecuritiesList/BrokerAccountSecuritiesList";
import { useParams } from "react-router-dom";

interface Props {}

const BrokerAccountPage: React.FC<Props> = () => {

    const { t } = useTranslation();

    const { brokerAccountId } = useParams(); // Получаем текущий таб из URL

    if (!brokerAccountId) {
        return <Fragment/>
    }
    
    return (<Fragment>
        <BrokerAccountSecuritiesList brokerAccountId={brokerAccountId}/>
    </Fragment>
    )
}


export default BrokerAccountPage;