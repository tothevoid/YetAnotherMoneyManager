import { Fragment, useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import { Flex, SimpleGrid } from "@chakra-ui/react";
import { BaseModalRef } from "../../shared/utilities/modalUtilities";
import { useCryptocurrencies } from "./hooks/useCryptocurrencies";
import { CryptocurrencyEntity } from "../../models/crypto/CryptocurrencyEntity";
import Cryptocurrency from "./components/Cryptocurrency/Cryptocurrency";
import CryptocurrencyModal from "./modals/CryptocurrencyModal";
import Placeholder from "../../shared/components/Placeholder/Placeholder";
import { ConfirmModal } from "../../shared/modals/ConfirmModal/ConfirmModal";
import { Nullable } from "../../shared/utilities/nullable";
import AddButton from "../../shared/components/AddButton/AddButton";
import { ActiveEntityMode } from "../../shared/enums/activeEntityMode";

const CryptocurrenciesPage: React.FC = () => {
    const { t } = useTranslation();
    
    const {
        cryptocurrencies,
        createCryptocurrencyEntity,
        updateCryptocurrencyEntity,
        deleteCryptocurrencyEntity
    } = useCryptocurrencies();

    const modalRef = useRef<BaseModalRef>(null);
    const confirmModalRef = useRef<BaseModalRef>(null);

    const [activeCryptocurrency, setActiveCryptocurrency] = useState<Nullable<CryptocurrencyEntity>>(null);
    const [activeEntityMode, setActiveEntityMode] = useState<ActiveEntityMode>(ActiveEntityMode.None);

    const onCryptocurrencySaved = (cryptocurrency: CryptocurrencyEntity, file: File | null) => {
        if (activeEntityMode === ActiveEntityMode.Add) {
            createCryptocurrencyEntity(cryptocurrency, file);
        } else {
            updateCryptocurrencyEntity(cryptocurrency, file);
        }
    }

    const onAddClicked = () => {
        setActiveEntityMode(ActiveEntityMode.Add);
        modalRef.current?.openModal()
    };

    const onEditClicked = (cryptocurrency: CryptocurrencyEntity) => {
        setActiveCryptocurrency(cryptocurrency);
        setActiveEntityMode(ActiveEntityMode.Edit);
        modalRef.current?.openModal();
    };

    const onDeleteClicked = (cryptocurrency: CryptocurrencyEntity) => {
        setActiveCryptocurrency(cryptocurrency);
        setActiveEntityMode(ActiveEntityMode.Delete);
        confirmModalRef.current?.openModal();
    };

    const onDeleteConfirmed = async () => {
        await deleteCryptocurrencyEntity(activeCryptocurrency!)
    }

    const getAddButton = () => {
        return <AddButton onClick={onAddClicked} buttonTitle={t("security_page_summary_add")}/>
    }

    if (!cryptocurrencies.length) {
        return <Placeholder text={t("cryptocurrencies_page_no_cryptocurrencies")}>
            {getAddButton()}
        </Placeholder>
    }

    return <Fragment>
        <Flex justifyContent="space-between" alignItems="center" pt={5} pb={5}>
           {getAddButton()}
        </Flex>
        <SimpleGrid pt={5} pb={5} gap={4} templateColumns='repeat(auto-fill, minmax(300px, 3fr))'>
            {
                cryptocurrencies.map((cryptocurrency: CryptocurrencyEntity) => 
                    <Cryptocurrency key={cryptocurrency.id} 
                        cryptocurrency={cryptocurrency} 
                        onEditClicked={onEditClicked} 
                        onDeletedClicked={onDeleteClicked}/>)
            }
        </SimpleGrid>
        <ConfirmModal onConfirmed={onDeleteConfirmed}
            title={t("security_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}/>
        <CryptocurrencyModal cryptocurrency={activeCryptocurrency} modalRef={modalRef} onSaved={onCryptocurrencySaved}/>
    </Fragment>
}

export default CryptocurrenciesPage;