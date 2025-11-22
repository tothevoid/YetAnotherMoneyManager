import { z } from 'zod';

export type DebtPaymentFormInput = z.infer<typeof DebtPaymentValidationSchema>;

export const DebtPaymentValidationSchema = z.object({
    id: z.string().optional(),
    amount: z.number().gt(0),
    targetAccount: z.object({
        id: z.string().nonempty({message: "Target account is not selected"}),
        name: z.string()
    }, {message: "Target account is not selected"}),
    debt: z.object({
        id: z.string().nonempty({message: "Debt is not selected"}),
        name: z.string()
    }, {message: "Debt is not selected"}),
    date: z.date(),
    isPercentagePayment: z.boolean()
})