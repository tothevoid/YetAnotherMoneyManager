import { Fragment, useRef } from "react";
import { Box} from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { DebtPaymentEntity } from "../../../../models/debts/DebtPaymentEntity";
import ShowModalButton from "../../../../shared/components/ShowModalButton/ShowModalButton";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import { useDebtPayments } from "../../hooks/useDebtPayments";
import DebtPaymentModal from "../../modals/DebtPaymentModal/DebtPaymentModal";
import DebtPayment from "../DebtPayment/DebtPayment";


const DebtsPaymentsList: React.FC = () => {
    const { t } = useTranslation();

    const {
        debtPayments,
        createDebtPaymentEntity,
        updateDebtPaymentEntity,
        deleteDebtPaymentEntity,
    } = useDebtPayments();

    const debtPaymentModalRef = useRef<BaseModalRef>(null);
    
    const onAddDebtPayment = () => {
        debtPaymentModalRef.current?.openModal()
    };

    return <Fragment>
        <ShowModalButton buttonTitle={t("debts_page_add_payment")} onClick={onAddDebtPayment}>
            <DebtPaymentModal modalRef={debtPaymentModalRef} onSaved={createDebtPaymentEntity}/>
        </ShowModalButton>
        <Box>
            {
                debtPayments.map((payment: DebtPaymentEntity) => 
                    <DebtPayment key={payment.id} debtPayment={payment}
                        onEditCallback={updateDebtPaymentEntity} 
                        onDeleteCallback={deleteDebtPaymentEntity}/>
                )
            }
        </Box>
    </Fragment>
}

export default DebtsPaymentsList;