import { DebtPaymentEntityResponse, DebtPaymentEntity, DebtPaymentEntityRequest } from "../../models/debts/DebtPaymentEntity";
import { convertToDateOnly } from "../../shared/utilities/dateUtils";

export const prepareDebtPayment = (debtPayment: DebtPaymentEntityResponse): DebtPaymentEntity => {
    return {
        id: debtPayment.id,
        amount: debtPayment.amount,
        debt: debtPayment.debt,
        targetAccount: debtPayment.targetAccount,
        date: new Date(debtPayment.date)
    };
}

export const prepareDebtPaymentRequest = (debtPayment: DebtPaymentEntity): DebtPaymentEntityRequest => {
    return {
        id: debtPayment.id,
        amount: debtPayment.amount,
        debtId: debtPayment.debt.id,
        targetAccountId: debtPayment.targetAccount.id,
        date: convertToDateOnly(debtPayment.date)
    };
}