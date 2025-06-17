import {  z } from 'zod';

export type CryptoAccountFormInput = z.infer<typeof CryptoAccountValidationSchema>;

export const CryptoAccountValidationSchema = z.object({
    id: z.string().optional(),
    name: z.string().min(1),
    cryptoProvider: z.object({
        id: z.string().nonempty({message: "Crypto provider is not selected"}),
        name: z.string()
    }, {message: "Crypto provider is not selected"}),
})