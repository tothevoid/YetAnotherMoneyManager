import { Button } from "@chakra-ui/react"
import { Fragment } from "react/jsx-runtime"
import TransactionModal, { TransactionModalRef } from "../../modals/TransactionModal/TransactionModal"
import { TransactionEntity } from "../../models/TransactionEntity"
import { useRef } from "react"
import { FundEntity } from "../../models/FundEntity"
import { MdAdd } from "react-icons/md"

type Props = {
    fundSources: FundEntity[],
    onTransactionCreated: (transaction: TransactionEntity) => void
}

const AddTransactionButton: React.FC<Props> = (props: Props) => {
    const addTransactionModalRef = useRef<TransactionModalRef>(null);
            
    const onAddTransactionClick = () => {
        addTransactionModalRef.current?.openModal()
    }

    return <Fragment>
        <Button background="purple.600" onClick={() => onAddTransactionClick()}>
            <MdAdd/>Add transaction
        </Button>
        <TransactionModal ref={addTransactionModalRef} 
            fundSources={props.fundSources}
            onSaved={props.onTransactionCreated}></TransactionModal>
    </Fragment>
}

export default AddTransactionButton;