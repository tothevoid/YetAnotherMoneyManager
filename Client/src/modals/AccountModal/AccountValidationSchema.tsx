import { string, z } from 'zod';

export type AccountFormInput = z.infer<typeof AccountValidationSchema>;

export const AccountValidationSchema = z.object({
	id: string().optional(),
	name: z.string().min(1),
	balance: z.number()
})