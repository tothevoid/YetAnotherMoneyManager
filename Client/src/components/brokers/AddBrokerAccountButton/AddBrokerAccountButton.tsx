import { MdAdd } from "react-icons/md"
import { Button, Icon } from "@chakra-ui/react"
import { useRef } from "react"

import { Fragment } from "react/jsx-runtime"
import { AccountModalRef } from "../../../modals/AccountModal/AccountModal"
import { useTranslation } from "react-i18next"
import { BrokerAccountEntity } from "../../../models/brokers/BrokerAccountEntity"
import { createBrokerAccount } from "../../../api/brokers/brokerAccountApi"
import BrokerAccountModal from "../modals/BrokerAccountModal/BrokerAccountModal"

type BrokerAccountProps = {
    onAdded: (account: BrokerAccountEntity) => void;
};

const AddBrokerAccountButton: React.FC<BrokerAccountProps> = ({ onAdded }) => {
    const modalRef = useRef<AccountModalRef>(null);
    
    const onAdd = () => {
        modalRef.current?.openModal()
    };

    const onAccountAdded = async (brokerAccount: BrokerAccountEntity) => {
        const createdBrokerAccount = await createBrokerAccount(brokerAccount);
        if (!createdBrokerAccount) {
            return;
        }

        onAdded(createdBrokerAccount);
    };

    const { t } = useTranslation();

    return (
        <Fragment>
            <Button background="purple.600" onClick={onAdd}>
                <Icon size='md'>
                    <MdAdd/>
                </Icon>
                {t("brocker_accounts_page_summary_add")}
            </Button>
            <BrokerAccountModal ref={modalRef} onSaved={onAccountAdded}></BrokerAccountModal>
        </Fragment>
    )
}

export default AddBrokerAccountButton