import config from '../../config' 
import { DividendPaymentEntity, DividendPaymentEntityRequest, DividendPaymentEntityResponse } from '../../models/brokers/DividendPaymentEntity';
import { convertToDateOnly } from '../../shared/utilities/dateUtils';
import { checkPromiseStatus, logPromiseError } from '../../shared/utilities/webApiUtilities';
import { createEntity, deleteEntity, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/DividendPayment`;

export const getDividendPaymentsByBrokerAccount = async (brokerAccountId: string): Promise<DividendPaymentEntity[]> => {
    const dividendPayments = await fetch(`${basicUrl}/GetByBrokerAccount?brokerAccountId=${brokerAccountId}`, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then((payments: DividendPaymentEntityResponse[]) => payments.map(prepareDividendPayment))
        .catch(logPromiseError);
     
    return dividendPayments ?? [];
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

const prepareDividendPayment = (dividendPayment: DividendPaymentEntityResponse): DividendPaymentEntity => {
    return {
        id: dividendPayment.id,
        dividend: dividendPayment.dividend,
        securitiesQuantity: dividendPayment.securitiesQuantity,
        tax: dividendPayment.tax,
        brokerAccount: dividendPayment.brokerAccount,
        receivedAt: new Date(dividendPayment.receivedAt)
    }
}

const prepareDividendPaymentRequest = (dividendPayment: DividendPaymentEntity): DividendPaymentEntityRequest => {
    return {
        id: dividendPayment.id,
        brokerAccountId: dividendPayment.brokerAccount.id,
        dividendId: dividendPayment.dividend.id,
        securitiesQuantity: dividendPayment.securitiesQuantity,
        tax: dividendPayment.tax,
        receivedAt: convertToDateOnly(dividendPayment.receivedAt) 
    };
}