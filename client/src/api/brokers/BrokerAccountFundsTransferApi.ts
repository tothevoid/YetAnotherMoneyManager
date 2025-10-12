import config from '../../config' 
import { BrokerAccountFundTransferEntity, BrokerAccountFundTransferEntityRequest, BrokerAccountFundTransferEntityResponse } from '../../models/brokers/BrokerAccountFundTransfer';
import { createEntity, deleteEntity, getAllEntities, updateEntity } from '../basicApi';
import { prepareBrokerAccountFundsTransfer, prepareBrokerAccountFundsTransferRequest } from './BrokerAccountFundsTransferMapping';

const basicUrl = `${config.api.URL}/BrokerAccountFundsTransfer`;

export const getBrokerAccountFundsTransfers = async (brokerAccountId: string): Promise<BrokerAccountFundTransferEntity[]> => {
    return await getAllEntities<BrokerAccountFundTransferEntityResponse>(`${basicUrl}?brokerAccountId=${brokerAccountId}`)
        .then(brokerAccounts => brokerAccounts.map(prepareBrokerAccountFundsTransfer));
};

export const createBrokerAccountFundsTransfer = async (addedBrokerAccountFundsTransfer: BrokerAccountFundTransferEntity): Promise<BrokerAccountFundTransferEntityResponse | void> => {
    return await createEntity<BrokerAccountFundTransferEntityRequest, BrokerAccountFundTransferEntityResponse>(basicUrl, 
        prepareBrokerAccountFundsTransferRequest(addedBrokerAccountFundsTransfer));
}

export const updateBrokerAccountFundsTransfer = async (updatedBrokerAccountFundsTransfer: BrokerAccountFundTransferEntity): Promise<boolean> => {
    return await updateEntity(basicUrl, prepareBrokerAccountFundsTransferRequest(updatedBrokerAccountFundsTransfer));
}

export const deleteBrokerAccountFundsTransfer = async (brokerAccountFundsTransferId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, brokerAccountFundsTransferId);
}
