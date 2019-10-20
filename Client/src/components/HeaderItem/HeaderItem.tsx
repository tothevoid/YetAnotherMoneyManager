import React from 'react';
import './HeaderItem.scss';

type Props = {
    title: string
}

const HeaderItem = (props: Props) => {
    const {title} = props;
    return <a href="/" className="header-item">{title}</a>
}

export default HeaderItem;