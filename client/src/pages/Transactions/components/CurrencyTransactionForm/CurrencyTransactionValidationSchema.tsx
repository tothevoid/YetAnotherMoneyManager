import { string, z } from 'zod';

export type CurrencyTransactionFormInput = z.infer<typeof CurrencyTransactionValidationSchema>;

export const CurrencyTransactionValidationSchema = z.object({
  id: string().optional(),
  name: string().nonempty({message: "Transaction name is required"}),
  date: z.date(),
  amount: z.number()
    .gte(0),
  rate: z.number()
    .gte(0),
  sourceAccount: z.object({
    id: string().nonempty({message: "Source account is not selected"}),
    name: string()
  }, {message: "Source account is not selected"}),
  destinationAccount: z.object({
    id: z.string().nonempty({message: "Destination account is not selected"}),
    name: z.string()
  }, {message: "Destination account is not selected"}),
})
