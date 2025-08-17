import { Flex, Text } from "@chakra-ui/react";
import { PropsWithChildren } from "react";

interface Props {
    text: string
}

const Placeholder: React.FC<PropsWithChildren<Props>> = ({text, children}) => {
    return <Flex gap={4} direction="column" alignItems="center" justifyContent="center" height="50vh" width="100%">
        <Text color="text_primary" fontSize='3xl'>{text}</Text>
        {children}
    </Flex>
}

export default Placeholder;