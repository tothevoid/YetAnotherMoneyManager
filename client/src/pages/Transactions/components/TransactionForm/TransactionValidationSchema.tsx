import { z } from 'zod';
import { TFunction } from 'i18next';

export const getTransactionValidationSchema = (t: TFunction) => z.object({
  id: z.string().optional(),
  name: z.string().min(1, t("validation_field_required")),
  date: z.date({ message: t("validation_date_required") }),
  amount: z.number().gt(0, t("validation_positive_number")),
  account: z.object({
    id: z.string().min(1, t("validation_account_required")),
    name: z.string()
  }, { message: t("validation_account_required") }),
  transactionType: z.object({
    id: z.string().min(1, t("validation_transaction_type_required")),
    name: z.string()
  }, { message: t("validation_transaction_type_required") }),
  direction: z.object({
    label: z.string(),
    value: z.string().min(1, t("validation_direction_required"))
  }, { message: t("validation_direction_required") }),
  cashback: z.number().gte(0, t("validation_non_negative_number")),
  isSystem: z.boolean()
});

export type TransactionFormInput = z.infer<ReturnType<typeof getTransactionValidationSchema>>;