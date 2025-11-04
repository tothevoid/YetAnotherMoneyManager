import config from '../../config' 
import { DividendPaymentEntity, DividendPaymentEntityRequest, DividendPaymentEntityResponse } from '../../models/brokers/DividendPaymentEntity';
import { PaginationConfig } from '../../shared/models/PaginationConfig';
import { checkPromiseStatus, logPromiseError } from '../../shared/utilities/webApiUtilities';
import { createEntity, deleteEntity, updateEntity } from '../basicApi';
import { prepareDividendPayment, prepareDividendPaymentRequest } from './dividendPaymentApiMapping';

const basicUrl = `${config.api.URL}/DividendPayment`;

export const getDividendPaymentsByBrokerAccount = async (brokerAccountId: string): Promise<DividendPaymentEntity[]> => {
    const dividendPayments = await fetch(`${basicUrl}/GetByBrokerAccount?brokerAccountId=${brokerAccountId}`, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then((payments: DividendPaymentEntityResponse[]) => payments.map(prepareDividendPayment))
        .catch(logPromiseError);
     
    return dividendPayments ?? [];
};

export const getBrokerAccountFundsTransferPagination = async (brokerAccountId: string): Promise<PaginationConfig | void> => {
    return await fetch(`${basicUrl}/GetPagination?brokerAccountId=${brokerAccountId}`, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
};

export const getEarningsByBrokerAccount = async (brokerAccountId: string): Promise<number> => {
    return await fetch(`${basicUrl}/GetEarningsByBrokerAccount?brokerAccountId=${brokerAccountId}`, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
};

export const createDividendPayment = async (modifiedDividendPayment: DividendPaymentEntity): Promise<void> => {
    await createEntity<DividendPaymentEntityRequest, DividendPaymentEntityResponse>(basicUrl, prepareDividendPaymentRequest(modifiedDividendPayment))
}

export const updateDividendPayment = async (modifiedDividendPayment: DividendPaymentEntity): Promise<void>=> {
    await updateEntity<DividendPaymentEntityRequest>(basicUrl, prepareDividendPaymentRequest(modifiedDividendPayment));
}

export const deleteDividendPayment = async (brokerAccountSecurityId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, brokerAccountSecurityId);
}