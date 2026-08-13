import { z } from 'zod';
import { TFunction } from 'i18next';

export const getAccountBalanceTransferValidationSchema = (t: TFunction) => z.object({
	id: z.string().optional(),
	from: z.object({
		id: z.string().min(1, t("validation_source_account_required")),
		name: z.string()
	}, { message: t("validation_source_account_required") }),
	to: z.object({
		id: z.string().min(1, t("validation_destination_account_required")),
		name: z.string()
	}, { message: t("validation_destination_account_required") }),
	balance: z.number().gt(0, t("validation_positive_number")),
	fee: z.number().gte(0, t("validation_non_negative_number")),
})
.refine(({ from, to }) => from.id !== to.id, {
	message: t("validation_same_account"),
	path: ["from"],
});

export type AccountBalanceTransferFormInput = z.infer<ReturnType<typeof getAccountBalanceTransferValidationSchema>>;