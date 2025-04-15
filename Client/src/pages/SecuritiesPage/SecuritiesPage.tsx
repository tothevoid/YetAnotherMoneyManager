import { Fragment } from "react";
import { useTranslation } from "react-i18next";
import SecuritiesList from "../../components/brokers/SecuritiesList/SecuritiesList";

interface Props {}

const SecuritiesPage: React.FC<Props> = () => {
    const { t } = useTranslation();

    return (<Fragment>
        <SecuritiesList/>
    </Fragment>   
    )
}

export default SecuritiesPage;