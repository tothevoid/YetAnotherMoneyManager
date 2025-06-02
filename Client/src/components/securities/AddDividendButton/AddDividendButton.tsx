import { MdAdd } from "react-icons/md"
import { Button, Icon } from "@chakra-ui/react"
import { useRef } from "react"
import { Fragment } from "react/jsx-runtime"
import { useTranslation } from "react-i18next"
import DividendModal from "../../../modals/DividendModal/DividendModal"
import { DividendEntity } from "../../../models/securities/DividendEntity"
import { createDividend } from "../../../api/securities/dividendApi"
import { BaseModalRef } from "../../../common/ModalUtilities"

type Props = {
    securityId: string,
    onAdded: (dividend: DividendEntity) => void;
};

const AddDividendButton: React.FC<Props> = ({ onAdded, securityId }) => {
    const modalRef = useRef<BaseModalRef>(null);
    
    const onAdd = () => {
        modalRef.current?.openModal()
    };

    const onDividendAdded = async (dividend: DividendEntity) => {
        const createdDividend = await createDividend(dividend);
        if (!createdDividend) {
            return;
        }

        onAdded(createdDividend);
    };

    const { t } = useTranslation();

    const dividend: DividendEntity = { security: {id: securityId} };

    return (
        <Fragment>
            <Button background="purple.600" onClick={onAdd}>
                <Icon size='md'>
                    <MdAdd/>
                </Icon>
                {t("security_page_summary_add")}
            </Button>
            <DividendModal dividend={dividend} modalRef={modalRef} onSaved={onDividendAdded}/>
        </Fragment>
    )
}

export default AddDividendButton