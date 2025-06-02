import { string, z } from 'zod';

export type AccountBalanceTransferFormInput = z.infer<typeof AccountBalanceTransferModalValidationSchema>;

export const AccountBalanceTransferModalValidationSchema = z.object({
	id: string().optional(),
	from: z.object({
		id: string().nonempty({message: "From account is not selected"}),
		name: string()
	}, {message: "From account is not selected"}),
    to: z.object({
		id: string().nonempty({message: "To account is not selected"}),
		name: string()
	}, {message: "To account is not selected"}),
    balance: z.number().gt(0),
    fee: z.number().gte(0),
})
.refine(({from, to}) => from.id !== to.id, {
    message: "From acount is the same as the to account",
    path: ["from"],
})