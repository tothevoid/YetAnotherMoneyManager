import {  z } from 'zod';

export type BankFormInput = z.infer<typeof BankValidationSchema>;

export const BankValidationSchema = z.object({
    id: z.string().optional(),
    name: z.string().min(1)
})