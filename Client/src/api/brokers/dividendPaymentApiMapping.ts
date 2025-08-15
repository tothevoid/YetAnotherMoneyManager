import { DividendPaymentEntityResponse, DividendPaymentEntity, DividendPaymentEntityRequest } from "../../models/brokers/DividendPaymentEntity";
import { convertToDateOnly } from "../../shared/utilities/dateUtils";

export const prepareDividendPayment = (dividendPayment: DividendPaymentEntityResponse): DividendPaymentEntity => {
    return {
        id: dividendPayment.id,
        dividend: dividendPayment.dividend,
        securitiesQuantity: dividendPayment.securitiesQuantity,
        tax: dividendPayment.tax,
        brokerAccount: dividendPayment.brokerAccount,
        receivedAt: new Date(dividendPayment.receivedAt)
    }
}

export const prepareDividendPaymentRequest = (dividendPayment: DividendPaymentEntity): DividendPaymentEntityRequest => {
    return {
        id: dividendPayment.id,
        brokerAccountId: dividendPayment.brokerAccount.id,
        dividendId: dividendPayment.dividend.id,
        securitiesQuantity: dividendPayment.securitiesQuantity,
        tax: dividendPayment.tax,
        receivedAt: convertToDateOnly(dividendPayment.receivedAt) 
    };
}