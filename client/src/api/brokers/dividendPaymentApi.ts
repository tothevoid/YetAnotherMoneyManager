import { DividendPaymentEntity, DividendPaymentEntityRequest, DividendPaymentEntityResponse } from '../../models/brokers/DividendPaymentEntity';
import { DividendPaymentsQuery } from '../../pages/BrokerAccount/hooks/useDividendPayments';
import { PaginationConfig } from '../../shared/models/PaginationConfig';
import { Nullable } from '../../shared/utilities/nullable';
import { createEntity, deleteEntity, getAllEntitiesByConfig, getEntity, getPagination, updateEntity } from '../basicApi';
import { prepareDividendPayment, prepareDividendPaymentRequest } from './dividendPaymentApiMapping';

const basicUrl = `DividendPayment`;

export const getDividendPaymentsByBrokerAccount = async (query: DividendPaymentsQuery): Promise<DividendPaymentEntity[]> => {
    return await getAllEntitiesByConfig<DividendPaymentsQuery, DividendPaymentEntityResponse>(`${basicUrl}/GetAll`, query)
        .then((dividendPayment) => {
            return dividendPayment.map(prepareDividendPayment)
        });
};

export const getDividendPaymentsPagination = async (brokerAccountId: Nullable<string>): Promise<PaginationConfig | void> => {
    const url = brokerAccountId ?
        `${basicUrl}/GetPagination?brokerAccountId=${brokerAccountId}`:
        `${basicUrl}/GetPagination`;
    return await getPagination(url);
};

export const getEarningsByBrokerAccount = async (brokerAccountId: string): Promise<number> => {
    return await getEntity<number>(`${basicUrl}/GetEarningsByBrokerAccount?brokerAccountId=${brokerAccountId}`) ?? 0;
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