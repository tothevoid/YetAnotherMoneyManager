import {  z } from 'zod';

export type TransactionTypeFormInput = z.infer<typeof TransactionTypeValidationSchema>;

export const TransactionTypeValidationSchema = z.object({
	id: z.string().optional(),
	name: z.string().min(1),
	active: z.boolean(),
})