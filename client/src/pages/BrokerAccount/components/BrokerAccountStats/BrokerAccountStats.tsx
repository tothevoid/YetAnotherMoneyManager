import { Fragment } from "react";
import { useTranslation } from "react-i18next";

interface Props {
    brokerAccountId: string
}

const BrokerAccountStats: React.FC<Props> = ({ brokerAccountId }) => {
    const { t } = useTranslation();

    return <Fragment/>
}

export default BrokerAccountStats;