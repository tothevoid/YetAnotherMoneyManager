import { z } from 'zod';
import { TFunction } from 'i18next';

export const getCryptocurrencyValidationSchema = (t: TFunction) => z.object({
    id: z.string().optional(),
    name: z.string().min(1, t("validation_field_required")),
    symbol: z.string().min(1, t("validation_field_required")),
    price: z.number().gte(0, t("validation_non_negative_number"))
});

export type CryptocurrencyFormInput = z.infer<ReturnType<typeof getCryptocurrencyValidationSchema>>;