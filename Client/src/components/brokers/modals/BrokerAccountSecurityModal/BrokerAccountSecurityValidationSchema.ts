import {  z } from 'zod';

export type BrokerAccountSecurityFormInput = z.infer<typeof BrokerAccountSecurityValidationSchema>;

export const BrokerAccountSecurityValidationSchema = z.object({
    id: z.string().optional(),
    brokerAccount: z.object({
        id: z.string().nonempty({message: "Broker account is not selected"}),
        name: z.string()
    }, {message: "Broker account is not selected"}),
    security: z.object({
        id: z.string().nonempty({message: "Security is not selected"}),
        name: z.string()
    }, {message: "Security is not selected"}),
    quantity: z.number().gt(0),
    initialPrice: z.number().gt(0)
})