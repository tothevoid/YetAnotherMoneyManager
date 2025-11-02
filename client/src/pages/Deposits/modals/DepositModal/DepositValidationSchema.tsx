import { string, z } from 'zod';

export type DepositFormInput = z.infer<typeof DepositValidationSchema>;

export const DepositValidationSchema = z.object({
  id: string().optional(),
  name: z.string().min(1),
  bank: z.object({
    id: z.string().nonempty({message: "Bank is not selected"}),
    name: z.string()
  }, {message: "Bank is not selected"}),
  percentage: z.number()
    .nonnegative()
    .gt(0)
    .max(100),
  initialAmount: z.number()
    .nonnegative()
    .gt(0),
  estimatedEarn: z.number()
    .nonnegative()
    .gt(0),
  currency: z.object({
    id: z.string().nonempty({message: "Currency is not selected"}),
    name: z.string()
  }, {message: "Currency is not selected"}),
  from: z.date(),
  to: z.date(),
})
.refine(({from, to}) => to > from, {
  message: "End date must be later than start date",
  path: ["to"],
})
.refine(({from, to}) => from < to, {
  message: "Start date must be earlier than end date",
  path: ["from"],
});