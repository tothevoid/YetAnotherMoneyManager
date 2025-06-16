import { z } from 'zod';

export type CryptocurrencyFormInput = z.infer<typeof CryptocurrencyValidationSchema>;

export const CryptocurrencyValidationSchema = z.object({
    id: z.string().optional(),
    name: z.string().min(1),
    symbol: z.string().min(1),
    price: z.number()
})