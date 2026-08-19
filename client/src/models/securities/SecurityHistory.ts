import { SecurityHistoryValue } from "./SecurityHistoryValue";

export interface SecurityHistory {
    values: SecurityHistoryValue[];
    startPrice: number;
    endPrice: number;
    diff: number;
    diffPercent: number;
    minPrice: number;
    maxPrice: number;
    avgPrice: number;
}
