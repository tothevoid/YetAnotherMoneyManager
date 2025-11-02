import {  z } from 'zod';

export type AccountFormInput = z.infer<typeof AccountValidationSchema>;

export const AccountValidationSchema = z.object({
	id: z.string().optional(),
	name: z.string().min(1),
	bank: z.object({
        id: z.string(),
        name: z.string()
    }).optional(),
	balance: z.number(),
	active: z.boolean(),
	createdOn: z.date(),
	accountType: z.object({
		id: z.string().nonempty({message: "Account type is not selected"}),
		name: z.string()
	}, {message: "Account type is not selected"}),
	currency: z.object({
		id: z.string().nonempty({message: "Currency is not selected"}),
		name: z.string()
	}, {message: "Currency is not selected"}),
})