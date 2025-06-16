import { Fragment } from "react";
import { useTranslation } from "react-i18next";
import BrokerAccountsList from "../BrokerAccounts/components/BrokerAccountsList/BrokerAccountsList";

interface Props {}

const CryptoAccountsPage: React.FC<Props> = () => {
    const { t } = useTranslation();

    return <Fragment>
        <BrokerAccountsList/>
    </Fragment>
}

export default CryptoAccountsPage;