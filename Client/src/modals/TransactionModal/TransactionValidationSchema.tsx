import { string, z } from 'zod';

export type TransactionFormInput = z.infer<typeof TransactionValidationSchema>;

export const TransactionValidationSchema = z.object({
  id: string().optional(),
  name: z.string().min(1),
  date: z.date(),
  moneyQuantity: z.number()
    .nonnegative()
    .gt(0),
  fundSource: z.object({
    id: string().nonempty({message: "Fund source is not selected"}),
    name: string()
  }, {message: "Fund source is not selected"}),
  transactionType: string().nonempty(),
  direction: z.object({
    label: string(),
    value: string()
  })
})