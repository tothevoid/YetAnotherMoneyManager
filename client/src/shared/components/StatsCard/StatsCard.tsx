import { Card } from "@chakra-ui/react"
import React from "react"

interface Props {
    title: string,
    value: number | string
}

const StatsCard: React.FC<Props> = ({title, value}) => {
    return <Card.Root backgroundColor="background_primary" borderColor="border_primary" color="text_primary">
        <Card.Header>
            {title}
        </Card.Header>
        <Card.Body fontSize="xl" fontWeight={700}>
            {value}
        </Card.Body>
    </Card.Root>
}

export default StatsCard;