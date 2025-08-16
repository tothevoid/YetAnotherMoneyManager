import {  z } from 'zod';

export type CryptoProviderFormInput = z.infer<typeof CryptoProviderValidationSchema>;

export const CryptoProviderValidationSchema = z.object({
    id: z.string().optional(),
    name: z.string().min(1)
})