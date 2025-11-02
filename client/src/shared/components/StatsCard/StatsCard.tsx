import { Card } from "@chakra-ui/react"
import React from "react"
import { Nullable } from "../../utilities/nullable"

interface Props {
    title: string,
    color?: Nullable<string>,
    value: number | string
}

const StatsCard: React.FC<Props> = ({title, value, color}) => {
    return <Card.Root backgroundColor="background_primary" borderColor="border_primary" color="text_primary">
        <Card.Header>
            {title}
        </Card.Header>
        <Card.Body fontSize="xl" fontWeight={700} color={color ? color : {}}>
            {value}
        </Card.Body>
    </Card.Root>
}

export default StatsCard;