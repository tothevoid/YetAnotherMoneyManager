import { SecurityTransactionEntity, SecurityTransactionEntityRequest, SecurityTransactionEntityResponse } from "../../models/securities/SecurityTransactionEntity"
import { convertToDateOnly } from "../../shared/utilities/dateUtils"

export const prepareSecurityTransactionRequest = (securityTransaction: SecurityTransactionEntity): SecurityTransactionEntityRequest => {
    return {
        id: securityTransaction.id,
        brokerCommission: securityTransaction.brokerCommission,
        stockExchangeCommission: securityTransaction.stockExchangeCommission,
        price: securityTransaction.price,
        quantity: securityTransaction.quantity,
        tax: securityTransaction.tax,
        date: convertToDateOnly(securityTransaction.date),
        brokerAccountId: securityTransaction.brokerAccount.id,
        securityId: securityTransaction.security.id,
    }
}

export const prepareSecurityTransaction = (securityTransaction: SecurityTransactionEntityResponse): SecurityTransactionEntity => {
    return {
        id: securityTransaction.id,
        brokerCommission: securityTransaction.brokerCommission,
        stockExchangeCommission: securityTransaction.stockExchangeCommission,
        price: securityTransaction.price,
        quantity: securityTransaction.quantity,
        tax: securityTransaction.tax,
        date: new Date(securityTransaction.date),
        brokerAccount: securityTransaction.brokerAccount,
        security: securityTransaction.security,
    }
}
