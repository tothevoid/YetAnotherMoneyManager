import { useCallback, useEffect, useState } from "react";
import { CryptoAccountCryptocurrencyEntity } from "../../../models/crypto/CryptoAccountCryptocurrencyEntity";
import { getCryptocurrenciesByCryptoAccount } from "../../../api/crypto/cryptoAccountCryptocurrencyApi";

export interface CryptoAccountCryptocurrenciesQuery {
    cryptoAccountId: string
}

export const useCryptoAccountCryptocurrencies = (queryParameters: CryptoAccountCryptocurrenciesQuery) => {
    const [cryptoAccountCryptocurrencies, setCryptoAccountCryptocurrencies] = useState<CryptoAccountCryptocurrencyEntity[]>([]);
    const [isBrokerAccountSecuritiesLoading, setLoading] = useState(false);

    const [error, setError] = useState<string | null>(null);
    const [cryptoAccountCryptocurrenciesQueryParameters, setCryptoAccountCryptocurrenciesQueryParameters] = 
        useState<CryptoAccountCryptocurrenciesQuery>(queryParameters);

    const fetchData = useCallback(async () => {
        setLoading(true)
        try {
            setCryptoAccountCryptocurrencies(await getCryptocurrenciesByCryptoAccount(cryptoAccountCryptocurrenciesQueryParameters.cryptoAccountId));
        } catch (err: any) {
            setError(err.message || 'Ошибка загрузки данных')
        } finally {
            setLoading(false)
        }
    }, [cryptoAccountCryptocurrenciesQueryParameters])

    useEffect(() => {
        fetchData();
    }, [fetchData])


    return {
        cryptoAccountCryptocurrencies,
        isBrokerAccountSecuritiesLoading,
        error,
        setCryptoAccountCryptocurrenciesQueryParameters,
        cryptoAccountCryptocurrenciesQueryParameters,
        reloadCryptoAccountCryptocurrencies: fetchData
    }
}
