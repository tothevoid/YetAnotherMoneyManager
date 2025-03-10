import { string, z } from 'zod';

export type DepositFormInput = z.infer<typeof depositValidationSchema>;

const depositValidationSchema = z.object({
  id: string().optional(),
  name: z.string().min(1, { message: 'Name should be at least one symbol' }),
  percentage: z.number()
    .nonnegative()
    .min(0)
    .max(100),
  initialAmount: z.number()
    .nonnegative()
    .min(0, { message: 'Initial amount should be more than zero' }),
  from: z.date().refine(date => date),
  to: z.date(),
});


export default depositValidationSchema;