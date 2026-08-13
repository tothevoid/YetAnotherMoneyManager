import { z } from "zod";
import { TFunction } from "i18next";

export const getBrokerAccountFundTransferValidationSchema = (t: TFunction) => z.object({
    id: z.string().optional(),
    date: z.date({ message: t("validation_date_required") }),
    brokerAccount: z.object({
        id: z.string().min(1, t("validation_broker_account_required"))
    }, { message: t("validation_broker_account_required") }),
    account: z.object({
        id: z.string().min(1, t("validation_account_required")),
        name: z.string()
    }, { message: t("validation_account_required") }),
    amount: z.number().gt(0, t("validation_positive_number")),
    income: z.object({
        label: z.string(),
        value: z.boolean()
    }, { message: t("validation_direction_required") })
});

export type BrokerAccountFundTransferFormInput = z.infer<ReturnType<typeof getBrokerAccountFundTransferValidationSchema>>;
