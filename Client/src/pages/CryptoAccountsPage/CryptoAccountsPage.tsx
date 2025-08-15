import { Fragment, useRef } from "react";
import { useTranslation } from "react-i18next";
import { useCryptoAccounts } from "./hooks/useCryptoAccounts";
import { CryptoAccountEntity } from "../../models/crypto/CryptoAccountEntity";
import CryptoAccount from "./components/CryptoAccount/CryptoAccount";
import { Flex, SimpleGrid } from "@chakra-ui/react";
import ShowModalButton from "../../shared/components/ShowModalButton/ShowModalButton";
import { BaseModalRef } from "../../shared/utilities/modalUtilities";
import CryptoAccountModal from "./modals/CryptoAccountModal/CryptoAccountModal";

const CryptoAccountsPage: React.FC = () => {
    const { t } = useTranslation()
    
    const {
        cryptoAccounts,
        createCryptoAccountEntity,
        updateCryptoAccountEntity,
        deleteCryptoAccountEntity,
        reloadCryptoAccounts
    } = useCryptoAccounts();

    const modalRef = useRef<BaseModalRef>(null);
    
    const onAdd = () => {
        modalRef.current?.openModal()
    };

    return (
        <Fragment>
            <Flex justifyContent="space-between" alignItems="center" pt={5} pb={5}>
                <ShowModalButton buttonTitle={t("crypto_accounts_page_add")} onClick={onAdd}>
                    <CryptoAccountModal modalRef={modalRef} onSaved={createCryptoAccountEntity}/>
                </ShowModalButton>
            </Flex>
            <SimpleGrid pt={5} pb={5} gap={4} templateColumns='repeat(auto-fill, minmax(400px, 3fr))'>
                {
                    cryptoAccounts.map((cryptoAccount: CryptoAccountEntity) => 
                        <CryptoAccount onReloadCryptoAccounts={reloadCryptoAccounts} cryptoAccount={cryptoAccount} onEditCallback={updateCryptoAccountEntity} 
                            onDeleteCallback={deleteCryptoAccountEntity} key={cryptoAccount.id}/>)
                }
            </SimpleGrid>
        </Fragment>
    );
}

export default CryptoAccountsPage;