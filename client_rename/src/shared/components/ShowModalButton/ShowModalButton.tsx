import { Button } from "@chakra-ui/react"
import { Fragment } from "react/jsx-runtime"
import React from "react"
import { MdAdd } from "react-icons/md"

interface Props {
    children: React.ReactNode,
    buttonTitle: string,
    onClick: () => void
}

const ShowModalButton: React.FC<Props> = (props: Props) => {
    return <Fragment>
        <Button background="purple.600" onClick={() => props.onClick()}>
            <MdAdd/>{props.buttonTitle}
        </Button>
        {props.children}
    </Fragment>
}

export default ShowModalButton;