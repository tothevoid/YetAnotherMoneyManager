import { Button, Icon } from "@chakra-ui/react";
import "./RefreshButton.scss"
import { MdRefresh } from "react-icons/md";

interface Props {
    title?: string,
    isRefreshing: boolean,
    transparent?: boolean,
    onClick: () => void
}

const RefreshButton: React.FC<Props> = ({title, isRefreshing, transparent, onClick}) => {
    const background = transparent ? "transparent" : "action_primary"
    return <Button disabled={isRefreshing} background={background} onClick={onClick}>
        <Icon 
            transition="transform 0.3s ease"
            animation={isRefreshing ? 'loading-spin 1.5s linear infinite' : 'none'}
            size='md'>
            <MdRefresh/>
        </Icon>
        {title}
    </Button>
}

export default RefreshButton;