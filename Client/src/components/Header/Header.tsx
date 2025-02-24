import './Header.scss';
import HeaderItem from '../HeaderItem/HeaderItem';

const Header = () => 
    <nav className="header">
        <HeaderItem title="Manager" path='/'></HeaderItem>
        <HeaderItem title="Deposits" path='/Deposits'></HeaderItem>
    </nav>

export default Header;