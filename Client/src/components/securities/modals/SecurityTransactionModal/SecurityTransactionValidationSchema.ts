import { z } from 'zod';

export type SecurityTransactionFormInput = z.infer<typeof SecurityTransactionValidationSchema>;

export const SecurityTransactionValidationSchema = z.object({
    id: z.string().optional(),
    security: z.object({
        id: z.string().nonempty({message: "Security is not selected"}),
        name: z.string()
    }, {message: "Security is not selected"}),
    brokerAccount: z.object({
        id: z.string().nonempty({message: "Broker account is not selected"}),
        name: z.string()
    }, {message: "Broker account is not selected"}),
    price: z.number().gte(0),
    date: z.date(),
    commission: z.number().gte(0),
    tax: z.number().gte(0),
    quantity: z.number().gt(0)
})