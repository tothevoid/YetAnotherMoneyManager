import React from 'react'
import './FundContainer.css'

const getClassName = (isNew: boolean) =>{
    const additionalClass = (isNew) ? 
        "container-new":"container-stored"
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