import { boolean, string, z } from 'zod';

export type AccountFormInput = z.infer<typeof AccountValidationSchema>;

export const AccountValidationSchema = z.object({
	id: string().optional(),
	name: z.string().min(1),
	balance: z.number(),
	currency: z.object({
		id: string().nonempty({message: "Currency is not selected"}),
		name: string()
	}, {message: "Currency is not selected"}),
})