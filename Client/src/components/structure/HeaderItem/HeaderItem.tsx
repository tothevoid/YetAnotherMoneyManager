import { Link } from '@chakra-ui/react';
import './HeaderItem.scss';

type Props = {
    title: string
    active: boolean
}

const HeaderItem = (props: Props) => {
    const {title} = props;

    const additionalConfig = props.active ? {
        background: "background_primary",
        borderRadius: "10px"
    }: {}

    return <Link {...additionalConfig} color="text_primary" className="header-item">{title}</Link>
}

export default HeaderItem;