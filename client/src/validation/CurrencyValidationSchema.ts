import {  z } from 'zod';

export const CurrencyValidationSchema = z.object({
    id: z.string().nonempty({message: "Currency is not selected"}),
    name: z.string()
}, { message: "Currency is not selected"});