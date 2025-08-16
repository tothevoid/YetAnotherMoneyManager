import { z } from 'zod';

export type DebtFormInput = z.infer<typeof DebtValidationSchema>;

export const DebtValidationSchema = z.object({
    id: z.string().optional(),
    name: z.string().min(1),
    amount: z.number().gt(0),
    currency: z.object({
        id: z.string().nonempty({message: "Currency is not selected"}),
        name: z.string()
    }, {message: "Currency is not selected"}),
    date: z.date()
})