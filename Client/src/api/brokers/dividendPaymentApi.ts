import config from '../../config' 
import { ClientDividendPaymentEntity, ServerDividendPaymentEntity } from '../../models/brokers/DividendPaymentEntity';
import { convertToDateOnly } from '../../shared/utilities/dateUtils';
import { checkPromiseStatus, logPromiseError } from '../../shared/utilities/webApiUtilities';
import { createEntity, deleteEntity, updateEntity } from '../basicApi';
import { prepareClientDividend } from '../securities/dividendApi';

const basicUrl = `${config.api.URL}/DividendPayment`;

export const getDividendPaymentsByBrokerAccount = async (brokerAccountId: string): Promise<ClientDividendPaymentEntity[]> => {
    const entities = await fetch(`${basicUrl}/GetByBrokerAccount?brokerAccountId=${brokerAccountId}`, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then(dividendPayments => dividendPayments.map(prepareClientDividendPayment))
        .catch(logPromiseError);
     
    return entities ?
        entities: 
        [] as ClientDividendPaymentEntity[];
};

export const createDividendPayment = async (modifiedDividendPayment: ClientDividendPaymentEntity): Promise<void> => {
    await createEntity<ServerDividendPaymentEntity>(basicUrl, prepareServerDividendPayment(modifiedDividendPayment))
}

export const updateDividendPayment = async (modifiedDividendPayment: ClientDividendPaymentEntity): Promise<void>=> {
    await updateEntity<ServerDividendPaymentEntity>(basicUrl, 
        prepareServerDividendPayment(modifiedDividendPayment));
}

export const deleteDividendPayment = async (brokerAccountSecurityId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, brokerAccountSecurityId);
}

const prepareClientDividendPayment = (dividendPayment: ServerDividendPaymentEntity): ClientDividendPaymentEntity => {
    const convertedDividendPayment: ClientDividendPaymentEntity = {
        ...dividendPayment,
        receivedAt: new Date(dividendPayment.receivedAt)
    }

    convertedDividendPayment.dividend = prepareClientDividend(convertedDividendPayment.dividend);

    return convertedDividendPayment;
}

const prepareServerDividendPayment = (dividendPayment: ClientDividendPaymentEntity): ServerDividendPaymentEntity => {
    return {
        id: dividendPayment.id,
        brokerAccountId: dividendPayment.brokerAccount.id,
        dividendId: dividendPayment.dividend.id,
        securitiesQuantity: dividendPayment.securitiesQuantity,
        tax: dividendPayment.tax,
        receivedAt: convertToDateOnly(dividendPayment.receivedAt) 
    };
}