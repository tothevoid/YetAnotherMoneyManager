import {  z } from 'zod';

export type BrokerAccountTypeFormInput = z.infer<typeof BrokerAccountTypeValidationSchema>;

export const BrokerAccountTypeValidationSchema = z.object({
    id: z.string().optional(),
    name: z.string().min(1)
})