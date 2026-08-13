import { z } from 'zod';
import { TFunction } from 'i18next';

export const getDebtPaymentValidationSchema = (t: TFunction) => z.object({
    id: z.string().optional(),
    amount: z.number().gt(0, t("validation_positive_number")),
    targetAccount: z.object({
        id: z.string().min(1, t("validation_target_account_required")),
        name: z.string()
    }, { message: t("validation_target_account_required") }),
    debt: z.object({
        id: z.string().min(1, t("validation_debt_required")),
        name: z.string()
    }, { message: t("validation_debt_required") }),
    date: z.date({ message: t("validation_date_required") }),
    isPercentagePayment: z.boolean()
});

export type DebtPaymentFormInput = z.infer<ReturnType<typeof getDebtPaymentValidationSchema>>;