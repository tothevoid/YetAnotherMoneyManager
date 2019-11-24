import "./TransactionTypeForm.scss"
import React, { Component, ChangeEvent } from "react";
import { TransactionType } from "../../models/TransactionType";
import config from "../../config";
import axios from "axios"

type Props = {
    types: TransactionType[],
    callback: () => void
}

type State = {
    filePath: string,
    file: File | null
    name: string,
    output: string
}

class TransactionTypeForm extends Component<Props, State>{
    state = {
        filePath: "",
        file: null,
        name: "",
        output: ""
    }

    handleChange = (event: ChangeEvent<HTMLInputElement>) =>{
        const files = event.target.files;
        if (files && files.length){
            this.setState({
                filePath: URL.createObjectURL(files[0]),
                file: files[0]
            })
        }
    }
    
    processAddedImage = (data: TransactionType) => {
        this.setState({output: this.getImageLinkByModel(data)});
    }

    getImageLinkByModel = (transactionType: TransactionType) => 
        `${config.api.URL}/images/${transactionType.id}.${transactionType.extension}`

    onSubmit = () => {
        if (this.state.file != null){
            const data = new FormData()
            //fix cast
            const file = (this.state.file!! as File)
            const parts = file.name.split(".");
            if (parts.length >= 2){
                const extension = parts[parts.length - 1];
                if (this.getIsExtensionAvailable(extension)){
                    data.append("name", parts[0]);
                    data.append("extension", extension);
                    data.append("file", file);
                    const url = `${config.api.URL}/TransactionType`;
                    axios.put(url, data, {
                        headers: {
                            'content-type': 'multipart/form-data',
                        },
                    })
                    .then(result => this.processAddedImage(result.data));
                } else {
                    const extensions = this.getSupportedExtensions().join(", ");
                    this.setState({output: `Incorrect file type. Available extensions: (${extensions})`});
                }
            } else {
                this.setState({output: "Incorrect filename"});
            }
        }
    }

    getSupportedExtensions = () => 
       ["png", "jpg", "svg"];

    getIsExtensionAvailable = (extName: string): boolean => {
        const supportedExtensions = this.getSupportedExtensions();
        return supportedExtensions.includes(extName);
    }

    render = () => {
        const elms = this.props.types || [];
        return <div>
            {
                elms.forEach((transaction: TransactionType) => {
                    return <div className="type">
                        <img alt="added" className="type-img" src={this.getImageLinkByModel(transaction)}></img>
                        <p className="type-sign">{transaction.name}</p>
                    </div>
                })
            }
            {
                (this.state.output) ?
                    <img alt="icon" className="type-image" src={this.state.output}/> : 
                    null
            }
            <label className="file-label">Type name</label>
            <input type="file-input"></input>
            <label className="file-label">Type icon</label>
            {
                (this.state.filePath) ?
                <img alt="icon" className="type-image" src={this.state.filePath}/> :
                null
            }
            <label className="type-file-label">
                <input onChange={this.handleChange} className="type-file" type="file"/>
                Custom Upload
            </label>
            <button onClick={()=>this.onSubmit()} type="submit" className="type-add-button">+</button>
        </div>
    }
}

export default TransactionTypeForm;