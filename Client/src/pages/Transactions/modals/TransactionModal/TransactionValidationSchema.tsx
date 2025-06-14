import { string, z } from 'zod';

export type TransactionFormInput = z.infer<typeof TransactionValidationSchema>;

export const TransactionValidationSchema = z.object({
  id: string().optional(),
  name: z.string().min(1),
  date: z.date(),
  amount: z.number()
    .nonnegative()
    .gt(0),
  account: z.object({
    id: string().nonempty({message: "Account is not selected"}),
    name: string()
  }, {message: "Account is not selected"}),
  transactionType: z.object({
    id: z.string().nonempty({message: "Transaction type is not selected"}),
    name: z.string()
  }, {message: "Transaction type is not selected"}),
  direction: z.object({
    label: string(),
    value: string()
  }),
  cashback: z.number()
    .nonnegative()
    .gte(0),
  isSystem: z.boolean()
})