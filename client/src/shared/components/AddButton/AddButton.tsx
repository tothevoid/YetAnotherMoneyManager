import { Button } from "@chakra-ui/react"
import React from "react"
import { MdAdd } from "react-icons/md"

interface Props {
    buttonTitle: string,
    onClick: () => void
}

const AddButton: React.FC<Props> = (props: Props) => {
    return <Button background="action_primary" onClick={() => props.onClick()}>
        <MdAdd/>{props.buttonTitle}
    </Button>
}

export default AddButton;