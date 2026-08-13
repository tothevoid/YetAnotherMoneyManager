import { z } from 'zod';
import { TFunction } from 'i18next';

export const getCryptoAccountValidationSchema = (t: TFunction) => z.object({
    id: z.string().optional(),
    name: z.string().min(1, t("validation_field_required")),
    cryptoProvider: z.object({
        id: z.string().min(1, t("validation_crypto_provider_required")),
        name: z.string()
    }, { message: t("validation_crypto_provider_required") }),
});

export type CryptoAccountFormInput = z.infer<ReturnType<typeof getCryptoAccountValidationSchema>>;