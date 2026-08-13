import { z } from 'zod';
import { TFunction } from 'i18next';

export const getBrokerAccountSecurityValidationSchema = (t: TFunction) => z.object({
    id: z.string().optional(),
    brokerAccount: z.object({
        id: z.string().min(1, t("validation_broker_account_required")),
        name: z.string()
    }, { message: t("validation_broker_account_required") }),
    security: z.object({
        id: z.string().min(1, t("validation_security_required")),
        name: z.string()
    }, { message: t("validation_security_required") }),
    quantity: z.number().gt(0, t("validation_positive_number")),
    price: z.number().gt(0, t("validation_positive_number"))
});

export type BrokerAccountSecurityFormInput = z.infer<ReturnType<typeof getBrokerAccountSecurityValidationSchema>>;