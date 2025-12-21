import { useCallback, useEffect, useState } from "react";
import { SimpleGrid, Box, Flex} from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { DebtEntity } from "../../../../models/debts/DebtEntity";
import SwitchButton from "../../../../shared/components/SwitchButton/SwitchButton";
import { useDebts } from "../../hooks/useDebts";
import DebtModal from "../../modals/DebtModal.tsx/DebtModal";
import Debt from "../Debt/Debt";
import Placeholder from "../../../../shared/components/Placeholder/Placeholder";
import { ConfirmModal } from "../../../../shared/modals/ConfirmModal/ConfirmModal";
import { useEntityModal } from "../../../../shared/hooks/useEntityModal";
import AddButton from "../../../../shared/components/AddButton/AddButton";
import { ActiveEntityMode } from "../../../../shared/enums/activeEntityMode";

interface Props {
    debtsPaymentsVersion: number,
    onDebtsChanged: (debts: number) => void
}

const DebtsList: React.FC<Props> = ({debtsPaymentsVersion, onDebtsChanged}) => {
    const { t } = useTranslation();

    const [onlyActive, setOnlyActive] = useState(true);

    const { 
        activeEntity,
        modalRef,
        confirmModalRef,
        onAddClicked,
        onEditClicked,
        onDeleteClicked,
        mode,
        onActionEnded
    } = useEntityModal<DebtEntity>();

    const {
        debts,
        createDebtEntity,
        updateDebtEntity,
        deleteDebtEntity,
        reloadDebts
    } = useDebts();

    const reloadData = useCallback(async () => {
        await reloadDebts();
    }, [reloadDebts]);

    useEffect(() => {
        if (debtsPaymentsVersion >= 0){
            reloadData();
        }
    }, [debtsPaymentsVersion, reloadData]);

    useEffect(()=> {
        onDebtsChanged(debts.length);
    }, [debts, onDebtsChanged])

    const getHeader = () => {
        const addButton = <AddButton buttonTitle={t("debts_page_add_debt")} 
            onClick={onAddClicked}/>

        return debts.length > 0 ?
            <Flex justifyContent="space-between">
                <SwitchButton active={onlyActive} title={t("debts_page_only_active")} onSwitch={onOnlyActiveSwitched}/>
                {debts.length && addButton}
            </Flex>:
            <Placeholder text={t("debts_page_no_debts")}>{addButton}</Placeholder>
    }

    const onOnlyActiveSwitched = (onlyActive: boolean) => {
        setOnlyActive(onlyActive);
    }

    const onDebtSaved = async (debt: DebtEntity) => {
        if (mode === ActiveEntityMode.Add) {
            await createDebtEntity(debt);
        } else if (mode === ActiveEntityMode.Edit) {
            await updateDebtEntity(debt);
        }
        onActionEnded();
    }
    
	const onDeleteConfirmed = async () => {
		if (!activeEntity) {
            throw new Error("Deleted entity is not set")
        }

        await deleteDebtEntity(activeEntity);
		onActionEnded();
    }

    return <Box>
        {getHeader()}
        <SimpleGrid pt={5} pb={5} gap={6} templateColumns='repeat(auto-fill, minmax(300px, 4fr))'>
        {
            debts.filter(debt => !onlyActive || debt.amount).map((debt: DebtEntity) => 
                <Debt key={debt.id} debt={debt} 
                    onEditClicked={onEditClicked}
                    onDeleteClicked={onDeleteClicked}/>
            )
        }
        </SimpleGrid>
        <ConfirmModal onConfirmed={onDeleteConfirmed}
            title={t("debts_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}/>
        <DebtModal debt={activeEntity} modalRef={modalRef} onSaved={onDebtSaved}/>
    </Box>
        
}

export default DebtsList;