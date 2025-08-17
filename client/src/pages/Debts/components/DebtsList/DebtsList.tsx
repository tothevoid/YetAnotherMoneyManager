import { useEffect, useRef } from "react";
import { SimpleGrid, Box, Flex} from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { DebtEntity } from "../../../../models/debts/DebtEntity";
import ShowModalButton from "../../../../shared/components/ShowModalButton/ShowModalButton";
import SwitchButton from "../../../../shared/components/SwitchButton/SwitchButton";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import { useDebts } from "../../hooks/useDebts";
import DebtModal from "../../modals/DebtModal.tsx/DebtModal";
import Debt from "../Debt/Debt";
import Placeholder from "../../../../shared/components/Placeholder/Placeholder";

interface Props {
    onDebtsChanged: (debts: number) => void
}

const DebtsList: React.FC<Props> = ({onDebtsChanged}) => {
    const { t } = useTranslation();

    const {
        debts,
        createDebtEntity,
        updateDebtEntity,
        deleteDebtEntity,
        debtQueryParameters,
        setDebtQueryParameters
    } = useDebts({onlyActive: true});

    const modalRef = useRef<BaseModalRef>(null);

    useEffect(()=> {
        onDebtsChanged(debts.length);
    }, [debts, onDebtsChanged])

    const onAdd = () => {
        modalRef.current?.openModal()
    };

    const onOnlyActiveSwitched = (onlyActive: boolean) => {
        setDebtQueryParameters({onlyActive});
    }

    const getAddButton = () => {
        return <ShowModalButton buttonTitle={t("debts_page_add_debt")} onClick={onAdd}>
            <DebtModal modalRef={modalRef} onSaved={createDebtEntity}/>
        </ShowModalButton>
    }

    if (!debts.length) {
        return <Placeholder text={t("debts_page_no_debts")}>
            {getAddButton()}
        </Placeholder>
    }

    return (
        <Box>
            <Flex justifyContent="space-between">
                <SwitchButton active={debtQueryParameters.onlyActive} title={t("debts_page_only_active")} onSwitch={onOnlyActiveSwitched}/>
                {debts.length && getAddButton()}
            </Flex>
            <SimpleGrid pt={5} pb={5} gap={6} templateColumns='repeat(auto-fill, minmax(300px, 4fr))'>
            {
                debts.map((debt: DebtEntity) => 
                    <Debt key={debt.id} debt={debt} 
                        onEditCallback={updateDebtEntity}
                        onDeleteCallback={deleteDebtEntity}/>
                )
            }
            </SimpleGrid>
        </Box>
    )
}

export default DebtsList;