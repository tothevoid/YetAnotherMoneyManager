import { Link } from '@chakra-ui/react';
import './HeaderItem.scss';

type Props = {
    title: string
    path: string
}

const HeaderItem = (props: Props) => {
    const {title} = props;
    return <Link color="text_primary" href={props.path} className="header-item">{title}</Link>
}

export default HeaderItem;