import config from '../../config' 
import { BrokerAccountFundTransferEntity, BrokerAccountFundTransferEntityRequest, BrokerAccountFundTransferEntityResponse } from '../../models/brokers/BrokerAccountFundTransfer';
import { FundTransfersQuery } from '../../pages/BrokerAccount/hooks/useBrokerAccountFundTransfers';
import { PaginationConfig } from '../../shared/models/PaginationConfig';
import { checkPromiseStatus, logPromiseError } from '../../shared/utilities/webApiUtilities';
import { createEntity, deleteEntity, getAllEntitiesByConfig, updateEntity } from '../basicApi';
import { prepareBrokerAccountFundsTransfer, prepareBrokerAccountFundsTransferRequest } from './BrokerAccountFundsTransferMapping';

const basicUrl = `${config.api.URL}/BrokerAccountFundsTransfer`;

export const getBrokerAccountFundsTransfers = async (query: FundTransfersQuery): Promise<BrokerAccountFundTransferEntity[]> => {
    return await getAllEntitiesByConfig<FundTransfersQuery, BrokerAccountFundTransferEntityResponse>(`${basicUrl}/GetAll`, query)
        .then((securityTransactions) => {
            return securityTransactions.map(prepareBrokerAccountFundsTransfer)
        })
};

export const getBrokerAccountFundsTransferPagination = async (brokerAccountId: string): Promise<PaginationConfig | void> => {
    return await fetch(`${basicUrl}/GetPagination?brokerAccountId=${brokerAccountId}`, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
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
