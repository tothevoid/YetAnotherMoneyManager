import { useCallback, useEffect, useState } from "react";
import { CryptoAccountCryptocurrencyEntity } from "../../../models/crypto/CryptoAccountCryptocurrencyEntity";
import { createCryptoAccountCryptocurrency, deleteCryptoAccountCryptocurrency, updateCryptoAccountCryptocurrency, getCryptocurrenciesByCryptoAccount } from "../../../api/crypto/cryptoAccountCryptocurrencyApi";

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

    const addCryptoAccountCryptocurrencyEntity = async (cryptoAccountCryptocurrency: CryptoAccountCryptocurrencyEntity) => {
        await createCryptoAccountCryptocurrency(cryptoAccountCryptocurrency);
        await fetchData();
    }

    const updateCryptoAccountCryptocurrencyEntity = async (cryptoAccountCryptocurrency: CryptoAccountCryptocurrencyEntity) => {
        await updateCryptoAccountCryptocurrency(cryptoAccountCryptocurrency);
        await fetchData();
    }

    const deleteCryptoAccountCryptocurrencyEntity = async (id: string) => {
        await deleteCryptoAccountCryptocurrency(id);
        await fetchData();
    }

    return {
        cryptoAccountCryptocurrencies,
        addCryptoAccountCryptocurrencyEntity,
        updateCryptoAccountCryptocurrencyEntity,
        deleteCryptoAccountCryptocurrencyEntity,
        isBrokerAccountSecuritiesLoading,
        error,
        setCryptoAccountCryptocurrenciesQueryParameters,
        cryptoAccountCryptocurrenciesQueryParameters,
        reloadCryptoAccountCryptocurrencies: fetchData
    }
}
