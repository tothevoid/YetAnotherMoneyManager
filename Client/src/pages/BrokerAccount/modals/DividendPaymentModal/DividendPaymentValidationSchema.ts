import { z } from 'zod';

export type DividendPaymentFormInput = z.infer<typeof DividendPaymentValidationSchema>;

export const DividendPaymentValidationSchema = z.object({
    id: z.string().optional(),
    dividend: z.object({
        id: z.string().nonempty({message: "Broker account is not selected"}),
        amount: z.number()
    }),
    brokerAccount: z.object({
        id: z.string().nonempty({message: "Broker account is not selected"}),
    }),
    securitiesQuantity: z.number().gt(0),
    tax: z.number().gte(0),
    receivedAt: z.date()
})

