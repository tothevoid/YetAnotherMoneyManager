import { string, z } from 'zod';

export type FundFormInput = z.infer<typeof FundValidationSchema>;

export const FundValidationSchema = z.object({
	id: string().optional(),
	name: z.string().min(1),
	balance: z.number()
})