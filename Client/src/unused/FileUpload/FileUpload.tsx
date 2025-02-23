import "./FileUpload.scss"
import React, { Component, ChangeEvent } from "react";
import { TransactionType } from "../../models/TransactionType";
import config from "../../config";
import axios from "axios"
import Button from "../Basic/Button/Button";

type Props = {
    transactionTypes: TransactionType[],
    callback: (type: TransactionType) => void,
    onTypeAdded: (type: TransactionType) => void
}

type State = {
    filePath: string,
    file: File | null
    name: string,
    output: string
}

class FileUpload extends Component<Props, State>{
    state = {
        filePath: "",
        file: null,
        name: "",
        output: "",
        isNewTypeVisible: false
    }

    onFileChanged = (event: ChangeEvent<HTMLInputElement>) =>{
        const files = event.target.files;
        if (files && files.length){
            this.setState({
                filePath: URL.createObjectURL(files[0]),
                file: files[0]
            })
        }
    }
    
    processAddedImage = (newType: TransactionType) => {
        this.props.onTypeAdded(newType);
    }
    
    onSubmit = () => {
        if (this.state.file != null){
            const data = new FormData()
            //fix cast
            const file = (this.state.file!! as File)
            const parts = file.name.split(".");
            if (parts.length >= 2){
                const extension = parts[parts.length - 1];
                if (this.getIsExtensionAvailable(extension)){
                    data.append("name", this.state.name);
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

    onTypeNameUpdated = ({ target }: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({ name: target.value});
    }

    render = () => {
        return <div className="file-upload-container">
            {
                (this.state.filePath) ?
                <img alt="icon" className="type-image" src={this.state.filePath}/> :
                null
            }
            <label className="type-file-label">
                <input onChange={this.onFileChanged} className="type-file" type="file"/>
                Upload icon
            </label>
            <div className="file-upload-field">
                <label className="file-label">Type name</label>
                <input name="name" onChange={this.onTypeNameUpdated} value={this.state.name} type="text"></input>
            </div>
            <Button text="+" onClick={this.onSubmit} classes="type-add-button"/>
        </div>
    }
}

export default FileUpload;