import React from 'react';
import './HeaderItem.css';

type HeaderProps = {
    title: string
}

const HeaderItem: React.FC<HeaderProps> = ({title}) => {
    return <a href="/" className="header-item">{title}</a>
}

export default HeaderItem;