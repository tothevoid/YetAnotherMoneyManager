import { MdAdd } from "react-icons/md"
import { Button, Icon } from "@chakra-ui/react"
import { useRef } from "react"

import { Fragment } from "react/jsx-runtime"
import { AccountModalRef } from "../../../modals/AccountModal/AccountModal"
import { useTranslation } from "react-i18next"
import { BrokerAccountSecurityEntity } from "../../../models/brokers/BrokerAccountSecurityEntity"
import { createBrokerAccountSecurity } from "../../../api/brokers/brokerAccountSecurityApi"
import BrokerAccountSecurityModal from "../modals/BrokerAccountSecurityModal/BrokerAccountSecurityModal"

type Props = {
    onAdded: (brokerAccountSecurity: BrokerAccountSecurityEntity) => void;
};

const AddBrokerAccountSecurityButton: React.FC<Props> = ({ onAdded }) => {
    const modalRef = useRef<AccountModalRef>(null);
    
    const onAdd = () => {
        modalRef.current?.openModal()
    };

    const onBrokerAccountSecurityAdded = async (brokerAccountSecurity: BrokerAccountSecurityEntity) => {
        const createdBrokerAccountSecurity = await createBrokerAccountSecurity(brokerAccountSecurity);
        if (!createdBrokerAccountSecurity) {
            return;
        }

        onAdded(createdBrokerAccountSecurity);
    };

    const { t } = useTranslation();

    return (
        <Fragment>
            <Button background="purple.600" onClick={onAdd}>
                <Icon size='md'>
                    <MdAdd/>
                </Icon>
                {t("broker_account_securities_page_summary_add")}
            </Button>
            <BrokerAccountSecurityModal ref={modalRef} onSaved={onBrokerAccountSecurityAdded}></BrokerAccountSecurityModal>
        </Fragment>
    )
}

export default AddBrokerAccountSecurityButton