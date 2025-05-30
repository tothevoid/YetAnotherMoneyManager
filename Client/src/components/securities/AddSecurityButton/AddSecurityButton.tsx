import { MdAdd } from "react-icons/md"
import { Button, Icon } from "@chakra-ui/react"
import { useRef } from "react"

import { Fragment } from "react/jsx-runtime"
import { AccountModalRef } from "../../../modals/AccountModal/AccountModal"
import { useTranslation } from "react-i18next"
import SecurityModal from "../modals/SecurityModal/SecurityModal"
import { createSecurity } from "../../../api/securities/securityApi"
import { SecurityEntity } from "../../../models/securities/SecurityEntity"

type BrokerAccountProps = {
    onAdded: (security: SecurityEntity) => void;
};

const AddBrokerAccountButton: React.FC<BrokerAccountProps> = ({ onAdded }) => {
    const modalRef = useRef<AccountModalRef>(null);
    
    const onAdd = () => {
        modalRef.current?.openModal()
    };

    const onSecurityAdded = async (security: SecurityEntity, icon: File | null) => {
        const createdSecurity = await createSecurity(security, icon);
        if (!createdSecurity) {
            return;
        }

        onAdded(createdSecurity);
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
            <SecurityModal ref={modalRef} onSaved={onSecurityAdded}></SecurityModal>
        </Fragment>
    )
}

export default AddBrokerAccountButton