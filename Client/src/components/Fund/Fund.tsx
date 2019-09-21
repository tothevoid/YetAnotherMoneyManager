import React from 'react'
import './Fund.css'
import { FundEntity } from '../../models/FundEntity';

const Fund = (props: FundEntity) => {
    const {name, balance} = props;
    return <div className="fund-sub-container"> 
        <p className="fund-name">{name}</p>
        <p className="fund-funds">{balance}<i className="currency">&#8381;</i></p>
    </div>
};

export default Fund;