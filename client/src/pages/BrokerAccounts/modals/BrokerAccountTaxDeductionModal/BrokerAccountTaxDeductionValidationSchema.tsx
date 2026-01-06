import { z } from "zod";

export const BrokerAccountTaxDeductionValidationSchema = z.object({
    id: z.string(),
    name: z.string(),
    dateApplied: z.date(),
    brokerAccount: z.object({
        id: z.string().nonempty({message: "Broker account is not selected"})
    }, {message: "Broker account is not selected"}),
    amount: z.number().min(0.01, "Enter amount"),
});

export type BrokerAccountTaxDeductionFormInput = z.infer<typeof BrokerAccountTaxDeductionValidationSchema>;
