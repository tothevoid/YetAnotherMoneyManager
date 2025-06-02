import { MdAdd } from "react-icons/md"
import { Button, Icon } from "@chakra-ui/react"
import { useRef } from "react"

import { Fragment } from "react/jsx-runtime"
import { useTranslation } from "react-i18next"
import { BrokerAccountEntity } from "../../../models/brokers/BrokerAccountEntity"
import { createBrokerAccount } from "../../../api/brokers/brokerAccountApi"
import BrokerAccountModal from "../modals/BrokerAccountModal/BrokerAccountModal"
import { BaseModalRef } from "../../../common/ModalUtilities"

type BrokerAccountProps = {
    onAdded: (account: BrokerAccountEntity) => void;
};

const AddBrokerAccountButton: React.FC<BrokerAccountProps> = ({ onAdded }) => {
    const modalRef = useRef<BaseModalRef>(null);
    
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
                {t("broker_accounts_page_summary_add")}
            </Button>
            <BrokerAccountModal modalRef={modalRef} onSaved={onAccountAdded}/>
        </Fragment>
    )
}

export default AddBrokerAccountButton