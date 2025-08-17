import { Fragment, useRef } from "react";
import { useTranslation } from "react-i18next";
import { Flex, SimpleGrid } from "@chakra-ui/react";
import ShowModalButton from "../../shared/components/ShowModalButton/ShowModalButton";
import { BaseModalRef } from "../../shared/utilities/modalUtilities";
import { useCryptocurrencies } from "./hooks/useCryptocurrencies";
import { CryptocurrencyEntity } from "../../models/crypto/CryptocurrencyEntity";
import Cryptocurrency from "./components/Cryptocurrency/Cryptocurrency";
import CryptocurrencyModal from "./modals/CryptocurrencyModal";
import Placeholder from "../../shared/components/Placeholder/Placeholder";

const CryptocurrenciesPage: React.FC = () => {
    const { t } = useTranslation();
    
    const {
        cryptocurrencies,
        createCryptocurrencyEntity,
        updateCryptocurrencyEntity,
        deleteCryptocurrencyEntity,
        reloadCryptocurrencies
    } = useCryptocurrencies();

    const onReloadCryptocurrencies = async () => {
        await reloadCryptocurrencies();
    }

    const modalRef = useRef<BaseModalRef>(null);
    
    const onAdd = () => {
        modalRef.current?.openModal()
    };

    const getAddButton = () => {
        return <ShowModalButton buttonTitle={t("security_page_summary_add")} onClick={onAdd}>
            <CryptocurrencyModal modalRef={modalRef} onSaved={createCryptocurrencyEntity}/>
        </ShowModalButton>
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
                    <Cryptocurrency key={cryptocurrency.id} cryptocurrency={cryptocurrency} 
                        onEditCallback={updateCryptocurrencyEntity} 
                        onDeleteCallback={deleteCryptocurrencyEntity}
                        onReloadSecurities={onReloadCryptocurrencies}/>)
            }
        </SimpleGrid>
    </Fragment>
}

export default CryptocurrenciesPage;