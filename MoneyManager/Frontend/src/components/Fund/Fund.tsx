import React, { Fragment } from 'react'
import './Fund.css'

const Fund = (props: any) => {
    const {name, funds} = props;
    return <Fragment> 
        <p className="fund-name">{name}</p>
        <p className="fund-funds">{funds}<i className="currency">&#8381;</i></p>
    </Fragment>
};

export default Fund;