import { Fragment, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { CryptoAccountEntity } from "../../models/crypto/CryptoAccountEntity";
import { getCryptoAccountById } from "../../api/crypto/cryptoAccountApi";
import CryptoAccountCryptocurrenciesList from "./components/CryptoAccountCryptocurrenciesList/CryptoAccountCryptocurrenciesList";
import CryptoAccountTabs from "./components/CryptoAccountTabs/CryptoAccountTabs";

interface State {
    cryptoAccount: CryptoAccountEntity | null,
    isReloading: boolean
}

const CryptoAccountPage: React.FC = () => {
    const { cryptoAccountId } = useParams();

    const [state, setState] = useState<State>({ cryptoAccount: null, isReloading: false });

    const fetchCryptoAccount = async () => {
        if (!cryptoAccountId) {
            return;
        }

        const cryptoAccount = await getCryptoAccountById(cryptoAccountId);
        if (!cryptoAccount) {
            return;
        }

        setState((currentState) => {
            return { ...currentState, cryptoAccount, isReloading: false };
        });
    };

    useEffect(() => {
        fetchCryptoAccount();
    }, []);

    if (!cryptoAccountId || !state.cryptoAccount) {
        return <Fragment />;
    }

    return (
        <Fragment>
            <CryptoAccountCryptocurrenciesList cryptoAccount={state.cryptoAccount} />
            <CryptoAccountTabs cryptoAccountId={cryptoAccountId} onDataChanged={fetchCryptoAccount} />
        </Fragment>
    );
};

export default CryptoAccountPage;