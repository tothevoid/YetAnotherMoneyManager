import {  z } from 'zod';

export type BrokerFormInput = z.infer<typeof BrokerValidationSchema>;

export const BrokerValidationSchema = z.object({
    id: z.string().optional(),
    name: z.string().min(1)
})