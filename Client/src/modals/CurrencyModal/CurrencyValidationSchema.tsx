import {  z } from 'zod';

export type CurrencyFormInput = z.infer<typeof CurrencyValidationSchema>;

export const CurrencyValidationSchema = z.object({
	id: z.string().optional(),
	name: z.string().min(1),
	active: z.boolean(),
})