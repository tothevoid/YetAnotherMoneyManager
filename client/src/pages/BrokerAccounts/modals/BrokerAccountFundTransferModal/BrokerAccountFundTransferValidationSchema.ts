import { z } from "zod";

export const BrokerAccountFundTransferValidationSchema = z.object({
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
    income: z.object({
        label: z.string(),
        value: z.boolean()
    }, {message: "Transfer type is not selected"})
});

export type BrokerAccountFundTransferFormInput = z.infer<typeof BrokerAccountFundTransferValidationSchema>;
