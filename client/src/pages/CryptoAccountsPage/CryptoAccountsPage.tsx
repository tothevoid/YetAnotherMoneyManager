import { Fragment } from "react";
import { useTranslation } from "react-i18next";
import { useCryptoAccounts } from "./hooks/useCryptoAccounts";
import { CryptoAccountEntity } from "../../models/crypto/CryptoAccountEntity";
import CryptoAccount from "./components/CryptoAccount/CryptoAccount";
import { Flex, SimpleGrid } from "@chakra-ui/react";
import CryptoAccountModal from "./modals/CryptoAccountModal/CryptoAccountModal";
import Placeholder from "../../shared/components/Placeholder/Placeholder";
import { ConfirmModal } from "../../shared/modals/ConfirmModal/ConfirmModal";
import { useEntityModal } from "../../shared/hooks/useEntityModal";
import { ActiveEntityMode } from "../../shared/enums/activeEntityMode";
import AddButton from "../../shared/components/AddButton/AddButton";

const CryptoAccountsPage: React.FC = () => {
    const { t } = useTranslation()
    
    const { 
        activeEntity,
        modalRef,
        confirmModalRef,
        onAddClicked,
        onEditClicked,
        onDeleteClicked,
        mode,
        onActionEnded
    } = useEntityModal<CryptoAccountEntity>();

    const {
        cryptoAccounts,
        createCryptoAccountEntity,
        updateCryptoAccountEntity,
        deleteCryptoAccountEntity
    } = useCryptoAccounts();

    const getAddButton = () => {
        return <AddButton buttonTitle={t("crypto_accounts_page_add")} onClick={onAddClicked}/>
    }

    const onCryptoAccountSaved = async (security: CryptoAccountEntity) => {
        if (mode === ActiveEntityMode.Add) {
            await createCryptoAccountEntity(security);
        } else if (mode === ActiveEntityMode.Edit) {
            await updateCryptoAccountEntity(security);
        }
        onActionEnded();
    }

    const onDeleteConfirmed = async () => {
		if (!activeEntity) {
            throw new Error("Deleted entity is not set")
        }

        await deleteCryptoAccountEntity(activeEntity);
		onActionEnded();
    }
    
    return (
        <Fragment>
            {
                cryptoAccounts.length ?
                    <Fragment>
                        <Flex justifyContent="space-between" alignItems="center" pt={5} pb={5}>
                            {getAddButton()}
                        </Flex>
                        <SimpleGrid pt={5} pb={5} gap={4} templateColumns='repeat(auto-fill, minmax(400px, 3fr))'>
                            {
                                cryptoAccounts.map((cryptoAccount: CryptoAccountEntity) => 
                                    <CryptoAccount cryptoAccount={cryptoAccount} 
                                        onEditClicked={onEditClicked} 
                                        onDeleteClicked={onDeleteClicked} 
                                        key={cryptoAccount.id}/>)
                            }
                        </SimpleGrid>
                        <ConfirmModal onConfirmed={onDeleteConfirmed}
                            title={t("broker_account_delete_title")}
                            message={t("modals_delete_message")}
                            confirmActionName={t("modals_delete_button")}
                            ref={confirmModalRef}/>
                    </Fragment>:
                    <Placeholder text={t("crypto_accounts_page_no_crypto_accounts")}>
                        {getAddButton()}
                    </Placeholder>
            }
            <CryptoAccountModal cryptoAccount={activeEntity} modalRef={modalRef} onSaved={onCryptoAccountSaved}/>
        </Fragment>
    );
}

export default CryptoAccountsPage;