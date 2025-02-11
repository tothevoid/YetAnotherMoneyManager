import './Fund.scss'
import { FundEntity } from '../../models/FundEntity';

const Fund = (props: FundEntity) => {
    const {name, balance} = props;
    return <div className="fund"> 
        <p className="fund-name">{name}</p>
        <p className="fund-balance">{balance}<i className="currency">&#8381;</i></p>
    </div>
};

export default Fund;