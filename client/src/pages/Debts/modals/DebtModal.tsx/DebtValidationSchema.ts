import { z } from 'zod';
import { TFunction } from 'i18next';

export const getDebtValidationSchema = (t: TFunction) => z.object({
    id: z.string().optional(),
    name: z.string().min(1, t("validation_field_required")),
    amount: z.number().gt(0, t("validation_positive_number")),
    currency: z.object({
        id: z.string().min(1, t("validation_currency_required")),
        name: z.string()
    }, { message: t("validation_currency_required") }),
    date: z.date({ message: t("validation_date_required") })
});

export type DebtFormInput = z.infer<ReturnType<typeof getDebtValidationSchema>>;