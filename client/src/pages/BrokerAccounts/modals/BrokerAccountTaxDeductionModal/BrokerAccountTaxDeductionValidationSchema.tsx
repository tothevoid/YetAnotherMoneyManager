import { z } from "zod";
import { TFunction } from "i18next";

export const getBrokerAccountTaxDeductionValidationSchema = (t: TFunction) => z.object({
    id: z.string().optional(),
    name: z.string().min(1, t("validation_field_required")),
    dateApplied: z.date({ message: t("validation_date_required") }),
    brokerAccount: z.object({
        id: z.string().min(1, t("validation_broker_account_required"))
    }, { message: t("validation_broker_account_required") }),
    amount: z.number().gt(0, t("validation_positive_number")),
});

export type BrokerAccountTaxDeductionFormInput = z.infer<ReturnType<typeof getBrokerAccountTaxDeductionValidationSchema>>;
