import React from 'react'
import './AddFundButton.css'

const AddFundButton = (props: any) =>{
    const {onClickCallback} = props;
    return <button className="add-fund-button" onClick={onClickCallback}>+</button>
}

export default AddFundButton;