import { Button } from "@chakra-ui/react"
import { Fragment } from "react/jsx-runtime"
import TransactionModal, { TransactionModalRef } from "../../modals/TransactionModal/TransactionModal"
import { TransactionEntity } from "../../models/TransactionEntity"
import { useRef } from "react"
import { FundEntity } from "../../models/FundEntity"
import { MdAdd } from "react-icons/md"
import { createTransaction } from "../../api/transactionApi"

type Props = {
    fundSources: FundEntity[],
    onTransactionCreated: (transaction: TransactionEntity) => void
}

const AddTransactionButton: React.FC<Props> = (props: Props) => {
    const addTransactionModalRef = useRef<TransactionModalRef>(null);
            
    const onAddTransactionClick = () => {
        addTransactionModalRef.current?.openModal()
    }

    const onTransactionAdded = async (transaction: TransactionEntity) => {
        const createdTransaction = await createTransaction(transaction);
        if (!createdTransaction) {
            return;
        }

        props.onTransactionCreated(createdTransaction);
    }

    return <Fragment>
        <Button background="purple.600" onClick={() => onAddTransactionClick()}>
            <MdAdd/>Add transaction
        </Button>
        <TransactionModal ref={addTransactionModalRef} 
            fundSources={props.fundSources}
            onSaved={onTransactionAdded}></TransactionModal>
    </Fragment>
}

export default AddTransactionButton;