import { Button } from "@chakra-ui/react"
import { Fragment } from "react/jsx-runtime"
import TransactionModal, { TransactionModalRef } from "../../modals/TransactionModal/TransactionModal"
import { AddIcon } from "@chakra-ui/icons"
import { TransactionEntity } from "../../models/TransactionEntity"
import { useRef } from "react"
import { TransactionType } from "../../models/TransactionType"
import { FundEntity } from "../../models/FundEntity"

type Props = {
    fundSources: FundEntity[],
    transactionTypes: TransactionType[],
    onTypeAdded: (type: TransactionType) => void
    onTransactionCreated: (transaction: TransactionEntity) => void
}

const AddTransactionButton: React.FC<Props> = (props: Props) => {
    const addTransactionModalRef = useRef<TransactionModalRef>(null);
            
    const onAddTransactionClick = () => {
        addTransactionModalRef.current?.openModal()
    }

    return <Fragment>
        <Button leftIcon={<AddIcon/>} onClick={() => onAddTransactionClick()}>Add transaction</Button>
        <TransactionModal ref={addTransactionModalRef} 
            // onTypeAdded={props.onTypeAdded} 
            transactionTypes={props.transactionTypes} fundSources={props.fundSources}
            onSaved={props.onTransactionCreated}></TransactionModal>
    </Fragment>
}

export default AddTransactionButton;