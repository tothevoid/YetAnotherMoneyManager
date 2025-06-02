import { MdAdd } from "react-icons/md"
import { Button, Icon } from "@chakra-ui/react"
import { useRef } from "react"
import { Fragment } from "react/jsx-runtime"
import { useTranslation } from "react-i18next"
import { ClientDebtPaymentEntity } from "../../../models/debts/DebtPaymentEntity"
import { createDebtPayment } from "../../../api/debts/debtPaymentApi"
import DebtPaymentModal from "../modals/DebtPaymentModal/DebtPaymentModal"
import { BaseModalRef } from "../../../common/ModalUtilities"

type Props = {
    onAdded: (debt: ClientDebtPaymentEntity) => void;
};

const AddDebtPaymentButton: React.FC<Props> = ({ onAdded }) => {
    const modalRef = useRef<BaseModalRef>(null);
    
    const onAdd = () => {
        modalRef.current?.openModal()
    };

    const onDebtAdded = async (debt: ClientDebtPaymentEntity) => {
        const createdDebtPayment = await createDebtPayment(debt);
        if (!createdDebtPayment) {
            return;
        }

        onAdded(createdDebtPayment);
    };

    const { t } = useTranslation();

    return (
        <Fragment>
            <Button background="purple.600" onClick={onAdd}>
                <Icon size='md'>
                    <MdAdd/>
                </Icon>
                {t("security_page_summary_add")}
            </Button>
            <DebtPaymentModal modalRef={modalRef} onSaved={onDebtAdded}/>
        </Fragment>
    )
}

export default AddDebtPaymentButton;