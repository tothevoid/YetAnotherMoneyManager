import React, { Fragment } from 'react'
import './ConfirmModal.css'
import ReactDOM from 'react-dom';

const getPortalTemplate = (props: any) => {
    const {text, onModalCallback} = props;
   
    const onButtonClicked = (isConfirmed: boolean) => (e:any) => 
        onModalCallback(isConfirmed);

    return <div className="modal-background">
        <div className="modal">
            <p className="modal-text">{text}</p>
            <div className="modal-buttons-container">
                <button onClick={onButtonClicked(true)} className="modal-button">Yes</button>
                <button onClick={onButtonClicked(false)} className="modal-button">No</button>
            </div>
        </div>
    </div>
}

const ConfirmModal = (props: any) => {
    const element = document.getElementById("portal") as Element;
    return <Fragment>
        {ReactDOM.createPortal(getPortalTemplate(props), element)}
    </Fragment>
}  

export default ConfirmModal;