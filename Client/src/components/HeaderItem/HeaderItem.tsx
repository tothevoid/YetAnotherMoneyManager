import './HeaderItem.scss';

type Props = {
    title: string
    path: string
}

const HeaderItem = (props: Props) => {
    const {title} = props;
    return <a href={props.path} className="header-item">{title}</a>
}

export default HeaderItem;