import { useCallback, useEffect, useState } from "react";
import { CryptoAccountEntity } from "../../../models/crypto/CryptoAccountEntity";
import { createCryptoAccount, deleteCryptoAccount, getCryptoAccounts, updateCryptoAccount } from "../../../api/crypto/cryptoAccountApi";

export const useCryptoAccounts = () => {
    const [cryptoAccounts, setCryptoAccounts] = useState<CryptoAccountEntity[]>([]);
    const [isCryptoAccountsLoading, setLoading] = useState(false);

    const [error, setError] = useState<string | null>(null);

    const fetchData = useCallback(async () => {
        setLoading(true)
        try {
            setCryptoAccounts(await getCryptoAccounts());
        } catch (err: any) {
            setError(err.message || 'Ошибка загрузки данных')
        } finally {
            setLoading(false)
        }
    }, [])

    useEffect(() => {
        fetchData();
    }, [fetchData])

    const createCryptoAccountEntity = async (createdCryptoAccount: CryptoAccountEntity) => {
        const addedCryptoAccount = await createCryptoAccount(createdCryptoAccount);
        if (!addedCryptoAccount) {
            return
        }

        setCryptoAccounts([createdCryptoAccount, ...cryptoAccounts]);
    }

    const updateCryptoAccountEntity = async (updatedCryptoAccount: CryptoAccountEntity) => {
        const cryptoAccountUpdated = await updateCryptoAccount(updatedCryptoAccount);
        if (!cryptoAccountUpdated) {
            return;
        }

        const updatedBrokerAccounts = cryptoAccounts.map((cryptoAccount: CryptoAccountEntity) => 
            cryptoAccount.id === updatedCryptoAccount.id ?
                {...updatedCryptoAccount}:
                cryptoAccount
        );

        setCryptoAccounts(updatedBrokerAccounts);
    }

    const deleteCryptoAccountEntity = async (deletedCryptoAccount: CryptoAccountEntity) => {
        const cryptoAccountDeleted = await deleteCryptoAccount(deletedCryptoAccount.id);
    
        if (!cryptoAccountDeleted) {
            return;
        }

        const updatedCryptoAccounts = cryptoAccounts
            .filter((cryptoAccount: CryptoAccountEntity) => cryptoAccount.id !== deletedCryptoAccount.id);
        setCryptoAccounts(updatedCryptoAccounts)
    }

    return {
        cryptoAccounts,
        isCryptoAccountsLoading,
        error,
        createCryptoAccountEntity,
        updateCryptoAccountEntity,
        deleteCryptoAccountEntity,
        reloadCryptoAccounts: fetchData
    }
}
