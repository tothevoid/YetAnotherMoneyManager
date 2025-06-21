import { string, z } from 'zod';

export type DividendFormInput = z.infer<typeof DividendValidationSchema>;

export const DividendValidationSchema = z.object({
  id: string().optional(),
  security: z.object({
    id: z.string().nonempty({message: "Security is not selected"}),
  }),
  declarationDate: z.date(),
  snapshotDate: z.date(),
  amount: z.number()
})
