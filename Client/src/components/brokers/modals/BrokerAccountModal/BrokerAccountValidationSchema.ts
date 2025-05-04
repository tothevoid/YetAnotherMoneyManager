import {  z } from 'zod';
import { CurrencyValidationSchema } from '../../../../validation/CurrencyValidationSchema';

export type BrokerAccountFormInput = z.infer<typeof BrokerAccountValidationSchema>;

export const BrokerAccountValidationSchema = z.object({
    id: z.string().optional(),
    name: z.string().min(1),
    type: z.object({
        id: z.string().nonempty({message: "Broker account type is not selected"}),
        name: z.string()
    }, {message: "Broker account type is not selected"}),
    currency: CurrencyValidationSchema,
    broker: z.object({
        id: z.string().nonempty({message: "Broker is not selected"}),
        name: z.string()
    }, {message: "Broker is not selected"}),
    initialValue: z.number().optional(),
    currentValue: z.number().optional()
})