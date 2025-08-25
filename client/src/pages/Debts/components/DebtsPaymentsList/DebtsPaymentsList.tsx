import { Fragment } from "react";
import { Box} from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { DebtPaymentEntity } from "../../../../models/debts/DebtPaymentEntity";
import { useDebtPayments } from "../../hooks/useDebtPayments";
import DebtPaymentModal from "../../modals/DebtPaymentModal/DebtPaymentModal";
import DebtPayment from "../DebtPayment/DebtPayment";
import { useEntityModal } from "../../../../shared/hooks/useEntityModal";
import { ConfirmModal } from "../../../../shared/modals/ConfirmModal/ConfirmModal";
import { ActiveEntityMode } from "../../../../shared/enums/activeEntityMode";
import AddButton from "../../../../shared/components/AddButton/AddButton";


const DebtsPaymentsList: React.FC = () => {
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
    } = useEntityModal<DebtPaymentEntity>();

    const {
        debtPayments,
        createDebtPaymentEntity,
        updateDebtPaymentEntity,
        deleteDebtPaymentEntity,
    } = useDebtPayments();

    const onDebtPaymentSaved = async (createdDebtPayment: DebtPaymentEntity) => {
        if (mode === ActiveEntityMode.Add) {
            await createDebtPaymentEntity(createdDebtPayment);
        } else if (mode === ActiveEntityMode.Edit) {
            await updateDebtPaymentEntity(createdDebtPayment);
        }
        onActionEnded();
    }

    const onDeleteConfirmed = async () => {
		if (!activeEntity) {
            throw new Error("Deleted entity is not set")
        }

        await deleteDebtPaymentEntity(activeEntity);
		onActionEnded();
    }

    return <Fragment>
        <AddButton buttonTitle={t("debts_page_add_payment")} onClick={onAddClicked}/>
        <Box>
            {
                debtPayments.map((payment: DebtPaymentEntity) => 
                    <DebtPayment key={payment.id} debtPayment={payment}
                        onEditClicked={onEditClicked} 
                        onDeleteClicked={onDeleteClicked}/>
                )
            }
        </Box>
        <ConfirmModal onConfirmed={onDeleteConfirmed}
            title={t("debt_payment_modal_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}/>
        <DebtPaymentModal debtPayment={activeEntity} modalRef={modalRef} onSaved={onDebtPaymentSaved}/>
    </Fragment>
}

export default DebtsPaymentsList;