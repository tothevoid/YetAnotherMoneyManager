import React from 'react'
import './FundContainer.scss'

const getClassName = (isNew: boolean) =>{
    const additionalClass = (isNew) ? 
        "fund-container-new":"fund-container-stored"
    return `fund-container ${additionalClass}`;
}

const FundContainer = (isNew: boolean = false, onClick: any = null) => (Child: any) =>
    ({...props}) =>{
        const onDivClicked = () => {if (onClick) onClick()};
        return <div onClick={onDivClicked} className={getClassName(isNew)}>
            <Child {...props}></Child>
        </div>
    }

export default FundContainer;