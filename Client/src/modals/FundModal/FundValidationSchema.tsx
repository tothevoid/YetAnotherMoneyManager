import { string, z } from 'zod';

export type FundFormInput = z.infer<typeof fundValidationSchema>;

export const fundValidationSchema = z.object({
	id: string().optional(),
	name: z.string().min(1),
	balance: z.number()
})