import React, { Fragment } from 'react'
import './ConfirmModal.css'
import ReactDOM from 'react-dom';

const getPortalTemplate = (props: any, Content: any) => {
    const {onModalCallback} = props;
    const isComponnetStatefull = !!(
        typeof Content === 'function'
        && Content.prototype
        && Content.prototype.isReactComponent
    );
    debugger;
    let contentRef = React.createRef() as any;
    
    const onButtonClicked = (isConfirmed: boolean) => (e:any) =>{
        let ref = {};
        if (isComponnetStatefull){
            ref = contentRef.current.state;
        }
        onModalCallback(isConfirmed, ref);
    }

    return <div className="modal-background">
        <div className="modal">
            {isComponnetStatefull ? <Content ref={contentRef}></Content>: <Content></Content>}
            <div className="modal-buttons-container">
                <button onClick={onButtonClicked(true)} className="modal-button">Yes</button>
                <button onClick={onButtonClicked(false)} className="modal-button">No</button>
            </div>
        </div>
    </div>
}

const ConfirmModal = (Content: any) => {
    const element = document.getElementById("portal") as Element;
    return (props: any) => {
        return <Fragment>
            {ReactDOM.createPortal(getPortalTemplate(props, Content), element)}
        </Fragment>
    }
}  

export default ConfirmModal;