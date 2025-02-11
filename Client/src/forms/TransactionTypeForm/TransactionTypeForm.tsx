import "./TransactionTypeForm.scss"
import { Component } from "react";
import { TransactionType } from "../../models/TransactionType";
import config from "../../config";
import hideableHOC from "../../HOC/Hideable/Hideable";
import FileUpload from "../../components/FileUpload/FileUpload";

type Props = {
    transactionTypes: TransactionType[],
    callback: (type: TransactionType) => void,
    onTypeAdded: (type: TransactionType) => void
}

class TransactionTypeForm extends Component<Props, any>{

    onTypeSelected = (type: TransactionType) => {
        this.props.callback(type);
    }

    getImageLinkByModel = (transactionType: TransactionType) => 
        `${config.api.URL}/images/${transactionType.id}.${transactionType.extension}`

    render = () => {
        const types = this.props.transactionTypes || [];
        const FileUploadForm = hideableHOC("Add type")(FileUpload);
        return <div>
            <div className="types-container">
                {
                    types.map((type: TransactionType) => {
                        return <div onClick={() => this.onTypeSelected(type)} key={type.id} className="transaction-type">
                            <img alt={type.name} className="transaction-type-image type-image" src={this.getImageLinkByModel(type)}></img>
                            <p className="transaction-type-name">{type.name}</p>
                        </div>
                    })
                }
            </div>
            {<FileUploadForm onTypeAdded={this.props.onTypeAdded}></FileUploadForm>}
        </div>
    }
}

export default TransactionTypeForm;