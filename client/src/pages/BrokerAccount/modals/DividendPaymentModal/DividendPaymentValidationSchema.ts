import { z } from 'zod';
import { TFunction } from 'i18next';

export const getDividendPaymentValidationSchema = (t: TFunction) => z.object({
    id: z.string().optional(),
    dividend: z.object({
        id: z.string().min(1, t("validation_dividend_required")),
        amount: z.number()
    }, { message: t("validation_dividend_required") }),
    brokerAccount: z.object({
        id: z.string().min(1, t("validation_broker_account_required")),
    }, { message: t("validation_broker_account_required") }),
    securitiesQuantity: z.number().gt(0, t("validation_positive_number")),
    tax: z.number().gte(0, t("validation_non_negative_number")),
    receivedAt: z.date({ message: t("validation_date_required") })
});

export type DividendPaymentFormInput = z.infer<ReturnType<typeof getDividendPaymentValidationSchema>>;
