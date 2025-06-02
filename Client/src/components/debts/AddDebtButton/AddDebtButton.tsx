import { MdAdd } from "react-icons/md"
import { Button, Icon } from "@chakra-ui/react"
import { useRef } from "react"
import { Fragment } from "react/jsx-runtime"
import { useTranslation } from "react-i18next"
import { ClientDebtEntity } from "../../../models/debts/DebtEntity"
import { createDebt } from "../../../api/debts/debtApi"
import DebtModal from "../modals/DebtModal.tsx/DebtModal"
import { BaseModalRef } from "../../../common/ModalUtilities"

type Props = {
    onAdded: (debt: ClientDebtEntity) => void;
};

const AddDebtButton: React.FC<Props> = ({ onAdded }) => {
    const modalRef = useRef<BaseModalRef>(null);
    
    const onAdd = () => {
        modalRef.current?.openModal()
    };

    const onDebtAdded = async (debt: ClientDebtEntity) => {
        const createdDebt = await createDebt(debt);
        if (!createdDebt) {
            return;
        }

        onAdded(createdDebt);
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
            <DebtModal modalRef={modalRef} onSaved={onDebtAdded}/>
        </Fragment>
    )
}

export default AddDebtButton