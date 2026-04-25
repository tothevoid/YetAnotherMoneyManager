import { string, z } from 'zod';

export type CryptoAccountCryptocurrencyFormInput = z.infer<typeof CryptoAccountCryptocurrencyValidationSchema>;

export const CryptoAccountCryptocurrencyValidationSchema = z.object({
  id: string().optional(),
  cryptocurrency: z.object({
    id: z.string().nonempty({message: "Cryptocurrency is not selected"}),
    name: z.string()
  }, {message: "Cryptocurrency is not selected"}),
  cryptoAccount: z.object({
    id: z.string().nonempty({message: "Crypto account is not selected"}),
    name: z.string()
  }, {message: "Crypto account is not selected"}),
  quantity: z.number()
    .nonnegative()
    .gte(0)
});