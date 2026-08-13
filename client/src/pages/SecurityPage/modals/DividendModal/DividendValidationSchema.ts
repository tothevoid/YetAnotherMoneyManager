import { z } from 'zod';
import { TFunction } from 'i18next';

export const getDividendValidationSchema = (t: TFunction) => z.object({
  id: z.string().optional(),
  security: z.object({
    id: z.string().min(1, t("validation_security_required")),
  }, { message: t("validation_security_required") }),
  declarationDate: z.date({ message: t("validation_date_required") }),
  snapshotDate: z.date({ message: t("validation_date_required") }),
  amount: z.number().gt(0, t("validation_positive_number"))
});

export type DividendFormInput = z.infer<ReturnType<typeof getDividendValidationSchema>>;
