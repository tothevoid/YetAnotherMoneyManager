import { z } from 'zod';
import { TFunction } from 'i18next';

export const getCurrencyTransactionValidationSchema = (t: TFunction) => z.object({
  id: z.string().optional(),
  name: z.string().min(1, t("validation_field_required")),
  date: z.date({ message: t("validation_date_required") }),
  amount: z.number().gt(0, t("validation_positive_number")),
  sourceAmount: z.number().optional(),
  rate: z.number().gt(0, t("validation_positive_number")),
  sourceAccount: z.object({
    id: z.string().min(1, t("validation_source_account_required")),
    name: z.string()
  }, { message: t("validation_source_account_required") }),
  destinationAccount: z.object({
    id: z.string().min(1, t("validation_destination_account_required")),
    name: z.string()
  }, { message: t("validation_destination_account_required") }),
});

export type CurrencyTransactionFormInput = z.infer<ReturnType<typeof getCurrencyTransactionValidationSchema>>;
