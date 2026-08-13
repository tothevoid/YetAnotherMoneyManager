import { z } from 'zod';
import { TFunction } from 'i18next';

export const getCurrencyValidationSchema = (t: TFunction) => z.object({
    id: z.string().min(1, t("validation_currency_required")),
    name: z.string()
}, { message: t("validation_currency_required") });

export const CurrencyValidationSchema = z.object({
    id: z.string().min(1),
    name: z.string()
});