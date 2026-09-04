import React, { Fragment, useCallback, useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import CryptoAccountHeader from "../CryptoAccountPage/components/CryptoAccountHeader/CryptoAccountHeader";
import CryptoAccountTabs from "../CryptoAccountPage/components/CryptoAccountTabs/CryptoAccountTabs";
import { getTotalBalance } from "../../api/crypto/cryptoAccountCryptocurrencyApi";

const CryptoAccountsPage: React.FC = () => {
    const { t } = useTranslation();
    const [totalBalanceUsd, setTotalBalanceUsd] = useState<number>(0);

    const fetchData = useCallback(async () => {
        const balance = await getTotalBalance();
        setTotalBalanceUsd(balance);
    }, []);

    useEffect(() => {
        fetchData();
    }, [fetchData]);

    return (
        <Fragment>
            <CryptoAccountHeader
                title={t("all_crypto_accounts_header")}
                totalBalanceUsd={totalBalanceUsd}
            />
            <CryptoAccountTabs onDataChanged={fetchData} />
        </Fragment>
    );
};

export default CryptoAccountsPage;