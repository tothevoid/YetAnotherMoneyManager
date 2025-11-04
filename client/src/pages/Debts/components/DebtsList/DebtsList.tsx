import { useCallback, useEffect } from "react";
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
        debtQueryParameters,
        setDebtQueryParameters,
        reloadDebts
    } = useDebts({onlyActive: true});

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

    const onOnlyActiveSwitched = (onlyActive: boolean) => {
        setDebtQueryParameters({onlyActive});
    }

    const getAddButton = () => {
        return <AddButton buttonTitle={t("debts_page_add_debt")} onClick={onAddClicked}/>
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

    return debts.length > 0 ?
        <Box>
            <Flex justifyContent="space-between">
                <SwitchButton active={debtQueryParameters.onlyActive} title={t("debts_page_only_active")} onSwitch={onOnlyActiveSwitched}/>
                {debts.length && getAddButton()}
            </Flex>
            <SimpleGrid pt={5} pb={5} gap={6} templateColumns='repeat(auto-fill, minmax(300px, 4fr))'>
            {
                debts.map((debt: DebtEntity) => 
                    <Debt key={debt.id} debt={debt} 
                        onEditClicked={onEditClicked}
                        onDeleteClicked={onDeleteClicked}/>
                )
            }
            </SimpleGrid>
            <ConfirmModal onConfirmed={onDeleteConfirmed}
                title={t("security_delete_title")}
                message={t("modals_delete_message")}
                confirmActionName={t("modals_delete_button")}
                ref={confirmModalRef}/>
            <DebtModal debt={activeEntity} modalRef={modalRef} onSaved={onDebtSaved}/>
        </Box>:
        <Placeholder text={t("debts_page_no_debts")}>{getAddButton()}</Placeholder>
}

export default DebtsList;