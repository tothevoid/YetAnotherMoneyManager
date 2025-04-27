import { MdAdd } from "react-icons/md"
import { Button, Icon } from "@chakra-ui/react"
import { useRef } from "react"
import { Fragment } from "react/jsx-runtime"
import { AccountModalRef } from "../../../modals/AccountModal/AccountModal"
import { useTranslation } from "react-i18next"
import { SecurityTransactionEntity } from "../../../models/securities/SecurityTransactionEntity"
import { createSecurityTransaction } from "../../../api/securities/securityTransactionApi"
import SecurityTransactionModal from "../modals/SecurityTransactionModal/SecurityTransactionModal"

type Props = {
    onAdded: (security: SecurityTransactionEntity) => void;
};

const AddSecurityTransactionButton: React.FC<Props> = ({ onAdded }) => {
    const modalRef = useRef<AccountModalRef>(null);
    
    const onAdd = () => {
        modalRef.current?.openModal()
    };

    const onSecurityTransactionAdded = async (securityTransaction: SecurityTransactionEntity) => {
        const transaction = await createSecurityTransaction(securityTransaction);
        if (!transaction) {
            return;
        }

        onAdded(transaction);
    };

    const { t } = useTranslation();

    return (
        <Fragment>
            <Button background="purple.600" onClick={onAdd}>
                <Icon size='md'>
                    <MdAdd/>
                </Icon>
                {t("entity_securities_transaction_page_summary_add")}
            </Button>
            <SecurityTransactionModal ref={modalRef} onSaved={onSecurityTransactionAdded}></SecurityTransactionModal>
        </Fragment>
    )
}

export default AddSecurityTransactionButton