import { z } from 'zod';

export type SecurityFormInput = z.infer<typeof SecurityValidationSchema>;

export const SecurityValidationSchema = z.object({
    id: z.string().optional(),
    name: z.string().min(1),
    ticker: z.string().min(1),
    type: z.object({
        id: z.string().nonempty({message: "Type is not selected"}),
        name: z.string()
    }, {message: "Type is not selected"}),
})