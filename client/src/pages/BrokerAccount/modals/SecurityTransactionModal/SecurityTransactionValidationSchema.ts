import { z } from 'zod';
import { TFunction } from 'i18next';

export const getSecurityTransactionValidationSchema = (t: TFunction) => z.object({
    id: z.string().optional(),
    security: z.object({
        id: z.string().min(1, t("validation_security_required")),
        name: z.string()
    }, { message: t("validation_security_required") }),
    brokerAccount: z.object({
        id: z.string().min(1, t("validation_broker_account_required")),
    }, { message: t("validation_broker_account_required") }),
    price: z.number().gte(0, t("validation_non_negative_number")),
    date: z.date({ message: t("validation_date_required") }),
    brokerCommission: z.number().gte(0, t("validation_non_negative_number")),
    stockExchangeCommission: z.number().gte(0, t("validation_non_negative_number")),
    tax: z.number().gte(0, t("validation_non_negative_number")),
    quantity: z.number().gt(0, t("validation_positive_number")),
    operation: z.object({
        label: z.string(),
        value: z.string().min(1, t("validation_operation_required"))
    }, { message: t("validation_operation_required") }),
});

export type SecurityTransactionFormInput = z.infer<ReturnType<typeof getSecurityTransactionValidationSchema>>;