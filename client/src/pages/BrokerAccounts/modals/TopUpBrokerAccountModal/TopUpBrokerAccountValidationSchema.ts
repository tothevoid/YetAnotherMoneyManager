import { z } from "zod";

export const TopUpBrokerAccountValidationSchema = z.object({
    id: z.string(),
    date: z.date(),
    brokerAccount: z.object({
        id: z.string().nonempty({message: "Broker account is not selected"}),
        name: z.string()
    }, {message: "Broker account is not selected"}),
    account: z.object({
        id: z.string().nonempty({message: "Account is not selected"}),
        name: z.string()
    }, {message: "Account is not selected"}),
    amount: z.number().min(0.01, "Enter amount"),
    income: z.boolean()
});

export type TopUpBrokerAccountFormInput = z.infer<typeof TopUpBrokerAccountValidationSchema>;
