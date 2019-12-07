import React, { Fragment } from 'react'
import './ConfirmModal.scss'
import ReactDOM from 'react-dom';

const getPortalTemplate = (props: any, Content: any) => {
    const {onModalCallback, title = "Confirm", confirmName = "Yes", cancelName = "No"} = props;
    const isComponnetStatefull = !!(
        typeof Content === 'function'
        && Content.prototype
        && Content.prototype.isReactComponent
    );
    let contentRef = React.createRef() as any;
    
    const onButtonClicked = (isConfirmed: boolean) =>{
        let ref = {};
        if (isComponnetStatefull){
            ref = contentRef.current.state;
        }
        onModalCallback(isConfirmed, ref);
    }

    const getAdditionalButton = () => {
        const {additionalName = "", additionalCallback = null} = props;
        const onAdditionalClick = () => {
            let ref = {};
            if (isComponnetStatefull){
                ref = contentRef.current.state;
            }
            additionalCallback(ref);
        }
        if (additionalName !== "" || additionalCallback){
            return <button onClick={onAdditionalClick} className="modal-button">{additionalName}</button>
        }
    }

    const onKeyDown = (event: React.KeyboardEvent<HTMLDivElement>) => {
        //enter
        if (event.keyCode === 13) {
            onButtonClicked(true);
        //esc
        } else if (event.keyCode === 27){
            onButtonClicked(false);
        }
    }

    return <div onKeyDown={onKeyDown} onClick={(e) => {onButtonClicked(false)}} className="modal-wrapper">
        <div onClick={(e) => e.stopPropagation()} className="modal">
            <div className="modal-header">
                <p className="modal-header-title"><b>{title}</b></p>
                <button onClick={() => onButtonClicked(false)} className="modal-close">X</button>
            </div>
            <div className="modal-content">
                {isComponnetStatefull ? <Content {...props} ref={contentRef}></Content>: <Content></Content>}
            </div>
            <div className="modal-buttons">
                <button onClick={() => onButtonClicked(true)} className="modal-button modal-confirm">{confirmName}</button>
                {getAdditionalButton()}
                <button onClick={() => onButtonClicked(false)} className="modal-button modal-cancel">{cancelName}</button>
            </div>
        </div>
    </div>
}

const ConfirmModal = (Content: any, portalId = "portal") => {
    const element = document.getElementById(portalId) as Element;
    return (props: any) => {
        return <Fragment>
            {ReactDOM.createPortal(getPortalTemplate(props, Content), element)}
        </Fragment>
    }
}  

export default ConfirmModal;