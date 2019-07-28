import React, { Fragment } from 'react'
import './Fund.css'

const Fund = (props: any) => {
    const {name, balance} = props;
    return <Fragment> 
        <p className="fund-name">{name}</p>
        <p className="fund-funds">{balance}<i className="currency">&#8381;</i></p>
    </Fragment>
};

export default Fund;