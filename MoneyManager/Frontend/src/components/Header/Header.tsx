import React from 'react';
import './Header.css';
import HeaderItem from '../HeaderItem/HeaderItem';

const Header: React.FC = () => {
    return <nav className="header">
        <HeaderItem title="abc"></HeaderItem>
    </nav>
}

export default Header;