import { Fragment, useEffect, useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import { useParams } from "react-router-dom";
import { Stack, Text } from "@chakra-ui/react";
import { ClientCryptoAccountEntity } from "../../models/crypto/CryptoAccountEntity";
import { getCryptoAccountById } from "../../api/crypto/cryptoAccountApi";
import CryptoAccountCryptocurrenciesList from "./components/CryptoAccountCryptocurrenciesList/CryptoAccountCryptocurrenciesList";

interface Props {}

interface State {
    cryptoAccount: ClientCryptoAccountEntity | null,
    isReloading: boolean
}

const CryptoAccountPage: React.FC<Props> = () => {
    const { t } = useTranslation();

    const { cryptoAccountId } = useParams(); // Получаем текущий таб из URL

    if (!cryptoAccountId) {
        return <Fragment/>
    }

    const [state, setState] = useState<State>({ cryptoAccount: null, isReloading: false })


    const fetchCryptoAccount = async () => {
        const cryptoAccount = await getCryptoAccountById(cryptoAccountId);
        if (!cryptoAccount) {
            return;
        }

        setState((currentState) => {
            return {...currentState, cryptoAccount, isReloading: false}
        })
    }

    useEffect(() => {
        fetchCryptoAccount();
    }, []);

    if (!state.cryptoAccount) {
        return <Fragment/>
    }

    //TODO: tabs style is duplicated

    return <Fragment>
        <Stack alignItems={"end"} gapX={2} direction={"row"} color="text_primary">
            <Text fontSize="3xl" fontWeight={900}> {state.cryptoAccount?.name}: </Text>
        </Stack>
        <CryptoAccountCryptocurrenciesList cryptoAccount={state.cryptoAccount}/>
    </Fragment>
}

export default CryptoAccountPage;