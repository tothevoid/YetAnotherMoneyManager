import { Nullable } from "../../shared/utilities/nullable";

export interface SecurityTransactionsRequest{
    recordsQuantity: number,
    pageIndex: number,
    brokerAccountId: Nullable<string>
}