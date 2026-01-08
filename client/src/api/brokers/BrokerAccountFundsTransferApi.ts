import { BrokerAccountFundTransferEntity, BrokerAccountFundTransferEntityRequest, BrokerAccountFundTransferEntityResponse } from '../../models/brokers/BrokerAccountFundTransfer';
import { FundTransfersQuery } from '../../pages/BrokerAccount/hooks/useBrokerAccountFundTransfers';
import { PaginationConfig } from '../../shared/models/PaginationConfig';
import { Nullable } from '../../shared/utilities/nullable';
import { createEntity, deleteEntity, getAllEntitiesByConfig, getPagination, updateEntity } from '../basicApi';
import { prepareBrokerAccountFundsTransfer, prepareBrokerAccountFundsTransferRequest } from './BrokerAccountFundsTransferMapping';

const basicUrl = `BrokerAccountFundsTransfer`;

export const getBrokerAccountFundsTransfers = async (query: FundTransfersQuery): Promise<BrokerAccountFundTransferEntity[]> => {
    return await getAllEntitiesByConfig<FundTransfersQuery, BrokerAccountFundTransferEntityResponse>(`${basicUrl}/GetAll`, query)
        .then((securityTransactions) => {
            return securityTransactions.map(prepareBrokerAccountFundsTransfer)
        })
};

export const getBrokerAccountFundsTransferPagination = async (brokerAccountId: Nullable<string>): Promise<PaginationConfig | void> => {
    const url = brokerAccountId ?
        `${basicUrl}/GetPaginationByBrokerAccount?brokerAccountId=${brokerAccountId}` :
        `${basicUrl}/GetPagination`;
    return getPagination(url);
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
