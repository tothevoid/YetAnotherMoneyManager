import { string, z } from 'zod';

export type DepositFormInput = z.infer<typeof depositValidationSchema>;

export const depositValidationSchema = z.object({
  id: string().optional(),
  name: z.string().min(1),
  percentage: z.number()
    .nonnegative()
    .gt(0)
    .max(100),
  initialAmount: z.number()
    .nonnegative()
    .gt(0),
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